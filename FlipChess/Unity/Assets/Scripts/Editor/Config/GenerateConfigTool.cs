using GameCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using ExcelDataReader;
using System.Data;

namespace GameEditor
{
    public class GenerateConfigTool
    {
        private static string[] m_dllNames = new string[] { "GameCommon", "GameClient", "GameServer" };
        private static string m_csvPath = Application.dataPath + "/../../Config/";

        [MenuItem("GameTool/GenerateConfig")]
        static void GenerateConfig()
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
                    if(types[i1].BaseType == null)
                    {
                        continue;
                    }
                    if(types[i1].BaseType.Name != "AConfig`1")
                    {
                        continue;
                    }
                    FileInfo[] fileInfos = dirInfo.GetFiles($"{types[i1].Name}.xlsx");
                    if(fileInfos.Length <= 0)
                    {
                        Debug.LogError($"找不到对应配置{types[i1].Name}");
                        continue;
                    }
                    using (FileStream stream = File.Open(fileInfos[0].FullName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet _dataSet = reader.AsDataSet();
                            object result = LoadObject(_dataSet.Tables[0], asm, $"{types[i1]}");
                            FieldInfo filed = types[i1].GetField("m_datas", BindingFlags.Instance | BindingFlags.NonPublic);
                            string genPath = $"Assets/ScriptableObjects/{dllName}/{types[i1].Name}.asset";
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
                    //string genPath = $"Assets/ScriptableObjects/{dllName}/{types[i1].Name}.asset";
                    //FileStream fs = new FileStream(fileInfos[0].FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    //using (StreamReader streamReader = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                    //{
                    //    object result = LoadObject(streamReader, asm, $"{types[i1]}");
                    //    FieldInfo filed = types[i1].GetField("m_datas", BindingFlags.Instance | BindingFlags.NonPublic);
                    //    ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(genPath);
                    //    if (obj == null)
                    //    {
                    //        obj = ScriptableObject.CreateInstance(types[i1]);
                    //        AssetDatabase.CreateAsset(obj, genPath);
                    //    }
                    //    filed.SetValue(obj, result);
                    //    EditorUtility.SetDirty(obj);
                    //    AssetDatabase.SaveAssets();
                    //}
                }
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static object LoadObject(DataTable dataTable, Assembly asm, string className)
        {
            string elementName = $"{className}Element";
            Type elementType = asm.GetType(elementName);
            FieldInfo[] fields = elementType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            Type listRef = typeof(List<>);
            Type[] listParam = { elementType };
            object result = Activator.CreateInstance(listRef.MakeGenericType(listParam));
            if (dataTable.Rows.Count < 3)
            {
                return result;
            }
            object[] elementDataTypes = dataTable.Rows[0].ItemArray;
            for (int i = 2; i < dataTable.Rows.Count; i++)
            {
                object valuelineObj = asm.CreateInstance(elementName);
                foreach (FieldInfo field in fields)
                {
                    for (int i1 = 0; i1 < elementDataTypes.Length; i1++)
                    {
                        string val = dataTable.Rows[i][i1].ToString();
                        string typeName = elementDataTypes[i1].ToString();
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
    }
}
