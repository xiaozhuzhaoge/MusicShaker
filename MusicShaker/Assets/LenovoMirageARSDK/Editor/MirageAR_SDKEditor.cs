using UnityEditor;
using UnityEngine;
using System.Collections;

namespace LenovoMirageARSDK.Editor
{
    
    [CustomEditor(typeof(MirageAR_SDK))]
    public class MirageAR_SDKEditor : UnityEditor.Editor
    {
        /// <summary>
        /// The Serialized Object
        /// </summary>
        private SerializedObject seriObject;

        //roperty Value
        private SerializedProperty mHasMirageARLogger;
        private SerializedProperty mMirageARLogger;
        private SerializedProperty mIs2DMode;

        private void OnEnable()
        {            
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("mHasMirageARLogger"), new GUIContent("Has MirageAR Logger"), true);
            if (serializedObject.FindProperty("mHasMirageARLogger").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("mMirageARLogger"), new GUIContent("MirageAR Logger"), true);
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("mIs2DMode"), new GUIContent("Is 2D Mode"), true);

            EditorGUILayout.EndVertical();

            if (GUI.changed) serializedObject.ApplyModifiedProperties();
        }
    }
    
}
