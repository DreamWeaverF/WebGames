using GameCommon;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    public class GenerateAutoTool
    {
        private static string[] m_dllNames = new string[] { "GameCommon","GameClient","GameServer" };
        [MenuItem("GameTool/GenerateAuto")]
        static void GenerateAuto()
        {
            string dllName;
            //generata
            for(int i = 0; i < m_dllNames.Length; i++)
            {
                dllName = m_dllNames[i];
                Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/{dllName}.dll");
                Type[] types = asm.GetTypes();
                for(int i1 = 0; i1 < types.Length; i1++)
                {
                    if(types[i1].GetCustomAttribute<GenerateAutoClassAttribute>() == null)
                    {
                        continue;
                    }
                    string soPath = $"Assets/ScriptableObjects/{dllName}/{types[i1].Name}.asset";
                    ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
                    if(so != null)
                    {
                        continue;
                    }
                    so = ScriptableObject.CreateInstance(types[i1]);
                    AssetDatabase.CreateAsset(so, soPath);
                    Debug.Log($"生成文件 {types[i1].Name}");
                }
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            //refresh
            for (int i = 0; i < m_dllNames.Length; i++)
            {
                dllName = m_dllNames[i];
                Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/{dllName}.dll");
                Type[] types = asm.GetTypes();
                for (int i1 = 0; i1 < types.Length; i1++)
                {
                    if (types[i1].GetCustomAttribute<GenerateAutoClassAttribute>() == null)
                    {
                        continue;
                    }
                    string soPath = $"Assets/ScriptableObjects/{dllName}/{types[i1].Name}.asset";
                    ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
                    if (so == null)
                    {
                        continue;
                    }
                    FieldInfo[] fields = types[i1].GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    for(int i2 = 0; i2 < fields.Length; i2++)
                    {
                        if(fields[i2].FieldType.GetCustomAttribute<GenerateAutoClassAttribute>() == null)
                        {
                            continue;
                        }
                        soPath = $"Assets/ScriptableObjects/{fields[i2].FieldType.Namespace}/{fields[i2].FieldType.Name}.asset";
                        ScriptableObject relySO = AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
                        if (relySO == null)
                        {
                            continue;
                        }
                        fields[i2].SetValue(so, relySO);
                    }
                    Debug.Log($"绑定文件 {types[i1].Name}");
                    EditorUtility.SetDirty(so);
                }
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }
}


