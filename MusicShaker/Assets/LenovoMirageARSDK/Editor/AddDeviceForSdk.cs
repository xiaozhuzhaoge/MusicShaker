using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Ximmerse.Vision;
using Ximmerse.Vision.Internal;
using System.IO;

namespace LenovoMirageARSDK.Editor
{
    /// <summary>
    /// DevicesSetting检测添加工具
    /// </summary>
    public class DevicesSettingWindow : EditorWindow
    {
        private Device mDeviceName;
        private string path;
        protected SerializedObject _serializedObject;
        protected SerializedProperty _assetLstProperty;
        public DeviceSettings m_CurrentDeviceSetting;
        private static int index = 0;
        private static bool CloseAll = false;

        public void Open(Device _name, string _path)
        {
            CloseAll = false;
            index++;
            Rect wr = new Rect(0, 0, 300, 300);
            DevicesSettingWindow frameworkConfigEditorWindow = (DevicesSettingWindow)EditorWindow.GetWindowWithRect(typeof(DevicesSettingWindow), wr, false, "Add DeviceStting");
            frameworkConfigEditorWindow.mDeviceName = _name;
            frameworkConfigEditorWindow._serializedObject = new SerializedObject(frameworkConfigEditorWindow);
            frameworkConfigEditorWindow._assetLstProperty = frameworkConfigEditorWindow._serializedObject.FindProperty("m_CurrentDeviceSetting");
            frameworkConfigEditorWindow.path = _path;
            frameworkConfigEditorWindow.Show();
        }

        void OnInspectorUpdate()
        {
            this.Repaint();
        }

        public static string GUILabelAndTextField(string labelContent, string textFieldContent, bool horizontal = true)
        {
            if (horizontal)
                EditorGUILayout.BeginHorizontal();

            GUILayout.Label(labelContent);

            string retString = EditorGUILayout.TextField(textFieldContent);

            if (horizontal)
                EditorGUILayout.EndHorizontal();

            return retString;
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label(mDeviceName.Name);
            mDeviceName.SettingsFile = GUILabelAndTextField("FileName", mDeviceName.SettingsFile);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(_assetLstProperty, true);
            GUILayout.Space(20);
            if (GUILayout.Button("Add"))
            {
                _serializedObject.ApplyModifiedProperties();
                AddDeviceForSdk.ChangeValue(mDeviceName, path);
                File.WriteAllText(path + mDeviceName.SettingsFile, JsonUtility.ToJson(m_CurrentDeviceSetting, true));
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                this.Close();
            }
            if (GUILayout.Button("CloseAll"))
            {
                CloseAll = true;
            }
            if (CloseAll)
                this.Close();
            EditorGUILayout.EndVertical();
        }
    }

    public class AddDeviceForSdk : EditorWindow
    {

        public static string EditorFileText;
        public static DeviceSettings m_Setting;

        protected SerializedObject _serializedObject;
        protected SerializedProperty _assetLstProperty;

        public Devices m_DevicesData = new Devices();
        private static Device m_CurrentDevices;
        public static string m_Devices;

        private static string Path;
        private string m_TempDevicesText;
        Vector2 ScrollPos;
        private static AddDeviceForSdk m_CurrentWindows;
        private bool isOpen = false;
        [MenuItem("Assets/LenovoMirageARSDK/DevicesSetting")]
        static void Open()
        {
            Rect wr = new Rect(0, 0, 700, 1000);


            AddDeviceForSdk frameworkConfigEditorWindow = (AddDeviceForSdk)EditorWindow.GetWindowWithRect(typeof(AddDeviceForSdk), wr, true, "Add Device");
            m_Setting = new DeviceSettings();
            m_CurrentWindows = frameworkConfigEditorWindow;
            frameworkConfigEditorWindow.Show();
            frameworkConfigEditorWindow.isOpen = false;
        }

        public static void ChangeValue(Device theDvice, string path)
        {
            foreach (var item in m_CurrentWindows.m_DevicesData.Platforms)
            {
                Device temp = item.Devices.Find(x => x.Name.Equals(theDvice.Name));
                if (temp != null)
                {
                    Debug.Log("改变名字" + theDvice.SettingsFile);
                    temp.SettingsFile = theDvice.SettingsFile;
                }
            }

            File.WriteAllText(path + "devices.json", JsonUtility.ToJson(m_CurrentWindows.m_DevicesData, true));
            AssetDatabase.Refresh();
            m_CurrentWindows.Repaint();
            m_CurrentWindows._serializedObject = null;
        }

        public static void AddValue(Device theDvice, string Platform, string path)
        {
            Debug.Log("当前模型列表" + theDvice.Models.Count);
            List<Device> item = m_CurrentWindows.m_DevicesData.Platforms.Find(x => x.Platform.Equals(Platform)).Devices;

            Device theDevice = item.Find(x => x.Name.Equals(theDvice.Name));

            if (theDevice == null)
            {
                item.Add(theDvice);
            }
            else
            {
                m_CurrentWindows.ShowNotification(new GUIContent("already existed Device"));
                return;
            }

            File.WriteAllText(path + "devices.json", JsonUtility.ToJson(m_CurrentWindows.m_DevicesData, true));
            m_CurrentWindows._serializedObject = null;
            m_CurrentWindows.Repaint();
        }

