using GameCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    public class GenConfigTool
    {
        private static string[] m_dllNames = new string[] { "GameCommon", "GameClient", "GameServer" };
        private static string m_csvPath = Application.dataPath + "/../../Config/";

        [MenuItem("GameTool/GenConfig")]
        static void AutoGenSO()
        {
            string dllName;
            for (int i = 0; i < m_dllNames.Length; i++)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(m_csvPath);
                dllName = m_dllNames[i];
                Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/{dllName}.dll");
                Type[] types = asm.GetTypes();
                for (int i1 = 0; i1 < types.Length; i1++)
                {
                    if (types[i1].GetCustomAttribute<GenConfigClassAttribute>() == null)
                    {
                        continue;
                    }
                    FileInfo[] fileInfos = dirInfo.GetFiles($"{types[i1].Name}.csv");
                    if(fileInfos.Length <= 0)
                    {
                        Debug.LogError($"找不到对应配置{types[i1].Name}");
                        continue;
                    }
                    string genPath = $"Assets/ScriptableObjects/{dllName}/{types[i1].Name}.asset";
                    FileStream fs = new FileStream(fileInfos[0].FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (StreamReader streamReader = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                    {
                        object result = LoadObject(streamReader, asm, $"{types[i1]}");
                        FieldInfo filed = types[i1].GetField("m_datas", BindingFlags.Instance | BindingFlags.NonPublic);
                        ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(genPath);
                        if (obj == null)
                        {
                            obj = ScriptableObject.CreateInstance(types[i1]);
                            AssetDatabase.CreateAsset(obj, genPath);
                        }
                        filed.SetValue(obj, result);
                        EditorUtility.SetDirty(obj);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static object LoadObject(TextReader tReader, Assembly asm, string className)
        {
            string elementName = $"{className}Element";
            Type elementType = asm.GetType(elementName);
            FieldInfo[] fields = elementType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            Type listRef = typeof(List<>);
            Type[] listParam = { elementType };
            object result = Activator.CreateInstance(listRef.MakeGenericType(listParam));
            string line;
            int lineNum = 0;
            string[] elementDataTypes = null;
            while ((line = tReader.ReadLine()) != null)
            {
                if (line.StartsWith("#"))
                    continue;
                ++lineNum;
                if (lineNum == 1)
                {
                    elementDataTypes = EnumerateCsvLine(line).ToArray();
                    continue;
                }
                //描述信息
                else if (lineNum == 2)
                {
                    continue;
                }
                string[] vals = EnumerateCsvLine(line).ToArray();
                if (vals.Length < 2)
                {
                    Debug.LogWarning(string.Format("CsvUtil: ignoring line '{0}': not enough fields", line));
                    continue;
                }
                object valuelineObj = asm.CreateInstance(elementName);
                foreach (FieldInfo field in fields)
                {
                    for (int i = 0; i < elementDataTypes.Length; ++i)
                    {
                        string val = vals[i];
                        string typeName = elementDataTypes[i];
                        if (string.Compare(typeName, field.Name, true) == 0)
                        {
                            SetField(val, field, valuelineObj);
                        }
                    }
                }
                result.GetType().GetMethod("Add").Invoke(result, new[] { valuelineObj });
            }
            return result;
        }
        private static void SetField(string strValue, FieldInfo field, object destObject)
        {
            object typedVal;
            if (typeof(System.Collections.IList).IsAssignableFrom(field.FieldType))
            {
                string[] vlists = strValue.Split("|");
                Type[] typeDefs = field.FieldType.GenericTypeArguments;
                Type listRef = typeof(List<>);
                Type[] listParam = { typeDefs[0] };
                typedVal = Activator.CreateInstance(listRef.MakeGenericType(listParam));
                for (int i = 0; i < vlists.Length; i++)
                {
                    typedVal.GetType().GetMethod("Add").Invoke(typedVal, new[] { vlists[i].Convert(typeDefs[0]) });
                }
            }
            else
            {
                typedVal = strValue.Convert(field.FieldType);
            }
            field.SetValue(destObject, typedVal);
        }
        private static IEnumerable<string> EnumerateCsvLine(string line)
        {
            // Regex taken from http://wiki.unity3d.com/index.php?title=CSVReader
            foreach (Match m in Regex.Matches(line,
                @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                RegexOptions.ExplicitCapture))
            {
                yield return m.Groups[1].Value;
            }
        }
    }
}
