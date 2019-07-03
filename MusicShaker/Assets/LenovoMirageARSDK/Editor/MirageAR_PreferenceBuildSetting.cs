using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using UnityEditorInternal.VR;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using System.Collections.Generic;

namespace LenovoMirageARSDK.Editor
{
    /// <summary>
    /// Help User to Quick Setting
    /// </summary>
    [InitializeOnLoad]
    public class MirageAR_PreferenceBuildSetting : EditorWindow
    {
        private const bool forceShow = false; // Set to true to get the dialog to show back up in the case you clicked Ignore All.

        private const string useRecommended = "Use recommended ({0})";
        private const string currentValue = " (current = {0})";
        private const string buildTarget = "Build Target";
        private const string defaultOrientation = "Default orientation";
        private const string showSplashScreen = "Show Splash Screen";
        private const string AndroidMinSDK = "Android Min SDK version";
        private const string AndroidTargetSDK = "Android Target SDK version";
        private const string XRSettings = "XR Settings";
        private const string ScriptingDefineSymbols = "Script Define Symbols";
        private const string StartScene = "Start Scene";
        private const BuildTarget recommended_BuildTarget_Android = BuildTarget.Android;
        private const BuildTarget recommended_BuildTarget_iOS = BuildTarget.iOS;
        private const UIOrientation recommended_defaultOrientation = UIOrientation.LandscapeRight;
        private const bool recommended_showSplashScreen = false;
        private const AndroidSdkVersions recommended_AndroidMinSDK = AndroidSdkVersions.AndroidApiLevel23;
        private const AndroidSdkVersions recommended_AndroidTargetSDK = AndroidSdkVersions.AndroidApiLevelAuto;
        private const string recommended_VirtualRealitySDKs_First = "None";
        private const string recommended_VirtualRealitySDKs_Second = "cardboard";
        private const string recommended_VRTKMirageScriptingDefineSymbols = "VRTK_DEFINE_SDK_MIRAGE";
        private const string recommended_StartScene = "Assets/LenovoMirageARSDK/Scenes/Scene_ARModeGuide.unity";

        protected GUIStyle m_TitleStyle;
        protected GUIStyle m_HeaderStyle;
        protected Vector2 m_ScrollPosition;
        protected static string[] s_BuildTargetNames = null;
        protected int buildTargetPopupIndex = 0;

        protected static string[] VRDevice =  { "None", "cardboard"};

        private static MirageAR_PreferenceBuildSetting window;      

        static MirageAR_PreferenceBuildSetting()
        {
            EditorApplication.update += Update;           
        }

        [UnityEditor.MenuItem("LenovoMirageAR/Settings/PreferenceBuildSetting")]
        public static void Enter()
        {
            //Debug.Log("Enter");
            window = GetWindow<MirageAR_PreferenceBuildSetting>(true, "Preference Build Setting",true);
            window.minSize = new Vector2(720, 480);
        }        

        #region Interface Implement
        #endregion Interface Implement

        #region EditorWindow

        /// <summary>
        /// OnEnable Init Value
        /// </summary>
        protected virtual void OnEnable()
        {          
            //Init Custom Title GUIStyle
            if (m_TitleStyle == null)
            {
                m_TitleStyle = new GUIStyle();
                m_TitleStyle.fontStyle = FontStyle.Bold;
                m_TitleStyle.richText = true;
                m_TitleStyle.alignment = TextAnchor.MiddleCenter;
                m_TitleStyle.fontSize = 28;
            }

            //Init Custom Header GUIStyle
            if (m_HeaderStyle == null)
            {
                m_HeaderStyle = new GUIStyle();
                m_HeaderStyle.fontStyle = FontStyle.Bold;
                m_HeaderStyle.fontSize = 14;
            }

            if (EditorUserBuildSettings.activeBuildTarget == recommended_BuildTarget_iOS)
            {
                buildTargetPopupIndex = 1;
            }            
        }

        static void Update()
        {
            CheckStartScene();

            if (!CheckBuildSettings())
            {
                Enter();
            }
            
            EditorApplication.update -= Update;
        }

