namespace LenovoMirageARSDK.OOBE
{
	using UnityEngine;
    using LenovoMirageARSDK;
    using Ximmerse.Vision.Internal;
    using System.Collections.Generic;
    using System.Linq;
    using Ximmerse.Vision;

    public class StateGame : State
    {
        public StateGame(IEntity parentEntity) : base(parentEntity)
        {
            stateID = "StateGame";
        }

        public StateGame() : base()
        {
            stateID = "StateGame";
        }

        const string EditorDeviceModel = "samsung SC-02H";
        Transform tsf;
        private RectTransform SizerPanel;
        private RectTransform CanvasRect;
        string platformName;
        private Devices deviceList;
        private List<Device> devices;
        private Transform NomodelTsf;

        public override void EnterState()
        {
            tsf = UICtr.Instance.Open(UIType.Adaptive);
            CanvasRect = UICtr.Instance.transform as RectTransform;

            // Init UI
            InitUI();

            SizerPanel = tsf as RectTransform;
            TextAsset devices = Resources.Load<TextAsset>("Profiles/devices");

            if (devices != null)
            {
                platformName = (Application.platform == RuntimePlatform.IPhonePlayer) ? "iOS" : "Android";
                deviceList = JsonUtility.FromJson<Devices>(devices.text);
            }

            GetDevices();
        }

        DeviceSettings currentSetting = null;
        void GetDevices(string deviceName = null)
        {
            string model = deviceName ?? SystemInfo.deviceModel;

//#if UNITY_EDITOR
//            model = EditorDeviceModel;
//#endif

            devices = deviceList.Platforms.Single(item => item.Platform == platformName).Devices;

            Device device = devices.Find(item => item.Models.Find(modelName => modelName.Equals(model)) != null);

            device = device ?? devices.Find(item => item.BaseModelNames.Find(modelName => modelName.Contains(model)) != null);

            if (device == null)
            {
                ModelNull();
                return;
            }
#if UNITY_ANDROID
            MirageAR_DeviceApi.PutServiceString("SelectModel", "");
#endif


            SetSize(device);
            parent.SetState(MainControl.Transtion.Start);            
        }

        private int currentIndex=0;

        void ModelNull()
        {
            UnityEngine.UI.Text m_Model = tsf.Find("ModelName").GetComponent<UnityEngine.UI.Text>();
            m_Model.text = devices[currentIndex].Name;

            MirageAR_UIEventListener.Get(tsf.Find("Left").gameObject).onClick = () => { currentIndex--; if (currentIndex < 0) currentIndex = devices.Count - 1; m_Model.text = devices[currentIndex].Name; SetSize(devices[currentIndex]); };
            MirageAR_UIEventListener.Get(tsf.Find("Right").gameObject).onClick = () => { currentIndex++; if (currentIndex >= devices.Count) currentIndex = 0; m_Model.text = devices[currentIndex].Name; SetSize(devices[currentIndex]); };
            MirageAR_UIEventListener.Get(tsf.Find("Button").gameObject).onClick = () => {
#if UNITY_ANDROID && !UNITY_EDITOR
                MirageAR_DeviceApi.PutServiceString("SelectModel", devices[currentIndex].SettingsFile);
#endif
                parent.SetState(MainControl.Transtion.Start);
            };
        }

        void SetSize(Device device)
        {
            TextAsset profile = Resources.Load<TextAsset>("Profiles/" + device.SettingsFile.Replace(".txt", ""));

            if (profile == null)
            {
                currentSetting = new DeviceSettings();
            }
            else
            {
                currentSetting = JsonUtility.FromJson<DeviceSettings>(profile.text);
            }

            float top = 1.0f - currentSetting.UITopOffset;
            float left = 1.0f - currentSetting.UILeftOffset;
            float bottom = 1.0f - currentSetting.UIBottomOffset;
            float right = 1.0f - currentSetting.UIRightOffset;

            SizerPanel.anchoredPosition = new Vector2(CanvasRect.sizeDelta.x * left, -CanvasRect.sizeDelta.y * top);
            SizerPanel.sizeDelta = new Vector2(CanvasRect.sizeDelta.x - right * CanvasRect.sizeDelta.x - SizerPanel.anchoredPosition.x, CanvasRect.sizeDelta.y - bottom * CanvasRect.sizeDelta.y - Mathf.Abs(SizerPanel.anchoredPosition.y));                        
        }

        public override void ExitState()
		{
            UICtr.Instance.Close(UIType.Adaptive);
        }

		public override void update()
		{

		}

#region Private Method
        
        /// <summary>
        /// Init UI
        /// </summary>
        private void InitUI()
        {
            tsf.Find("RawImage").gameObject.SetActive(false);
            tsf.Find("Left").gameObject.SetActive(true);
            tsf.Find("Right").gameObject.SetActive(true);
            tsf.Find("Button").gameObject.SetActive(true);
            tsf.Find("Text").gameObject.SetActive(true);
            tsf.Find("ModelName").gameObject.SetActive(true);
            tsf.Find("Image").gameObject.SetActive(true);
        }

#endregion //Private Method
    }
}