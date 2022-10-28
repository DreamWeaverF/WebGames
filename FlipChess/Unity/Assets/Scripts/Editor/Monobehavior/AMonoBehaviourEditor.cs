using GameClient;
using GameServer;
using GameCommon;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;

namespace GameEditor
{
    [CustomEditor(typeof(AMonoBehaviour),true)]
    [CanEditMultipleObjects]
    public class AMonoBehaviourEditor : Editor
    {
        private AMonoBehaviour m_target;
        public override void OnInspectorGUI()
        {
            m_target = (AMonoBehaviour)target;
            DrawDefaultInspector();
            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Bind Message"))
            {
                FieldInfo[] fields = m_target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                FieldInfo field;
                for(int i = 0; i < fields.Length; i++)
                {
                    field = fields[i];
                    if (field.GetCustomAttribute<SerializeField>() == null)
                    {
                        continue;
                    }
                    if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(SerializationDictionary<,>))
                    {
                        List<Type> keys = new List<Type>();
                        Type listRef = typeof(List<>);
                        Type valueType = field.FieldType.GenericTypeArguments[1];
                        Type[] listParam = { valueType };
                        object values = Activator.CreateInstance(listRef.MakeGenericType(listParam));
                        Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/{valueType.Namespace}.dll");
                        Type[] types = asm.GetTypes();
                        for (int i1 = 0; i1 < types.Length; i1++)
                        {
                            if (types[i1].BaseType == null)
                            {
                                continue;
                            }
                            if (types[i1].BaseType.BaseType == null)
                            {
                                continue;
                            }
                            if (types[i1].BaseType.BaseType != valueType)
                            {
                                continue;
                            }
                            string soPath = $"Assets/ScriptableObjects/{valueType.Namespace}/{types[i1].Name}.asset";
                            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
                            if (so == null)
                            {
                                continue;
                            }
                            keys.Add(types[i1]);
                            values.GetType().GetMethod("Add").Invoke(values, new[] { so });
                        }
                        object dictVal = Activator.CreateInstance(field.FieldType, new object[] { keys,values });
                        field.SetValue(m_target, dictVal);
                    }
                    else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        Type listRef = typeof(List<>);
                        Type valueType = field.FieldType.GenericTypeArguments[0];
                        Type[] listParam = { valueType };
                        object values = Activator.CreateInstance(listRef.MakeGenericType(listParam));
                        Assembly asm = Assembly.LoadFile($"{Application.dataPath} + /../Library/ScriptAssemblies/{valueType.Namespace}.dll");
                        Type[] types = asm.GetTypes();
                        for (int i1 = 0; i1 < types.Length; i1++)
                        {
                            if (types[i1].BaseType == null)
                            {
                                continue;
                            }
                            if (types[i1].BaseType.BaseType == null)
                            {
                                continue;
                            }
                            if (types[i1].BaseType.BaseType != valueType)
                            {
                                continue;
                            }
                            string soPath = $"Assets/ScriptableObjects/{valueType.Namespace}/{types[i1].Name}.asset";
                            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
                            if (so == null)
                            {
                                continue;
                            }
                            values.GetType().GetMethod("Add").Invoke(values, new[] { so });
                        }
                        field.SetValue(m_target, values);
                    }
                    else
                    {
                        if (field.FieldType.GetCustomAttribute<GenerateAutoClassAttribute>() != null)
                        {
                            string soPath = $"Assets/ScriptableObjects/{field.FieldType.Namespace}/{field.FieldType.Name}.asset";
                            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(soPath);
                            field.SetValue(m_target, so);
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