        protected virtual void OnGUI()
        {
            GUILayout.BeginVertical();

            //Draw the Title
            GUILayout.Space(10);
            GUILayout.Label("<color=red>Lenovo MirageAR Settings</color>", m_TitleStyle);
            //Image Title
            //var resourcePath = GetResourcePath();
            //var logo = AssetDatabase.LoadAssetAtPath<Texture2D>(resourcePath + "lenovo_logo_red.png");
            //var rect = GUILayoutUtility.GetRect(position.width, 150, GUI.skin.box);
            //if (logo)
            //    GUI.DrawTexture(rect, logo, ScaleMode.ScaleToFit);            

            //Draw help text
            EditorGUILayout.HelpBox("Recommended project settings for LenovoMirageAR!", MessageType.Warning);
#if !UNITY_2017_2
            EditorGUILayout.HelpBox("Recommended Use Unity 2017.2.0f3!",MessageType.Error);
#endif

            DrawSeparator();

            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);

            //1,Check Build Target
            #region Build Target Setting

            EditorGUILayout.LabelField("Build Target Setting", m_HeaderStyle);

            GUILayout.Space(10);

            BeginGroup();

            EditorGUILayout.BeginHorizontal();

            if (s_BuildTargetNames == null)
            {
                BuildTargetTypes();
            }

            buildTargetPopupIndex = EditorGUILayout.Popup("Build Target", buildTargetPopupIndex, s_BuildTargetNames);

