using GameCommon;
using GameLaunch;
using UnityEditor;

namespace GameEditor
{
    [CustomEditor(typeof(LaunchController), true)]
    [CanEditMultipleObjects]
    public class LaunchControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedProperty settings = serializedObject.FindProperty("m_launchSettings");
            EditorGUILayout.PropertyField(settings, false);
            if(settings.objectReferenceValue != null)
            {
                CreateEditor((LaunchSettings)settings.objectReferenceValue).OnInspectorGUI();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
