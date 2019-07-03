namespace LenovoMirageARSDK.OOBE
{
	using UnityEngine;
    using LenovoMirageARSDK;
    using DG.Tweening;

    public class UsbState : State
    {
        private string BackButtonName = "Back";

        private Transform Bg;
        private static bool usbflag;
        public static bool Usbflag
        {
            get { return usbflag; }
            set
            {
                usbflag = value;
            }
        }
        public static bool isUsbState;
        public static System.Action<bool> usbState;

        public UsbState(IEntity parentEntity) : base(parentEntity)
        {
            stateID = "UsbState";
        }

        public UsbState() : base()
        {
            stateID = "UsbState";
        }

        Transform tsf;

        public override void EnterState()
        {
            isUsbState = true;
            tsf = UICtr.Instance.Open(UIType.Usb);
            if (Bg == null)
            {
                Bg = tsf.Find("Bg");
            }
            MirageAR_UIEventListener.Get(Bg.gameObject).onBeginDrag += DoDragEvent;
            MirageAR_UIEventListener.Get(tsf.Find(BackButtonName).gameObject).onClick = DoBackEvent;
            UICtr.Instance.Open(UIType.CornerSelect).SetAsLastSibling();
        }

        void DoDragEvent(UnityEngine.EventSystems.BaseEventData data)
        {
            tsf.SetAsLastSibling();
            if ((data as UnityEngine.EventSystems.PointerEventData).delta.x < 0)
            {
                UICtr.Instance.Open(UIType.Beacon);
                MirageAR_UIEventListener.Get(Bg.gameObject).onBeginDrag -= DoDragEvent;
                tsf.DOLocalMoveX(-(tsf as RectTransform).rect.width + 30, 0.7f)
                    .OnComplete(()=> {
                        tsf.localPosition = Vector3.zero;
                        parent.SetState(MainControl.Transtion.Beacon);
                    });
            }
            else if((data as UnityEngine.EventSystems.PointerEventData).delta.x > 0)
            {
                UICtr.Instance.Open(UIType.Control);
                MirageAR_UIEventListener.Get(Bg.gameObject).onBeginDrag -= DoDragEvent;
                tsf.DOLocalMoveX((tsf as RectTransform).rect.width + 30, 0.7f)
                   .OnComplete(() => {
                       tsf.localPosition = Vector3.zero;
                       parent.SetState(MainControl.Transtion.Ctr);
                   });
            }
        }

        void DoBackEvent()
        {
            parent.SetState(MainControl.Transtion.Back);
        }

        public override void ExitState()
        {
            isUsbState = false;
            UICtr.Instance.Close(UIType.Usb);
            UICtr.Instance.Close(UIType.CornerSelect);
        }

        public override void update()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (MirageAR_DeviceApi.CheckARHeadPermission() == 1)
            {
                // It Will Appear Twice Usb Permission Reqeuest
                //MirageAR_DeviceApi.RequestARHeadPermission();
            }
            else if (MirageAR_DeviceApi.CheckARHeadPermission() == 2)
            {
                if (!Usbflag)
                {
                    Usbflag = true;
                    if (usbState != null)
                    {
                        usbState(usbflag);
                    }
                }
            }
            else
            {
                //if (Usbflag)
                //{
                //    Usbflag = false;
                //    if (usbState != null)
                //    {
                //        usbState(usbflag);
                //    }
                //}
            }
#endif
        }

    }
}