            switch (buildTargetPopupIndex)
            {
                case 0:
                    if (EditorUserBuildSettings.activeBuildTarget!=recommended_BuildTarget_Android)
                    {
                        if (GUILayout.Button("Switch", GUILayout.Width(80)))
                        {
                            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Android,recommended_BuildTarget_Android);
                        }
                    }
                    break;
                case 1:
                    if (EditorUserBuildSettings.activeBuildTarget != recommended_BuildTarget_iOS)
                    {
                        if (GUILayout.Button("Switch", GUILayout.Width(80)))
                        {
                            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.iOS, recommended_BuildTarget_iOS);
                        }
                    }
                    break;
                default:
                    break;
            }

            GUILayout.Space(10);

            GUILayout.EndHorizontal();

            EndGroup();

            if (EditorUserBuildSettings.activeBuildTarget != recommended_BuildTarget_Android&& EditorUserBuildSettings.activeBuildTarget != recommended_BuildTarget_iOS)
            {
                //Draw help text
                EditorGUILayout.HelpBox("LenovoMirageAR Only Support Anndroid&iOS,Please Switch To Target Platform First!", MessageType.Error);
                                
                EditorGUILayout.EndScrollView();
                GUILayout.EndVertical();
                return;               
            }

            #endregion Build Target Setting

            int numItems = 0;

            DrawSeparator();

            EditorGUILayout.LabelField("Build Settings", m_HeaderStyle);

            GUILayout.Space(10);

            BeginGroup();

            //2,Check Build Setting for Android
            if (EditorUserBuildSettings.activeBuildTarget == recommended_BuildTarget_Android)
            {     
                //Orientation Right
                if (!CheckOrientation())
                {
                    ++numItems;

                    GUILayout.Label(defaultOrientation + string.Format(currentValue, PlayerSettings.defaultInterfaceOrientation));

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended, recommended_defaultOrientation)))
                    {
                        SetOrientation();
                    }                    

                    GUILayout.EndHorizontal();
                }

                //Show SplashScreen
                if (!CheckShowSplashScreen())
                {
                    ++numItems;

                    GUILayout.Label(showSplashScreen + string.Format(currentValue, PlayerSettings.SplashScreen.show));

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended, recommended_showSplashScreen)))
                    {
                        SetShowSplashScreen();
                    }

                    GUILayout.EndHorizontal();
                }

                //Android min SDK
                if (!CheckAndroidMinSDK())
                {
                    ++numItems;

                    GUILayout.Label(AndroidMinSDK + string.Format(currentValue, PlayerSettings.Android.minSdkVersion));

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended, recommended_AndroidMinSDK)))
                    {
                        SetAndroidMinSDK();
                    }                    

                    GUILayout.EndHorizontal();
                }

                //Andrid Target SDK
                if (!CheckAndroidTargetSDK())
                {
                    ++numItems;

                    GUILayout.Label(AndroidTargetSDK + string.Format(currentValue, PlayerSettings.Android.targetSdkVersion));

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended, recommended_AndroidTargetSDK)))
                    {
                        SetAndroidTargetSDK();
                    }                    

                    GUILayout.EndHorizontal();
                }

                //XR Setting
                if (!CheckXRSettings(BuildTargetGroup.Android))
                {
                    ++numItems;

                    string value = string.Empty;
                    if (VREditor.GetVREnabledOnTargetGroup(BuildTargetGroup.Android))
                    {
                        value = "VR Enalbe;Devices=";
                        string[] currentSupportDevices = VREditor.GetVREnabledDevicesOnTargetGroup(BuildTargetGroup.Android);
                        if (currentSupportDevices.Length == 0)
                        {
                            value += "Empty";
                        }
                        else
                        {
                            value += string.Join(",", currentSupportDevices);
                        }
                    }
                    else
                    {
                        value = "VR Disable";
                    }

                    GUILayout.Label(XRSettings + string.Format(currentValue, value));

                    GUILayout.BeginHorizontal();

                    string recommend = recommended_VirtualRealitySDKs_First + "," + recommended_VirtualRealitySDKs_Second;
                    if (GUILayout.Button(string.Format(useRecommended, recommend)))
                    {
                        SetXRSettings(BuildTargetGroup.Android);                       
                    }

                    GUILayout.EndHorizontal();
                }

                //Scrip DefineSymbols
                if (!CheckScriptingDefineSymbols(BuildTargetGroup.Android))
                {
                    ++numItems;

                    GUILayout.Label(string.Format("{0} miss {1}",ScriptingDefineSymbols,recommended_VRTKMirageScriptingDefineSymbols));

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended,recommended_VRTKMirageScriptingDefineSymbols)))
                    {
                        SetScriptingDefineSymbols(BuildTargetGroup.Android);
                    }

                    GUILayout.EndHorizontal();
                }

                //Start Scene
                if (!CheckStartScene())
                {
                    ++numItems;

                    string labelText="";
                    if (EditorBuildSettings.scenes.Length==0)
                    {
                        labelText = string.Format(currentValue, "null");
                    }
                    else if(EditorBuildSettings.scenes[0].path!=recommended_StartScene)
                    {
                        labelText = string.Format(currentValue, EditorBuildSettings.scenes[0].path);
                    }
                    else if (!EditorBuildSettings.scenes[0].enabled)
                    {
                        labelText = string.Format(currentValue, EditorBuildSettings.scenes[0].path+" disable");
                    }

                    GUILayout.Label(StartScene + labelText);

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended, recommended_StartScene)))
                    {                        
                        SetStartScene();
                    }

                    GUILayout.EndHorizontal();
                }


            }
            else if (EditorUserBuildSettings.activeBuildTarget == recommended_BuildTarget_iOS) //2,Check Build Setting for iOS
            {
                //Orientation Right
                if (PlayerSettings.defaultInterfaceOrientation != recommended_defaultOrientation)
                {
                    ++numItems;

                    GUILayout.Label(defaultOrientation + string.Format(currentValue, PlayerSettings.defaultInterfaceOrientation));

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended, recommended_defaultOrientation)))
                    {
                        PlayerSettings.defaultInterfaceOrientation = recommended_defaultOrientation;
                    }

                    GUILayout.EndHorizontal();
                }

                //Show SplashScreen
                if (!CheckShowSplashScreen())
                {
                    ++numItems;

                    GUILayout.Label(showSplashScreen + string.Format(currentValue, PlayerSettings.SplashScreen.show));

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended, recommended_showSplashScreen)))
                    {
                        SetShowSplashScreen();
                    }

                    GUILayout.EndHorizontal();
                }

                //XR Setting
                if (!(CheckXRSettings(BuildTargetGroup.iOS)))
                {
                    ++numItems;

                    string value = string.Empty;
                    if (VREditor.GetVREnabledOnTargetGroup(BuildTargetGroup.iOS))
                    {
                        value = "VR Enalbe;Devices=";
                        string[] currentSupportDevices = VREditor.GetVREnabledDevicesOnTargetGroup(BuildTargetGroup.iOS);
                        if (currentSupportDevices.Length == 0)
                        {
                            value += "Empty";
                        }
                        else
                        {
                            value += string.Join(",", currentSupportDevices);
                        }
                    }
                    else
                    {
                        value = "VR Disable";
                    }

                    GUILayout.Label(XRSettings + string.Format(currentValue, value));

                    GUILayout.BeginHorizontal();

                    string recommend = recommended_VirtualRealitySDKs_First + "," + recommended_VirtualRealitySDKs_Second;
                    if (GUILayout.Button(string.Format(useRecommended, recommend)))
                    {
                        VREditor.SetVREnabledOnTargetGroup(BuildTargetGroup.iOS, true);

                        VREditor.SetVREnabledDevicesOnTargetGroup(BuildTargetGroup.iOS, VRDevice);
                    }

                    GUILayout.EndHorizontal();
                }

                //Scrip DefineSymbols
                if (!CheckScriptingDefineSymbols(BuildTargetGroup.iOS))
                {
                    ++numItems;                    

                    GUILayout.Label(string.Format("{0} miss {1}", ScriptingDefineSymbols, recommended_VRTKMirageScriptingDefineSymbols));

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended, recommended_VRTKMirageScriptingDefineSymbols)))
                    {
                        SetScriptingDefineSymbols(BuildTargetGroup.iOS);
                    }

                    GUILayout.EndHorizontal();                    
                }

                //Start Scene
                if (!CheckStartScene())
                {
                    ++numItems;

                    string labelText = "";
                    if (EditorBuildSettings.scenes.Length == 0)
                    {
                        labelText = string.Format(currentValue, "null");
                    }
                    else if (EditorBuildSettings.scenes[0].path != recommended_StartScene)
                    {
                        labelText = string.Format(currentValue, EditorBuildSettings.scenes[0].path);
                    }
                    else if (!EditorBuildSettings.scenes[0].enabled)
                    {
                        labelText = string.Format(currentValue, EditorBuildSettings.scenes[0].path + " disable");
                    }

                    GUILayout.Label(StartScene + labelText);

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(string.Format(useRecommended, recommended_StartScene)))
                    {
                        SetStartScene();
                    }

                    GUILayout.EndHorizontal();
                }
            }            

            EndGroup();

            if (numItems>0)
            {

                EditorGUILayout.EndScrollView();

                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Accept All"))
                {
                    SetBuildSettings();
                }

                GUILayout.EndHorizontal();
            }
            else 
            {
                GUILayout.Label("<color=green>Set Correctly!</color>", m_TitleStyle);

                EditorGUILayout.EndScrollView();

                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Close"))
                {
                    Close();
                }

                GUILayout.EndHorizontal();
            }        
        }

        #endregion EditorWindow

        #region Editor GUI Extension

        //protected static System.Collections.Generic.List<int>
        // s_IndentLevels=new System.Collections.Generic.List<int>();

        protected static void BeginGroup(float value = 14.0f)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(value);
            EditorGUILayout.BeginVertical();
            //int indent=EditorGUI.indentLevel+(int)value;
            //EditorGUI.indentLevel=indent;
            //s_IndentLevels.Add(indent);
        }

        protected static void EndGroup()
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            //int i=s_IndentLevels.Count-1;
            //if(i>=0) {
            //	EditorGUI.indentLevel=s_IndentLevels[i];
            //	s_IndentLevels.RemoveAt(i);
            //}
        }

        /// <summary>
        /// Draw a visible separator in addition to adding some padding.
        /// </summary>
        protected static void DrawSeparator()
        {
            GUILayout.Space(12f);

            if (Event.current.type == EventType.Repaint)
            {
                Texture2D tex = EditorGUIUtility.whiteTexture;
                Rect rect = GUILayoutUtility.GetLastRect();
                GUI.color = new Color(0f, 0f, 0f, 0.25f);
                GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
                GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
                GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
                GUI.color = Color.white;
            }
        }

        protected static bool DrawToggle(string label, string key, bool value)
        {
            value = PlayerPrefsEx.GetBool(key, value);
            bool newValue = EditorGUILayout.Toggle(label, value);
            if (newValue != value)
            {
                PlayerPrefsEx.SetBool(key, newValue);
            }
            return newValue;
        }

        protected static string DrawPopup(string label, string key, string value, string[] displayedOptions)
        {
            value = PlayerPrefsEx.GetString(key, value);
            //
            int intValue = System.Array.IndexOf(displayedOptions, value);
            if (intValue < 0) { intValue = 0; }
            //
            int newValue = EditorGUILayout.Popup(label, intValue, displayedOptions);
            if (newValue != intValue)
            {
                PlayerPrefsEx.SetString(key, displayedOptions[newValue]);
            }
            return displayedOptions[newValue];
        }

        protected static int DrawIntPopup(string label, string key, int value, string[] displayedOptions, int[] optionValues)
        {
            value = PlayerPrefsEx.GetInt(key, value);
            int newValue = EditorGUILayout.IntPopup(label, value, displayedOptions, optionValues);
            if (newValue != value)
            {
                PlayerPrefsEx.SetInt(key, newValue);
            }
            return newValue;
        }

        protected static int DrawEnumPopup<T>(string label, string key, int value)
        {
            string[] displayedOptions = System.Enum.GetNames(typeof(T));
            int[] optionValues = (int[])System.Enum.GetValues(typeof(T));
            return DrawIntPopup(label, key, value, displayedOptions, optionValues);
        }

        protected static Vector3 DrawVector3Field(string label, string key, Vector3 value)
        {
            value = PlayerPrefsEx.GetVector3(key, value);
            Vector3 newValue = EditorGUILayout.Vector3Field(label, value);
            if (newValue != value)
            {
                PlayerPrefsEx.SetVector3(key, newValue);
            }
            return newValue;
        }

        protected static Color DrawColorField(string label, string key, Color value)
        {
            value = PlayerPrefsEx.GetColor(key, value);
            Color newValue = EditorGUILayout.ColorField(label, value);
            if (newValue != value)
            {
                PlayerPrefsEx.SetColor(key, newValue);
            }
            return newValue;
        }

        protected static float DrawSlider(string label, string key, float value, float min, float max)
        {
            value = PlayerPrefsEx.GetFloat(key, value);
            float newValue = EditorGUILayout.Slider(label, value, min, max);
            if (newValue != value)
            {
                PlayerPrefsEx.SetFloat(key, newValue);
            }
            return newValue;
        }

        protected static T DrawObjectField<T>(string label, string key, T value) where T : UnityEngine.Object
        {
            value = PlayerPrefsEx.GetObject(key, value) as T;
            T newValue = EditorGUILayout.ObjectField(label, value, typeof(T), false) as T;
            if (newValue != value)
            {
                PlayerPrefsEx.SetObject(key, newValue);
            }
            return newValue;
        }

        #endregion Editor GUI Extension

        #region Check Fuc

        /// <summary>
        /// Check Build Settings is Correct
        /// </summary>
        /// <returns>return true if Build Setttings is Correct</returns>
        public static bool CheckBuildSettings()
        {
            bool returnValue = false;

            if (EditorUserBuildSettings.activeBuildTarget == recommended_BuildTarget_Android)
            {
                returnValue = CheckOrientation() &&
                    CheckShowSplashScreen()&&
                    CheckAndroidMinSDK() &&
                    CheckAndroidTargetSDK() &&
                    CheckXRSettings(BuildTargetGroup.Android) &&
                    CheckScriptingDefineSymbols(BuildTargetGroup.Android);
            }
            else if (EditorUserBuildSettings.activeBuildTarget == recommended_BuildTarget_iOS)
            {
                returnValue = CheckOrientation() &&
                    CheckShowSplashScreen() &&
                    CheckXRSettings(BuildTargetGroup.iOS) &&
                    CheckScriptingDefineSymbols(BuildTargetGroup.iOS);
            }

            return returnValue;
        }

        /// <summary>
        /// Check the Orientation Settings
        /// </summary>
        /// <returns></returns>
        private static bool CheckOrientation()
        {
            return PlayerSettings.defaultInterfaceOrientation == recommended_defaultOrientation;
        }

        /// <summary>
        /// Check Is Show Splash Screen
        /// </summary>
        /// <returns></returns>
        private static bool CheckShowSplashScreen()
        {
            return PlayerSettings.SplashScreen.show==recommended_showSplashScreen;
        }

        /// <summary>
        /// Check the Android Min SDK
        /// </summary>
        /// <returns></returns>
        private static bool CheckAndroidMinSDK()
        {
            return PlayerSettings.Android.minSdkVersion == recommended_AndroidMinSDK;
        }

        /// <summary>
        /// Check Android Target SDK
        /// </summary>
        /// <returns></returns>
        private static bool CheckAndroidTargetSDK()
        {
            return PlayerSettings.Android.targetSdkVersion == recommended_AndroidTargetSDK;
        }

        /// <summary>
        /// Check the XRSettings Whether Turn On,and List SDK As "None" First,"cardboard" Second
        /// </summary>
        /// <returns>return true if XRSetting right</returns>
        private static bool CheckXRSettings(BuildTargetGroup targetGroup)
        {
            bool value = false;

            if (!VREditor.GetVREnabledOnTargetGroup(targetGroup)) return value;

            string[] vrDevices = VREditor.GetVREnabledDevicesOnTargetGroup(targetGroup);

            if (vrDevices.Length == 2)
            {
                //Debug.Log(vrDevices[0]+":"+ vrDevices[1]);
                if (vrDevices[0] == "None" && vrDevices[1] == "cardboard")
                {
                    value = true;
                }
            }
            return value;
        }

        /// <summary>
        /// Check ScriptingDefineSymbols Whether Contains "VRTK_DEFINE_SDK_MIRAGE"
        /// </summary>
        /// <returns>return ture if has Contain</returns>
        private static bool CheckScriptingDefineSymbols(BuildTargetGroup targetGroup)
        {
            bool value = false;

            string scriptingDefineSymbols = string.Empty;

            scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            string[] currentSymbos = scriptingDefineSymbols.Split(';').Distinct().OrderBy(smybol => smybol, StringComparer.Ordinal).ToArray();

            foreach (var item in currentSymbos)
            {
                if (item == recommended_VRTKMirageScriptingDefineSymbols)
                {
                    value = true;
                    break;
                }
            }

            return value;
        }

        /// <summary>
        /// Check StartScene Whether Equals "Assets/LenovoMirageARSDK/Scenes/Scene_ARModeGuide.unity"
        /// </summary>
        /// <returns></returns>
        private static bool CheckStartScene()
        {
            bool value = false;

            //Get BuildSettings Scenes List
            EditorBuildSettingsScene[] editorBuildSettingsScenes = EditorBuildSettings.scenes;

            if (editorBuildSettingsScenes.Length!=0)
            {
                if (editorBuildSettingsScenes[0].path==recommended_StartScene)
                {
                    if (editorBuildSettingsScenes[0].enabled)
                    {
                        value = true;
                    }
                }
            }            

            return value;
        }

        #endregion Check Fuc

        #region Set Fuc

        /// <summary>
        /// Set the BuildSettings Value
        /// </summary>
        private static void SetBuildSettings()
        {
            if (EditorUserBuildSettings.activeBuildTarget == recommended_BuildTarget_Android)
            {
                //Orientation Right
                if (!CheckOrientation())
                {
                    SetOrientation();
                }

                // SplashScreen
                if (!CheckShowSplashScreen())
                {
                    SetShowSplashScreen();
                }

                //Android min SDK
                if (!CheckAndroidMinSDK())
                {
                    SetAndroidMinSDK();
                }

                //Andrid Target SDK
                if (!CheckAndroidTargetSDK())
                {
                    SetAndroidTargetSDK();
                }

                //XR Setting
                if (!CheckXRSettings(BuildTargetGroup.Android))
                {
                    SetXRSettings(BuildTargetGroup.Android);
                }

                //Scrip DefineSymbols
                if (!CheckScriptingDefineSymbols(BuildTargetGroup.Android))
                {
                    SetScriptingDefineSymbols(BuildTargetGroup.Android);
                }

                //Start Scene
                if (!CheckStartScene())
                {
                    SetStartScene();
                }
            }
            else if (EditorUserBuildSettings.activeBuildTarget == recommended_BuildTarget_iOS)
            {
                //Orientation Right
                if (!CheckOrientation())
                {
                    SetOrientation(); ;
                }

                //SplashScreen
                if (!CheckShowSplashScreen())
                {
                    SetShowSplashScreen();
                }

                //XR Setting
                if (!(CheckXRSettings(BuildTargetGroup.iOS)))
                {
                    SetXRSettings(BuildTargetGroup.iOS);
                }

                //Scrip DefineSymbols
                if (!CheckScriptingDefineSymbols(BuildTargetGroup.iOS))
                {
                    SetScriptingDefineSymbols(BuildTargetGroup.iOS);
                }

                //Start Scene
                if (!CheckStartScene())
                {
                    SetStartScene();
                }
            }
        }

        /// <summary>
        /// Set the Orientation Settings:LandscapeRight
        /// </summary>
        /// <returns></returns>
        private static void SetOrientation()
        {
            PlayerSettings.defaultInterfaceOrientation = recommended_defaultOrientation;
        }

        /// <summary>
        /// Set  Show Splash Screen:False
        /// </summary>
        /// <returns></returns>
        private static void SetShowSplashScreen()
        {
            PlayerSettings.SplashScreen.show=recommended_showSplashScreen;
        }

        /// <summary>
        /// Set the Android Min SDK:Android API Level 23
        /// </summary>
        /// <returns></returns>
        private static void SetAndroidMinSDK()
        {
            PlayerSettings.Android.minSdkVersion = recommended_AndroidMinSDK;
        }

        /// <summary>
        /// Set Android Target SDK:Android Level Auto
        /// </summary>
        /// <returns></returns>
        private static void SetAndroidTargetSDK()
        {
            PlayerSettings.Android.targetSdkVersion = recommended_AndroidTargetSDK;
        }

        /// <summary>
        /// Set XRSettings:Turn On XRSettings,and List SDK As "None" First,"cardboard" Second
        /// </summary>
        /// <returns>return true if XRSetting right</returns>
        private static void SetXRSettings(BuildTargetGroup targetGroup)
        {
            VREditor.SetVREnabledOnTargetGroup(targetGroup, true);

            VREditor.SetVREnabledDevicesOnTargetGroup(targetGroup, VRDevice);
        }

        /// <summary>
        /// Set Script Define Symbols:Add VRTKMirage ScriptingDefineSymbols
        /// </summary>
        /// <returns></returns>
        private static void SetScriptingDefineSymbols(BuildTargetGroup targetGroup)
        {
            string scriptingDefineSymbols = string.Empty;

            scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Format("{0};{1}", recommended_VRTKMirageScriptingDefineSymbols, scriptingDefineSymbols));
        }

        /// <summary>
        /// Set StartScene Value="Assets/LenovoMirageARSDK/Scenes/Scene_ARModeGuide.unity"
        /// </summary>
        /// <returns></returns>
        private static void SetStartScene()
        {
            //Get BuildSettings Scenes List
            EditorBuildSettingsScene[] editorBuildSettingsScenes = EditorBuildSettings.scenes;
            List<EditorBuildSettingsScene> listEditorBuildSettingsScenes = editorBuildSettingsScenes.ToList<EditorBuildSettingsScene>();

            if (editorBuildSettingsScenes.Length==0)
            {
                listEditorBuildSettingsScenes.Add(new EditorBuildSettingsScene(recommended_StartScene, true));                
            }
            else if (editorBuildSettingsScenes[0].path==recommended_StartScene&& editorBuildSettingsScenes[0].enabled==false)
            {
                listEditorBuildSettingsScenes[0].enabled = true;
            }
            else
            {               
                listEditorBuildSettingsScenes.Insert(0, new EditorBuildSettingsScene(recommended_StartScene, true));
                
            }

            EditorBuildSettings.scenes = listEditorBuildSettingsScenes.ToArray();            
        }

        #endregion Set Fuc

        #region Tool Fuc        

        /// <summary>
        /// Get the Resource Path
        /// </summary>
        /// <returns></returns>
        private string GetResourcePath()
        {
            var ms = MonoScript.FromScriptableObject(this);
            var path = AssetDatabase.GetAssetPath(ms);
            path = Path.GetDirectoryName(path);
            return path.Substring(0, path.Length - "Editor".Length) + "Textures/";
        }

        /// <summary>
        /// Get the Build Target Types:Define the index of Android,iOS
        /// </summary>
        protected virtual void BuildTargetTypes()
        {
            s_BuildTargetNames = new string[2];

            s_BuildTargetNames[0] = recommended_BuildTarget_Android.ToString();
            s_BuildTargetNames[1] = recommended_BuildTarget_iOS.ToString();
        }
        #endregion Tool Fuc
    }
}