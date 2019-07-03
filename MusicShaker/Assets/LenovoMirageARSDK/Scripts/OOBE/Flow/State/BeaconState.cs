namespace LenovoMirageARSDK.OOBE
{
	using UnityEngine;
    using LenovoMirageARSDK;
    using DG.Tweening;

	public class BeaconState : State
	{
        private string BackButtonName = "Back";
        public static bool isBeaconState;

		public BeaconState(IEntity parentEntity):base(parentEntity)
		{
			stateID = "BeaconState";
		}

		public BeaconState() : base()
		{
			stateID = "BeaconState";
		}
        Transform tsf;

        public override void EnterState()
		{
            Debug.Log("XX");
            isBeaconState = true;
            tsf = UICtr.Instance.Open(UIType.Beacon);
            MirageAR_UIEventListener.Get(tsf.Find("Bg").gameObject).onBeginDrag += DoDragEvent;
            MirageAR_UIEventListener.Get(tsf.Find(BackButtonName).gameObject).onClick = () => { parent.SetState(MainControl.Transtion.Back); };
            UICtr.Instance.Open(UIType.CornerSelect).SetAsLastSibling();
		}

        void DoDragEvent(UnityEngine.EventSystems.BaseEventData data)
        {
            tsf.SetAsLastSibling();
            if ((data as UnityEngine.EventSystems.PointerEventData).delta.x < 0)
            {
#if UNITY_IOS
                UICtr.Instance.Open(UIType.Control);
#else
                UICtr.Instance.Open(UIType.Control);
#endif
                MirageAR_UIEventListener.Get(tsf.Find("Bg").gameObject).onBeginDrag -= DoDragEvent;
                tsf.DOLocalMoveX(-(tsf as RectTransform).rect.width  + 30, 0.7f)
                    .OnComplete(() => {
                        tsf.localPosition = Vector3.zero;
#if UNITY_IOS
                        parent.SetState(MainControl.Transtion.Ctr);
#else 
                        parent.SetState(MainControl.Transtion.Ctr);
#endif
                    });
            }
            else if ((data as UnityEngine.EventSystems.PointerEventData).delta.x > 0)
            {
#if UNITY_IOS
                UICtr.Instance.Open(UIType.Control);
#else
                UICtr.Instance.Open(UIType.Usb);
#endif
                MirageAR_UIEventListener.Get(tsf.Find("Bg").gameObject).onBeginDrag -= DoDragEvent;
                tsf.DOLocalMoveX((tsf as RectTransform).rect.width  + 30, 0.7f)
                   .OnComplete(() => {
                       tsf.localPosition = Vector3.zero;
#if UNITY_IOS
                       parent.SetState(MainControl.Transtion.Ctr);
#else 
                        parent.SetState(MainControl.Transtion.Usb);
#endif
                   });
            }
        }

        public override void ExitState()
        {
            isBeaconState = false;
            UICtr.Instance.Close(UIType.Beacon);
            UICtr.Instance.Close(UIType.CornerSelect);
        }

		public override void update()
		{

		}

	}
}