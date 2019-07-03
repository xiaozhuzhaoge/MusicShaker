namespace LenovoMirageARSDK.OOBE
{
	using UnityEngine;
    using LenovoMirageARSDK;
    using DeviceManager;
    using SmartLocalization;

    public class CommunalText
    {
        public const string TitleKey = "MirageARSDK.OOBE.Title.Exit";
        public const string TitleYes = "MirageARSDK.OOBE.Title.Yes";
        public const string TitleNo = "MirageARSDK.OOBE.Title.No";
        public const string TitleSearching = "MirageARSDK.OOBE.Controller.Searching";
    }

    public class WelcomeState : State
    {
        private string HasButtonName = "Has";
        private string BuyButtonName = "Buy";
        private string BackButtonName = "Back";
        const string BuyUrl = "http://www.techweb.com.cn/article/2017-12-20/2620667.shtml";

        private string Title = "是否退出";
        private string YesBtn = "是";
        private string NoBtn ="否";

        public WelcomeState(IEntity parentEntity) : base(parentEntity)
        {
            stateID = "WelcomeState";
            parentEntity.DestroyEvent(() => {
                if (LanguageManager.Instance!=null)
                {
                    LanguageManager.Instance.OnChangeLanguage -= LanguageChange;
                }                
            });
            LanguageManager.Instance.OnChangeLanguage += LanguageChange;
        }

        void LanguageChange(LanguageManager manager)
        {
            Title = manager.GetTextValue(CommunalText.TitleKey);
            YesBtn = manager.GetTextValue(CommunalText.TitleYes);
            NoBtn = manager.GetTextValue(CommunalText.TitleNo);
        }

        public WelcomeState() : base()
        {
            stateID = "WelcomeState";
        }

        public override void EnterState()
        {
            PlayerPrefs.SetInt("FristEnterARMode", 1);
            Transform tsf = UICtr.Instance.Open(UIType.Welcome);

            MirageAR_UIEventListener.Get(tsf.Find(BackButtonName).gameObject).onClick += DoBackEvent;
            MirageAR_UIEventListener.Get(tsf.Find(HasButtonName).gameObject).onClick += DoHasEvent;
            MirageAR_UIEventListener.Get(tsf.Find(BuyButtonName).gameObject).onClick += DoBuyEvent;

            LanguageChange(LanguageManager.Instance);
        }

        void DoBackEvent()
        {
#if (UNITY_ANDROID||UNITY_IOS)&&!UNITY_EDITOR
            DeviceUtils.ShowDialog(Title, null , YesBtn, ()=> { Application.Quit(); }, NoBtn, ()=> { });     
#else
            // Exit Playing
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            //parent.SetState(MainControl.Transtion.Back);
        }

        void DoHasEvent()
        {
            parent.SetState(MainControl.Transtion.Enter);
        }

        void DoBuyEvent()
        {
            Application.OpenURL(BuyUrl);// parent.SetState(MainControl.Transtion.Buy);
        }

        public override void ExitState()
        {
            UICtr.Instance.Close(UIType.Welcome);
        }

        public override void update()
        {

        }

    }
}