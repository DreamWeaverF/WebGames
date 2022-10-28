using GameClient;
using GameCommon;
using GameServer;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    [CustomEditor(typeof(AUI), true)]
    [CanEditMultipleObjects]
    public class AUIEditor : Editor
    {
        private AUI m_target;
        public override void OnInspectorGUI()
        {
            m_target = (AUI)target;
            DrawDefaultInspector();
            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Bind Message"))
            {
                FieldInfo[] fields = m_target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                FieldInfo field;
                for (int i = 0; i < fields.Length; i++)
                {
                    field = fields[i];
                    if (field.FieldType == typeof(SerializationDictionary<Type, AMessageNoticeHander>))
                    {
                        List<AMessageNoticeHander> values = new List<AMessageNoticeHander>();
                        Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/GameClient.dll");
                        Type[] types = asm.GetTypes();
                        List<Type> keys = new List<Type>();
                        for (int i1 = 0; i1 < types.Length; i1++)
                        {
                            if (types[i1].BaseType.BaseType != typeof(AMessageNoticeHander))
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
                            values.Add(so as AMessageNoticeHander);
                        }
                        //SerializationDictionary<Type, AMessageNoticeHander> handers = new SerializationDictionary<Type, AMessageNoticeHander>(keys, values);
                        //field.SetValue(m_target, handers);
                    }
                    else if (field.FieldType == typeof(SerializationDictionary<Type, AMessageRequestHander>))
                    {
                        List<AMessageRequestHander> values = new List<AMessageRequestHander>();
                        Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/GameServer.dll");
                        Type[] types = asm.GetTypes();
                        List<Type> keys = new List<Type>();
                        for (int i1 = 0; i1 < types.Length; i1++)
                        {
                            if (types[i1].BaseType.BaseType != typeof(AMessageRequestHander))
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
                            values.Add(so as AMessageRequestHander);
                        }
                        //SerializationDictionary<Type, AMessageRequestHander> handers = new SerializationDictionary<Type, AMessageRequestHander>(keys, values);
                        //field.SetValue(m_target, handers);
                    }
                    else
                    {
                        if (field.FieldType.GetCustomAttribute<GenerateAutoClassAttribute>() != null)
                        {
                            string soPath = $"Assets/ScriptableObjects/{field.FieldType.Namespace}/{field.FieldType.Name}.asset";
                            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
                            field.SetValue(m_target, so);
                        }
                        else if(field.GetCustomAttribute<UIComponentFieldAttribute>() != null)
                        {
                            Transform child = m_target.transform.Find(field.Name);
                            if(child != null)
                            {
                                field.SetValue(m_target, child.GetComponent(field.FieldType));
                            }
                        }
                    }
                }
                EditorUtility.SetDirty(m_target);
                AssetDatabase.SaveAssets();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
