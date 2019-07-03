namespace LenovoMirageARSDK.OOBE
{
	using UnityEngine;
    using LenovoMirageARSDK;
    using Ximmerse.Vision;
    using DeviceManager;
    using SmartLocalization;

    public class SettingHint : State
    {
        private static bool ctrflag;
        public static bool CtrFlag { get { return ctrflag; } set { ctrflag = value; } }

        public static System.Action<bool> CtrState;

        const string DownAppUrl = "https://www.lenovo.com.cn/activities/515/index.html?pmf_source=P0000000442M0000";

        private Transform Ctr;
        private UnityEngine.UI.Text CtrText;
        private Transform Usb;
        private UnityEngine.UI.Text UsbText;
        private Transform Beacon;
        private Transform BeginButton;
        private Transform HorizontalSettingGroup;
        private Transform HorizontalBtnGroup;
        private Transform DownBtn;
        private Transform BackButton;

        private string BeginButtonName = "Begin";
        private string DownStoreName = "DownStore";
        private string ButtonGroup= "HorizontalBtnGroup";
        private string SettingGroup = "HorizontalSettingGroup";

        private string Title;
        private string YesBtn;
        private string NoBtn;

        private Peripheral ctrPeripheral;
        private Peripheral usbPeripheral;

        bool SetBatteryLevel = false;

        #region Private Method

        /// <summary>
        /// Low Level Color
        /// </summary>
        private Color m_LowLevelColor = Color.red;

        /// <summary>
        /// Normal Level Color
        /// </summary>
        private Color m_NormalLevelColor = Color.white;

        #endregion


        /// <summary>
        /// Save Times
        /// </summary>
        private float tempTime;

        public SettingHint(IEntity parentEntity) : base(parentEntity)
        {
            stateID = "SettingHint";

            VisionSDK.Instance.Connections.OnPeripheralStateChange += OnPeripheralStateChange;

            parentEntity.DestroyEvent(() => { VisionSDK.Instance.Connections.OnPeripheralStateChange -= OnPeripheralStateChange;
                if (LanguageManager.Instance!=null)
                {
                    LanguageManager.Instance.OnChangeLanguage -= ChangeTitleLanguage;
                }                
            });

            LanguageManager.Instance.OnChangeLanguage += ChangeTitleLanguage;
        }

        void ChangeTitleLanguage(LanguageManager manager)
        {
            Title = manager.GetTextValue(CommunalText.TitleKey);
            YesBtn = manager.GetTextValue(CommunalText.TitleYes);
            NoBtn = manager.GetTextValue(CommunalText.TitleNo);
        }

        public SettingHint() : base()
        {
            stateID = "SettingHint";
        }

        public override void EnterState()
        {
            // Save The Time When Enter This Page 
            tempTime = Time.timeSinceLevelLoad;

            Transform tsf = UICtr.Instance.Open(UIType.Setting);

            if (HorizontalSettingGroup == null)
                HorizontalSettingGroup = tsf.Find(SettingGroup);

            if (HorizontalBtnGroup == null)
                HorizontalBtnGroup = tsf.Find(ButtonGroup);
            if (Ctr == null)
                Ctr = HorizontalSettingGroup.Find("Ctr");
            if (CtrText==null)
            {
                CtrText = Ctr.Find("Text").GetComponent<UnityEngine.UI.Text>();
            }

            if (Usb == null)
            {
                Usb = HorizontalSettingGroup.Find("Usb");
#if UNITY_IOS
                Usb.gameObject.SetActive(false);
                HorizontalSettingGroup.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>().spacing = -461f;
#endif
            }
            if (UsbText==null)
            {
                UsbText = Usb.Find("Text").GetComponent<UnityEngine.UI.Text>();
            }

            if (Beacon == null)
                Beacon = HorizontalSettingGroup.Find("Beacon");
            if (BeginButton == null)
                BeginButton = HorizontalBtnGroup.Find(BeginButtonName);
            if (DownBtn == null)
            {
                DownBtn = HorizontalBtnGroup.Find("DownStore");

                bool Launcher = false;
#if !UNITY_EDITOR && UNITY_ANDROID
                ///判断是否存在包名
                AndroidJavaObject packageManager =  AndroidUtil.GetActivity().Call<AndroidJavaObject>("getPackageManager");
                AndroidJavaObject packageInfos = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);
                AndroidJavaObject[] packages = packageInfos.Call<AndroidJavaObject[]>("toArray");
                for (int i = 0; i < packages.Length; i++)
                {
                    AndroidJavaObject applicationInfo = packages[i].Get<AndroidJavaObject>("applicationInfo");
                    if ((applicationInfo.Get<int>("flags") & applicationInfo.GetStatic<int>("FLAG_SYSTEM")) == 0)
                    {
                        string packageName = applicationInfo.Get<string>("packageName");
                        if (packageName == MirageAR_SDK.mLauncherPackageName)
                        {
                            Launcher = true;
                        }
                    }
                }
#elif !UNITY_EDITOR && UNITY_IOS
                //TODO:iOS Check The Launcher Whether Installed

#endif
                if (Launcher)
                {
                    ///如果存在则下载按钮隐藏
                    DownBtn.gameObject.SetActive(false);
                }
                else
                {
                    MirageAR_UIEventListener.Get(DownBtn.gameObject).onClick = () => { Application.OpenURL(DownAppUrl); };
                }
            }

            ChangeTitleLanguage(LanguageManager.Instance);

            if (BackButton == null)
                BackButton = tsf.Find("Back");

            InitSet();

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            BeginButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
            MirageAR_UIEventListener.Get(BackButton.gameObject).onClick = () => { DeviceUtils.ShowDialog(Title, null , YesBtn, () => { Application.Quit(); }, NoBtn , () => { }); };
#else            
            MirageAR_UIEventListener.Get(BackButton.gameObject).onClick = () => { UnityEditor.EditorApplication.isPlaying = false; };
#endif
            MirageAR_UIEventListener.Get(BeginButton.gameObject).onClick += DoEnterGameEvent;
            MirageAR_UIEventListener.Get(Ctr.gameObject).onClick = ()=>{ parent.SetState(MainControl.Transtion.Ctr); };
            MirageAR_UIEventListener.Get(Usb.gameObject).onClick = () => { parent.SetState(MainControl.Transtion.Usb); };
            MirageAR_UIEventListener.Get(Beacon.gameObject).onClick = ()=> { parent.SetState(MainControl.Transtion.Beacon); };

            bool CanEnterGame;
#if !UNITY_IOS
            CanEnterGame = CtrFlag && UsbState.Usbflag;
#else
            CanEnterGame = CtrFlag;
#endif

            if (CanEnterGame)
            {
                BeginButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
            }
            else
            {
#if !UNITY_EDITOR
                MirageAR_UIEventListener.Get(BeginButton.gameObject).onClick -= DoEnterGameEvent;
#endif
            }

            // Init 
            UpdateCtrBattery();
            UpdateUsbBattery();
        }

        void InitSet()
        {
            Usb.Find("Switch").gameObject.SetActive(UsbState.Usbflag);
            Usb.Find("False").gameObject.SetActive(!UsbState.Usbflag);

            Peripheral controllerPeripheral = VisionSDK.Instance.Connections.GetPeripheral("XCobra-0");
            if (controllerPeripheral != null)
            {
                SetCtrState(controllerPeripheral);
            }
        }     

        void SetCtrState(Peripheral peripheral)
        {
            if (ctrPeripheral == null)
            {
                ctrPeripheral = peripheral;
            }

            ctrflag = peripheral.Connected;

            Ctr.Find("Switch").gameObject.SetActive(ctrflag);
            Ctr.Find("False").gameObject.SetActive(!ctrflag);

            if (CtrState != null)
            {
                CtrState(ctrflag);
            }

            if (ctrflag)
            {
                if (peripheral.GetBatteryLevel() < 0)
                {
                    SetBatteryLevel = true;
                }
                else
                {
                    Ctr.Find("Text").GetComponent<UnityEngine.UI.Text>().text = peripheral.GetBatteryLevel().ToString() + "%";
                }
            }
        }

        void OnPeripheralStateChange(object sender, PeripheralStateChangeEventArgs eventArguments)
        {
            if (eventArguments.Peripheral is HmdPeripheral)
            {

            }
            else
            {
                SetCtrState(eventArguments.Peripheral);
            }
        }

        void DoEnterGameEvent()
        {
            parent.SetState(MainControl.Transtion.SettingEnd);
            UICtr.Instance.Close(UIType.Setting);
        }

        public override void ExitState()
        {
            UICtr.Instance.Close(UIType.Setting);
        }

        public override void update()
        {
            //if Space Time > 1S，Update The Baettery Info
            if (Time.timeSinceLevelLoad-tempTime>1)
            {
                UpdateCtrBattery();
                UpdateUsbBattery();

                tempTime = Time.timeSinceLevelLoad;
            }

            if (SetBatteryLevel)
            {
                if (ctrPeripheral != null && ctrPeripheral.GetBatteryLevel() > 0)
                {
                    SetBatteryLevel = false;
                    Ctr.Find("Text").GetComponent<UnityEngine.UI.Text>().text = ctrPeripheral.GetBatteryLevel().ToString() + "%";
                }
            }
        }

        /// <summary>
        /// Update The Controller Battery
        /// </summary>
        void UpdateCtrBattery()
        {
            int batteryValue;

            if (MirageAR_SDK.Instance.ControllerPeripheral==null) 
            {
                batteryValue = 0;
            }
            else
            {
                if (MirageAR_SDK.Instance.ControllerPeripheral.ControllerInput.connectionState != Ximmerse.InputSystem.DeviceConnectionState.Connected)
                {
                    batteryValue = 0;
                }
                else
                {
                    batteryValue = Mathf.Clamp(MirageAR_SDK.Instance.ControllerPeripheral.GetBatteryLevel(), 0, 100);
                }                
            }

            //CtrText.color = batteryValue <= 20 ? m_LowLevelColor : m_NormalLevelColor;
            CtrText.text = batteryValue.ToString()+"%";
            Ctr.Find("electric/Image").GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.RoundToInt(31 * batteryValue)/100, 14);            
        }

        /// <summary>        
        /// Update The Usb Battery
        /// </summary>
        void UpdateUsbBattery()
        {
            int batteryValue;

            if (MirageAR_SDK.Instance.HmdPeripheral == null)
            {
                batteryValue = 0;
            }
            else
            {
                if (MirageAR_SDK.Instance.HmdPeripheral.ControllerInput.connectionState != Ximmerse.InputSystem.DeviceConnectionState.Connected)
                {
                    batteryValue = 0;
                }
                else
                {
                    batteryValue = Mathf.Clamp(MirageAR_SDK.Instance.HmdPeripheral.GetBatteryLevel(), 0, 100);
                }                
            }

            //UsbText.color= batteryValue <= 20 ? m_LowLevelColor : m_NormalLevelColor;
            UsbText.text = batteryValue.ToString() + "%";
            Usb.Find("electric/Image").GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.RoundToInt(31 * batteryValue) / 100, 14);
        }
    }
}