using GameCommon;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    [CustomEditor(typeof(GameClient.NetworkController))]
    [CanEditMultipleObjects]
    public class GameClientNetworkControllerEditor : Editor
    {
        private GameClient.NetworkController m_target;
        public override void OnInspectorGUI()
        {
            m_target = (GameClient.NetworkController)target;
            DrawDefaultInspector();
            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Bind Message"))
            {
                FieldInfo field = m_target.GetType().GetField("m_noticeHanders", BindingFlags.NonPublic | BindingFlags.Instance);
                List<Type> keys = new List<Type>();
                List<GameClient.AMessageNoticeHander> values = new List<GameClient.AMessageNoticeHander>();
                Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/GameClient.dll");
                Type[] types = asm.GetTypes();
                for (int i1 = 0; i1 < types.Length; i1++)
                {
                    if (types[i1].BaseType.BaseType != typeof(GameClient.AMessageNoticeHander))
                    {
                        continue;
                    }
                    string soPath = $"Assets/ScriptableObjects/GameClient/{types[i1].Name}.asset";
                    ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
                    if (so == null)
                    {
                        continue;
                    }
                    keys.Add(types[i1]);
                    values.Add(so as GameClient.AMessageNoticeHander);
                }
                SerializationDictionary<Type, GameClient.AMessageNoticeHander> handers = new SerializationDictionary<Type, GameClient.AMessageNoticeHander>(keys, values);
                field.SetValue(m_target, handers);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    [CustomEditor(typeof(GameServer.NetworkController))]
    [CanEditMultipleObjects]
    public class GameServerNetworkControllerEditor : Editor
    {
        private GameServer.NetworkController m_target;
        public override void OnInspectorGUI()
        {
            m_target = (GameServer.NetworkController)target;
            DrawDefaultInspector();
            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Bind Message"))
            {
                FieldInfo field = m_target.GetType().GetField("m_requestHanders", BindingFlags.NonPublic | BindingFlags.Instance);
                List<Type> keys = new List<Type>();
                List<GameServer.AMessageRequestHander> values = new List<GameServer.AMessageRequestHander>();
                Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/GameServer.dll");
                Type[] types = asm.GetTypes();
                for (int i1 = 0; i1 < types.Length; i1++)
                {
                    if (types[i1].BaseType.BaseType != typeof(GameServer.AMessageRequestHander))
                    {
                        continue;
                    }
                    string soPath = $"Assets/ScriptableObjects/GameServer/{types[i1].Name}.asset";
                    ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
                    if (so == null)
                    {
                        continue;
                    }
                    keys.Add(types[i1]);
                    values.Add(so as GameServer.AMessageRequestHander);
                }
                SerializationDictionary<Type, GameServer.AMessageRequestHander> handers = new SerializationDictionary<Type, GameServer.AMessageRequestHander>(keys, values);
                field.SetValue(m_target, handers);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
