using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Ximmerse.Vision;
using Ximmerse.Vision.Internal;
using System.IO;

namespace LenovoMirageARSDK.Editor
{
    public class AddDeviceWindows : EditorWindow
    {

        private string[] m_Platform = { "iOS", "Android" };
        private int Index = 1;
        private Device m_Device;

        [SerializeField]
        public List<string> ModelsName = new List<string>();
        private DeviceSettings m_Setting;
        protected SerializedObject _serializedObject;
        protected SerializedProperty _assetLstProperty;
        private static string path;

        public static void Open(string _path)
        {
            path = _path;
            Rect tempRect = new Rect(0, 0, 300, 400);
            AddDeviceWindows temp = EditorWindow.GetWindowWithRect(typeof(AddDeviceWindows), tempRect) as AddDeviceWindows;
            temp.m_Device = new Device();
            temp.m_Setting = new DeviceSettings();
        }

        void OnInspectorUpdate()
        {
            this.Repaint();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);

            GUILayout.Label("Platform");
            Index = EditorGUILayout.Popup(Index, m_Platform);
            GUILayout.Space(10);

            m_Device.Name = GUILabelAndTextField("Device Name", m_Device.Name, true);

            m_Device.SettingsFile = GUILabelAndTextField("File Name", m_Device.SettingsFile, true);

            if (m_Device.SettingsFile != null && !m_Device.SettingsFile.Contains(".txt"))
            {
                m_Device.SettingsFile += ".txt";
            }
            GUILayout.Space(10);

            if (m_Setting == null)
            {
                m_Setting = new DeviceSettings();
            }

            if (_assetLstProperty == null)
            {
                _serializedObject = new SerializedObject(this);
                _assetLstProperty = _serializedObject.FindProperty("ModelsName");
            }

            GUILayout.Label("------------Must Set RealModelName -------------");
            if (_assetLstProperty != null && _serializedObject != null)
                EditorGUILayout.PropertyField(_assetLstProperty, true);
            GUILayout.Label("------------/Must Set RealModelName -------------");

            GUILayout.Label("UIBottom");
            m_Setting.UIBottomOffset = EditorGUILayout.FloatField(m_Setting.UIBottomOffset);//float.Parse(GUILabelAndTextField("UIBottom", m_Setting.UIBottomOffset.ToString(),true));

            GUILayout.Label("UILeft");
            m_Setting.UILeftOffset = EditorGUILayout.FloatField(m_Setting.UILeftOffset);

            GUILayout.Label("UIRight");
            m_Setting.UIRightOffset = EditorGUILayout.FloatField(m_Setting.UIRightOffset);

            GUILayout.Label("UITop");
            m_Setting.UITopOffset = EditorGUILayout.FloatField(m_Setting.UITopOffset);

            GUILayout.Space(10);

            if (GUILayout.Button("Add"))
            {
                if (_assetLstProperty != null && _serializedObject != null)
                    _serializedObject.ApplyModifiedProperties();

                if (m_Device.Name == null || m_Device.Name.Equals(string.Empty))
                {
                    this.ShowNotification(new GUIContent("Name is Null"));
                    return;
                }

                if (m_Device.SettingsFile == null || m_Device.SettingsFile.Equals(string.Empty))
                {
                    this.ShowNotification(new GUIContent("FileName is Null"));
                    return;
                }

                m_Device.Models = new List<string>();
                m_Device.Models.AddRange(ModelsName);

                File.WriteAllText(path + m_Device.SettingsFile, JsonUtility.ToJson(m_Setting, true));
                AddDeviceForSdk.AddValue(m_Device, m_Platform[Index], path);

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }

            EditorGUILayout.EndVertical();
        }

        private string GUILabelAndTextField(string labelContent, string textFieldContent, bool horizontal = true)
        {
            if (horizontal)
                EditorGUILayout.BeginHorizontal();

            GUILayout.Label(labelContent);

            string retString = EditorGUILayout.TextField(textFieldContent);

            if (horizontal)
                EditorGUILayout.EndHorizontal();

            return retString;
        }

    }
}
