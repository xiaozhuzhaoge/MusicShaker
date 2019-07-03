namespace LenovoMirageARSDK
{
    using LenovoMirageARSDK.OOBE;
    using SmartLocalization;
    using System.Collections;
    using UnityEngine;
    using Ximmerse.Vision;

    public class UseGuide : MonoBehaviour
    {
        IEntity entity;
        public static System.Action<string, string> CurrentState;

        /// <summary>
        /// Flag to Load Next Level
        /// </summary>
        public static bool IsLoadNextScene = false;

        private IEnumerator Start()
        {
            //SmartCultureInfo sCInfo_Cn = new SmartCultureInfo("zh-CN", "Chinese (China)", "中文 (中国)", false);
            //SmartCultureInfo sCInfo_En = new SmartCultureInfo("en", "English", "English", false);

            yield return new WaitUntil(() => VisionSDK.Instance.Inited);

            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    LanguageManager.Instance.ChangeLanguage("en");
                    break;
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.ChineseTraditional:
                default:
                    LanguageManager.Instance.ChangeLanguage("zh-CN");
                    break;
            }

            entity = this.StateMachine<MainControl>()
                          .OnValueChange(CurrentState)
                          .Begin();
           
            // If ISClient,Exit FSM
            if (LenovoMirageARSDK.MirageAR_SDK.Instance.MISClient)
            {
                // Load Next Level
                EnterNextScene();
            }
        }

        private void Update()
        {
            if (entity != null) entity.Updata();
        }

        private void OnDestroy()
        {
            if (entity != null)
                entity.Destroy();
        }

        #region Private Method

        /// <summary>
        /// Load Next Scene
        /// </summary>
        public static void EnterNextScene()
        {
            // Load The Next Scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        #endregion
    }
}
