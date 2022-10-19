using GameCommon;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


namespace GameEditor
{
    public class GenerateMessageTool
    {
        private static string m_messageRequestSenderStr =
@"using GameCommon;
using System.Threading.Tasks;

namespace GameClient
{
    [GenerateAutoClass]
    public class #Request#Sender : AMessageRequestSender<#Request#,#Response#>
    {
        public async Task<bool> SendMessage(#Params#)
        {
#Context#
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            
            return true;
        }
    }
}";
        private static string m_messageRequestHanderTxt =
@"using GameCommon;
using System.Threading.Tasks;

namespace GameServer
{
    [GenerateAutoClass]
    public class #Request#Hander : AMessageRequestHander<#Request#,#Response#>
    {
        protected override async Task OnMessage(UserData userData, #Request# request)
        {
            await Task.CompletedTask;
        }
    }
}";
        private static string m_messageNoticeSenderTxt =
@"using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class #Notice#Sender : AMessageNoticeSender<#Notice#>
    {
        public void SendMessage(List<long> userIds, #Params#)
        {
#Context#
            SendMessageCore(userIds);
        }
    }
}";
        private static string m_messageNoticeHanderTxt =
@"using GameCommon;

namespace GameClient
{
    [GenerateAutoClass]
    public class #Notice#Hander : AMessageNoticeHander<#Notice#>
    {
        protected override void OnMessage(#Notice# notice)
        {
            
        }
    }
}";
        [MenuItem("GameTool/GenerateMessage")]
        static void GenerateMessage()
        {
            string classStr;
            string filePath;
            string messageName;
            string responseName;
            string param;
            string context;
            Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/GameCommon.dll");
            Type[] types = asm.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].BaseType == typeof(AMessageRequest))
                {
                    messageName = types[i].Name;
                    filePath = $"{Application.dataPath}/Scripts/Client/Message/{messageName.Replace("MessageRequest", "")}";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = $"{filePath}/{messageName}Sender.cs";
                    responseName = messageName.Replace("Request", "Response");
                    if (!File.Exists(filePath))
                    {
                        classStr = m_messageRequestSenderStr;
                        classStr = classStr.Replace("#Request#", messageName);
                        classStr = classStr.Replace("#Response#", responseName);
                        param = "";
                        context = "";
                        PropertyInfo[] propertys = types[i].GetProperties();
                        for (int i1 = 0; i1 < propertys.Length; i1++)
                        {
                            string propertyName = propertys[i1].Name;
                            if(propertyName == "RpcId")
                            {
                                continue;
                            }
                            if (i1 != 0)
                            {
                                param += ",";
                            }
                            propertyName = char.ToLower(propertyName[0]) + propertyName.Substring(1);
                            context += $"\t\t\tm_request.{propertys[i1].Name} = {propertyName};{Environment.NewLine}";
                            param += $"{propertys[i1].PropertyType.FullName} {propertyName}";
                        }
                        classStr = classStr.Replace("#Params#", param);
                        classStr = classStr.Replace("#Context#", context);

                        FileStream file = new FileStream(filePath, FileMode.CreateNew);
                        StreamWriter fileW = new StreamWriter(file, Encoding.UTF8);
                        fileW.Write(classStr);
                        fileW.Flush();
                        fileW.Close();
                        file.Close();
                    }
                    filePath = $"{Application.dataPath}/Scripts/Server/Message/{messageName.Replace("MessageRequest", "")}";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = $"{filePath}/{messageName}Hander.cs";
                    if (!File.Exists(filePath))
                    {
                        classStr = m_messageRequestHanderTxt;
                        classStr = classStr.Replace("#Request#", messageName);
                        classStr = classStr.Replace("#Response#", responseName);

                        FileStream file = new FileStream(filePath, FileMode.CreateNew);
                        StreamWriter fileW = new StreamWriter(file, Encoding.UTF8);
                        fileW.Write(classStr);
                        fileW.Flush();
                        fileW.Close();
                        file.Close();
                    }
                }
                if (types[i].BaseType == typeof(AMessageNotice))
                {
                    messageName = types[i].Name;
                    filePath = $"{Application.dataPath}/Scripts/Server/Message/{messageName.Replace("MessageNotice", "")}";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = $"{filePath}/{messageName}Sender.cs";
                    if (!File.Exists(filePath))
                    {
                        classStr = m_messageNoticeSenderTxt;
                        classStr = classStr.Replace("#Notice#", messageName);
                        param = "";
                        context = "";
                        PropertyInfo[] propertys = types[i].GetProperties();
                        for (int i1 = 0; i1 < propertys.Length; i1++)
                        {
                            if (i1 != 0)
                            {
                                param += ",";
                            }
                            string propertyName = propertys[i1].Name;
                            propertyName = char.ToLower(propertyName[0]) + propertyName.Substring(1);
                            context += $"\t\t\tm_notice.{propertys[i1].Name} = {propertyName};{Environment.NewLine}";
                            param += $"{propertys[i1].PropertyType.FullName} {propertyName}";
                        }
                        classStr = classStr.Replace("#Params#", param);
                        classStr = classStr.Replace("#Context#", context);

                        FileStream file = new FileStream(filePath, FileMode.CreateNew);
                        StreamWriter fileW = new StreamWriter(file, Encoding.UTF8);
                        fileW.Write(classStr);
                        fileW.Flush();
                        fileW.Close();
                        file.Close();
                    }
                    filePath = $"{Application.dataPath}/Scripts/Client/Message/{messageName.Replace("MessageNotice", "")}";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = $"{filePath}/{messageName}Hander.cs";
                    if (!File.Exists(filePath))
                    {
                        classStr = m_messageNoticeHanderTxt;
                        classStr = classStr.Replace("#Notice#", messageName);

                        FileStream file = new FileStream(filePath, FileMode.CreateNew);
                        StreamWriter fileW = new StreamWriter(file, Encoding.UTF8);
                        fileW.Write(classStr);
                        fileW.Flush();
                        fileW.Close();
                        file.Close();
                    }
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
