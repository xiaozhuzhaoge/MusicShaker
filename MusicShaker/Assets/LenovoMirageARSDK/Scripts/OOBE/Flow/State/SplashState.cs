namespace LenovoMirageARSDK.OOBE
{
	using UnityEngine;
    using UnityEngine.UI;
    using LenovoMirageARSDK;
    using DeviceManager;
    using System.Collections.Generic;

    public class SplashState : State
	{
        #region Private Properties
        private const string DEFAULT_APP_LOGO = "default_banner";
        #endregion //Private Properties

        /// <summary>
        /// 闪屏图片
        /// </summary>
        public static Sprite SplashSprite;

        private string[] targetPermissionNames;

        private string BgName = "Splash";
        private bool FristEnter;

		public SplashState(IEntity parentEntity):base(parentEntity)
		{
			stateID = "SplashState";
		}

		public SplashState() : base()
		{
			stateID = "SplashState";
		}

        public override void EnterState()
        {
            // Init targetPermissions
            targetPermissionNames = MirageAR_AndroidPermissionManager.MirageARPermissions;

            // Init App Banner
            SplashSprite=GetAppBanner();

            Transform splashTsf = UICtr.Instance.Open(UIType.Splash);
            if (SplashSprite != null)
            {
                splashTsf.Find("Splash").GetComponent<Image>().sprite = SplashSprite;
            }

#if UNITY_EDITOR
            PlayerPrefs.DeleteKey("FristEnterARMode");
#endif

            FristEnter = PlayerPrefs.GetInt("FristEnterARMode", 0) == 0;
            MirageAR_UIEventListener.Get(splashTsf.Find(BgName).gameObject).onClick += DoEvent;

            /// 权限申请
            /// 
#if UNITY_ANDROID && !UNITY_EDITOR
            RequestMultiPermissions();
#endif
        }

        private void RequestMultiPermissions()
        {          

            DeviceUtils.RequestPermissions(MirageAR_AndroidPermissionManager.MirageARPermissions).ThenAction((result) =>
            {
                List<string> TryPermissionsList = new List<string>();

                if (result.IsAllGranted)
                {
                    //Debug.Log("All Permission is Granted");
                }
                else
                {
                    for (int i = 0; i < result.PermissionNames.Length; i++)
                    {
                        if (result.GrantResults[i])
                        {
                            //Debug.Log(result.PermissionNames[i] + " is Granted");
                        }
                        else
                        {
                            if (DeviceUtils.IsRefuseShowRequestDialog(result.PermissionNames[i]))
                            {
                                Application.Quit();
                            }
                            else
                            {
                                TryPermissionsList.Add(result.PermissionNames[i]);
                            }
                        }
                    }
                }

                if (TryPermissionsList.Count > 0)
                {
                    targetPermissionNames = TryPermissionsList.ToArray();
                    TryPermissionsList.Clear();

                    RequestMultiPermissions();
                }
            });
        }

        void DoEvent()
        {
            if (FristEnter)
            {
                parent.SetState(MainControl.Transtion.FirstGame);
            }
            else
            {
                parent.SetState(MainControl.Transtion.SecondGame);
            }
        }

		public override void ExitState()
		{
            UICtr.Instance.Close(UIType.Splash);
		}

		public override void update()
		{

		}

        #region Private Method

        /// <summary>
        /// Get APP Banner
        /// </summary>
        /// <returns></returns>
        private Sprite GetAppBanner()
        {
            Texture2D texture2D = Resources.Load<Texture2D>(DEFAULT_APP_LOGO);

            if (texture2D!=null)
            {
                Sprite sprite = Sprite.Create(texture2D, new Rect(0,0,texture2D.width,texture2D.height),new Vector2(0.5f,0.5f));
                return sprite;
            }   
            else
            {
                Debug.LogWarning("Default APP Logo is Empty,Please Set First!");
                return null;
            }
        }

        #endregion

    }
}