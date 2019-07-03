namespace LenovoMirageARSDK
{
    using LenovoMirageARSDK.OOBE;
    using UnityEngine;
    using Ximmerse.Vision;

    public class CornerSelectPanel : MonoBehaviour
    {
        private Transform Ctr;
        private Transform Usb;
        private Transform Beacon;

        private Transform CtrFlag;
        private Transform UsbFlag;
        private Transform BeaconFlag;
        private Transform HorizontalGroup;

        private UnityEngine.UI.Image CtrSwitch;
        private UnityEngine.UI.Image UsbSwitch;
        private Transform BeaconSwitch;

        [SerializeField]
        private UnityEngine.Sprite[] imagelist;

        private string CurrentState;

        private void Awake()
        {
            HorizontalGroup = transform.Find("HorizontalGroup");

            Ctr = HorizontalGroup.Find("Ctr");
            Usb = HorizontalGroup.Find("Usb");
            Beacon = HorizontalGroup.Find("Beacon");

            CtrFlag = Ctr.Find("Flag");
            UsbFlag = Usb.Find("Flag");
            BeaconFlag = Beacon.Find("Flag");

            CtrSwitch = Ctr.Find("Switch").GetComponent<UnityEngine.UI.Image>();
            UsbSwitch = Usb.Find("Switch").GetComponent<UnityEngine.UI.Image>();
            BeaconSwitch = Beacon.Find("Switch");

            MirageAR_UIEventListener.Get(Ctr.gameObject).onClick = DoCtrEvent;
            MirageAR_UIEventListener.Get(Usb.gameObject).onClick = DoUsbEvent;
            MirageAR_UIEventListener.Get(Beacon.gameObject).onClick = DoBeaconEvent;
            SettingHint.CtrState += SetCtr;
            UsbState.usbState += SetUsb;

#if UNITY_IOS
            MirageAR_UIEventListener.Get(Usb.gameObject).onClick -= DoUsbEvent;
            UsbState.usbState -= SetUsb;         
            HorizontalGroup.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>().spacing = -150.17f;
            Usb.gameObject.SetActive(false);
#endif
        }

        void SetCtr(bool flag)
        {
            if (CtrFlag.gameObject.activeInHierarchy)
            {
                if (flag)
                {
                    CtrSwitch.sprite = imagelist[3];
                }
                else
                {
                    CtrSwitch.sprite = imagelist[1];
                }
            }
            else
            {
                if (flag)
                {
                    CtrSwitch.sprite = imagelist[2];
                }
                else
                {
                    CtrSwitch.sprite = imagelist[0];
                }
            }
        }

        void SetUsb(bool flag)
        {
            if (UsbFlag.gameObject.activeInHierarchy)
            {
                if (flag)
                {
                    UsbSwitch.sprite = imagelist[3];
                }
                else
                {
                    UsbSwitch.sprite = imagelist[1];
                }
            }
            else
            {
                if (flag)
                {
                    UsbSwitch.sprite = imagelist[2];
                }
                else
                {
                    UsbSwitch.sprite = imagelist[0];
                }
            }
        }

        private void OnEnable()
        {
            CtrFlag.gameObject.SetActive(ControllState.isControllstate);
#if !UNITY_IOS
            UsbFlag.gameObject.SetActive(UsbState.isUsbState);
            SetUsb(UsbState.Usbflag);
#endif
            BeaconFlag.gameObject.SetActive(BeaconState.isBeaconState);

            SetCtr(SettingHint.CtrFlag);
        }

        void DoCtrEvent()
        {
            this.StateMachineEvent("Ctr");
        }

        void DoUsbEvent()
        {
            this.StateMachineEvent("Usb");
        }

        void DoBeaconEvent()
        {
            this.StateMachineEvent("Beacon");
        }

        private void OnDestroy()
        {
            SettingHint.CtrState -= SetCtr;
            UsbState.usbState -= SetUsb;
        }

    }
}
