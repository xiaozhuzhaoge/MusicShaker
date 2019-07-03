using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Using The SmartLocalization
using SmartLocalization;


namespace LenovoMirageARSDK.Example
{
    /// <summary>
    /// The Example of LenovoMirageARSDK
    /// Example of Localization
    /// </summary>
    public class BasicExample_6_LocalizationTest_Ctrl : MonoBehaviour
    {
        #region Private Properties

        /// <summary>
        /// LanguageManager Instance
        /// </summary>
        private LanguageManager languageManager;

        private SmartCultureInfo sCInfo_Cn=new SmartCultureInfo("zh-CN", "Chinese (China)", "中文 (中国)", false);
        private SmartCultureInfo sCInfo_En=new SmartCultureInfo("en", "English", "English", false);

        #endregion

        #region Unity Methods             

        IEnumerator Start()
        {
            //Wait Until Has LanguageManager Instance
            yield return new WaitUntil(() => LanguageManager.HasInstance);
            languageManager = LanguageManager.Instance;

            //Init the Default Language：
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    languageManager.ChangeLanguage("en");
                    break;
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.ChineseTraditional:
                default:
                    languageManager.ChangeLanguage("zh-CN");
                    break;
            }
        }

        #endregion  //Unity Methods

        #region Private Methods
        #endregion

        #region Public Methods

        /// <summary>
        /// Switch Language To Chinese
        /// </summary>
        public void SwitchLanguageToCn()
        {           
            if (languageManager.CurrentlyLoadedCulture == sCInfo_Cn) return;

            languageManager.ChangeLanguage("zh-CN");
        }

        /// <summary>
        /// Switch Language To English
        /// </summary>
        public void SwitchLanguageToEn()
        {
            if (languageManager.CurrentlyLoadedCulture == sCInfo_En) return;

            languageManager.ChangeLanguage("en");
        }

        #endregion  //Public Methods
    }
}