        void OnInspectorUpdate()
        {
            this.Repaint();
        }

        //void OnHierarchyChange()
        //{
        //    this.Close();
        //    this.Show();
        //}

        //void OnProjectChange()
        //{
        //    this.Close();
        //    this.Show();
        //}

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            GUI.skin.label.fontSize = 12;

            GUILayout.Space(10);
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.EndHorizontal();
            //Debug.Log(Application.dataPath);

            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, true, false, GUILayout.Width(800), GUILayout.Height(900));

            if (Path == null || Path.Equals(string.Empty))
            {
                UnityEngine.Object tempObj = Selection.activeObject;
                Path = AssetDatabase.GetAssetPath(tempObj);
                if (!Path.Contains("devices"))
                {
                    return;
                }
                StreamReader m_reader = new StreamReader(Path);
                m_Devices = m_reader.ReadToEnd();

                m_CurrentWindows = this;
                EditorFileText = "pvt_";
                m_CurrentDevices = new Device();
                isOpen = false;
            }
            if (m_Devices != null && !m_Devices.Equals(string.Empty))
            {
                if (_serializedObject == null)
                {
                    StreamReader m_reader = new StreamReader(Path);
                    m_Devices = m_reader.ReadToEnd();
                    m_DevicesData = JsonUtility.FromJson<Devices>(m_Devices);

                    List<Device> m_DeviceList = m_DevicesData.Platforms.Find(item => item.Platform.Equals("Android")).Devices;
                    string temp = Path;
                    temp = temp.Replace("Assets/", "/");
                    string latPath = Application.dataPath + temp;
                    latPath = latPath.Replace("devices.json", "");

                    DirectoryInfo dir = new DirectoryInfo(latPath);

                    FileInfo[] allInfo = dir.GetFiles();
                    List<string> m_HasFileList = new List<string>();

                    for (int i = 0; i < allInfo.Length; i++)
                    {
                        m_HasFileList.Add(allInfo[i].Name);
                    }
                    int index = 0;
                    if (!isOpen)
                    {
                        isOpen = true;
                        for (int i = 0; i < m_DeviceList.Count; i++)
                        {
                            if (!m_HasFileList.Contains(m_DeviceList[i].SettingsFile))
                            {
                                DevicesSettingWindow tempWindows = new DevicesSettingWindow();
                                tempWindows.Open(m_DeviceList[i], latPath);
                                tempWindows.position = new Rect(++index * position.x, position.yMax / 2, 300, 300);
                            }
                        }
                    }

                    _serializedObject = new SerializedObject(this);
                    _assetLstProperty = _serializedObject.FindProperty("m_DevicesData");
                }

                if (_assetLstProperty != null && _serializedObject != null)
                {
                    EditorGUILayout.PropertyField(_assetLstProperty, true);
                }

                //Debug.Log(Path);
            }
            EditorGUILayout.EndScrollView();

            #region 保存

            if (GUILayout.Button("Apply"))
            {
                string temp = Path;
                temp = temp.Replace("Assets/", "/");
                _serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(this);
                int[] list = new int[m_DevicesData.Platforms.Count];

                for (int i = 0; i < m_DevicesData.Platforms.Count; i++)
                {
                    list[i] = m_DevicesData.Platforms[i].Devices.Count;
                }
                Devices tempDevices = JsonUtility.FromJson<Devices>(m_Devices);
                int[] list2 = new int[tempDevices.Platforms.Count];
                for (int i = 0; i < tempDevices.Platforms.Count; i++)
                {
                    list2[i] = tempDevices.Platforms[i].Devices.Count;
                }

                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] <= list2[i])
                        continue;

                    int NewInt = list[i] - list2[i];
                    for (int j = list2[i]; j < list[i]; j++)
                    {
                        string latPath = Application.dataPath + temp;
                        latPath = latPath.Replace("devices.json", "");
                        DevicesSettingWindow tempWindows = new DevicesSettingWindow();
                        tempWindows.Open(m_DevicesData.Platforms[i].Devices[j], latPath);
                        tempWindows.position = new Rect(j - list2[i] + 0.3f * position.x, position.yMax / 2, 300, 300);
                    }
                }

                File.WriteAllText(Application.dataPath + temp, JsonUtility.ToJson(m_DevicesData, true));
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();


            }
            #endregion

            #region 添加
            if (GUILayout.Button("Add"))
            {
                string temp = Path;
                temp = temp.Replace("Assets/", "/");
                string latPath = Application.dataPath + temp;
                latPath = latPath.Replace("devices.json", "");
                AddDeviceWindows.Open(latPath);
            }
            #endregion

            #region 重置
            if (GUILayout.Button("Reset"))
            {
                _serializedObject = null;
                isOpen = false;
            }
            #endregion

            EditorGUILayout.EndVertical();
        }
    }

}
