namespace LenovoMirageARSDK.OOBE
{
    using LenovoMirageARSDK;
	using UnityEngine;
    using Ximmerse.Vision;
    using DG.Tweening;
    using SmartLocalization;

    public class ControllState : State
    {
        private string GifOneName = "Gif1";
        private string GifTwoName = "Gif2";
        private string SearchName = "Search";        
        private string BackButtonName ="Back";
        
        private Transform SearchTsf;
        private Transform textStartSearch;
        private Transform textSearching;
        private Transform textConnected;
        //private UnityEngine.UI.Text m_Text;
        private bool isPairing = false;
        private float PairingTime = 10f;
        private float TempTime = 0;

        public static bool isControllstate;

        public ControllState(IEntity parentEntity) : base(parentEntity)
        {
            stateID = "ControllState";

            // Bind OnPeripheralStateChange
            VisionSDK.Instance.Connections.OnPeripheralStateChange += OnPeripheralStateChange;
            parentEntity.DestroyEvent(() => {
                VisionSDK.Instance.Connections.OnPeripheralStateChange -= OnPeripheralStateChange;
            });
        }

        public ControllState() : base()
        {
            stateID = "ControllState";
        }

        Transform tsf;
        public override void EnterState()
        {
            isControllstate = true;
            tsf = UICtr.Instance.Open(UIType.Control);

            SettingHint.CtrState += SetText;

            if (SearchTsf == null)
            {
                SearchTsf = tsf.Find(SearchName);
                //m_Text = SearchTsf.GetComponentInChildren<UnityEngine.UI.Text>();
            }

            if (textStartSearch == null) textStartSearch = SearchTsf.Find("Text");
            if (textSearching == null) textSearching = SearchTsf.Find("Text (1)");
            if (textConnected == null) textConnected = SearchTsf.Find("Text (2)");          

            MirageAR_UIEventListener.Get(tsf.Find("Bg").gameObject).onBeginDrag += DoDragEvent;
            MirageAR_UIEventListener.Get(SearchTsf.gameObject).onClick = OnDoSearchEvent;
            MirageAR_UIEventListener.Get(tsf.Find(BackButtonName).gameObject).onClick = DoBackEvent;
            isPairing = false;

            Transform tsf2 = UICtr.Instance.Open(UIType.CornerSelect);
            tsf2.SetAsLastSibling();

            //Init UI
            InitUI();
           
        }

        void DoDragEvent(UnityEngine.EventSystems.BaseEventData data)
        {
            tsf.SetAsLastSibling();
            if ((data as UnityEngine.EventSystems.PointerEventData).delta.x < 0)
            {
#if UNITY_IOS
                UICtr.Instance.Open(UIType.Beacon);
#else 
                UICtr.Instance.Open(UIType.Usb);
#endif
                MirageAR_UIEventListener.Get(tsf.Find("Bg").gameObject).onBeginDrag -= DoDragEvent;
                tsf.DOLocalMoveX(-(tsf as RectTransform).rect.width + 30, 0.7f)
                    .OnComplete(() => {
                        tsf.localPosition = Vector3.zero;

#if UNITY_IOS
                        parent.SetState(MainControl.Transtion.Beacon);
#else 
                        parent.SetState(MainControl.Transtion.Usb);
#endif

                        
                    });
            }
            else if ((data as UnityEngine.EventSystems.PointerEventData).delta.x > 0)
            {
#if UNITY_IOS
                UICtr.Instance.Open(UIType.Beacon);
#else
                UICtr.Instance.Open(UIType.Beacon);
#endif

                MirageAR_UIEventListener.Get(tsf.Find("Bg").gameObject).onBeginDrag -= DoDragEvent;
                tsf.DOLocalMoveX(((tsf as RectTransform).rect.width) - 30, 0.7f)
                   .OnComplete(() => {
                       tsf.localPosition = Vector3.zero;

#if UNITY_IOS
                        parent.SetState(MainControl.Transtion.Beacon);
#else
                        parent.SetState(MainControl.Transtion.Beacon);
#endif
                   });
            }
        }

        void SetText(bool flag)
        {
            //if (flag)
            //{
            //    m_Text.text = "已连接控制器";
            //}            
        }

        void DoBackEvent()
        {
            parent.SetState(MainControl.Transtion.Back);
        }

        void OnDoSearchEvent()
        {
            if (isPairing)
                return;

            //m_Text.text ="正在搜索";
            TempTime = 0;
            isPairing = true;
            VisionSDK.Instance.Tracking.StartPairing();

            textStartSearch.gameObject.SetActive(false);
            textConnected.gameObject.SetActive(false);
            textSearching.GetComponent<UnityEngine.UI.Text>().text=LanguageManager.Instance.GetTextValue(CommunalText.TitleSearching);
            textSearching.gameObject.SetActive(true);
            SearchTsf.GetComponent<UnityEngine.UI.Button>().interactable =false;
            MirageAR_UIEventListener.Get(SearchTsf.gameObject).onClick = null;
        }

        public override void ExitState()
        {
            isControllstate = false;
            UICtr.Instance.Close(UIType.Control);
            UICtr.Instance.Close(UIType.CornerSelect);           

            VisionSDK.Instance.Tracking.StopPairing();
            isPairing = false;
        }

        public override void update()
        {
            if (isPairing)
            {
                TempTime += Time.deltaTime;
                if (TempTime >= PairingTime)
                {
                    isPairing = false;                    
                    VisionSDK.Instance.Tracking.StopPairing();
                    InitUI();
                }
                else
                {
                    textSearching.GetComponent<UnityEngine.UI.Text>().text = LanguageManager.Instance.GetTextValue(CommunalText.TitleSearching) + "..." +(PairingTime- Mathf.RoundToInt(TempTime));
                }
            }
        }

#region Private Method

        /// <summary>
        /// Init UI
        /// </summary>
        private void InitUI()
        {
            if (MirageAR_SDK.Instance.ControllerPeripheral.ControllerInput.connectionState == Ximmerse.InputSystem.DeviceConnectionState.Connected)
            {
                InitSearchText(true);
            }
            else
            {
                InitSearchText(false);
            }
        }

        /// <summary>
        /// Init the Search Text,True Connected,False Not Connected
        /// </summary>
        private void InitSearchText(bool flag)
        {
            SearchTsf.GetComponent<UnityEngine.UI.Button>().interactable = !flag;
            if (!flag)
            {
                MirageAR_UIEventListener.Get(SearchTsf.gameObject).onClick = OnDoSearchEvent;
            }
            else
            {
                MirageAR_UIEventListener.Get(SearchTsf.gameObject).onClick = null;
            }
            
            textStartSearch.gameObject.SetActive(!flag);           
            textConnected.gameObject.SetActive(flag);
            textSearching.gameObject.SetActive(false);
        }

        void OnPeripheralStateChange(object sender, PeripheralStateChangeEventArgs eventArguments)
        {
            if (eventArguments.Peripheral is ControllerPeripheral)
            {
                if (eventArguments.Peripheral.ControllerInput.connectionState==Ximmerse.InputSystem.DeviceConnectionState.Connected)
                {
                    InitSearchText(true);

                    if (isPairing)
                    {
                        isPairing = false;
                        VisionSDK.Instance.Tracking.StopPairing();                        
                    }
                }
                else
                {
                    InitSearchText(false);
                }
            }
        }

#endregion

    }
}