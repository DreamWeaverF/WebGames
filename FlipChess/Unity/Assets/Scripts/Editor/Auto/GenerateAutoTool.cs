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
                    string genPath = $"Assets/ScriptableObjects/{dllName}/{types[i1].Name}.asset";
                    ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(genPath);
                    if(so != null)
                    {
                        continue;
                    }
                    so = ScriptableObject.CreateInstance(types[i1]);
                    AssetDatabase.CreateAsset(so, genPath);
                }
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
    }
}


