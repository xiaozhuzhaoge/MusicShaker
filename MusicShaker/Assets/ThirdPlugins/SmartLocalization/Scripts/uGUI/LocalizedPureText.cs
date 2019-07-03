namespace SmartLocalization.Editor
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    [RequireComponent(typeof(Text))]
    public class LocalizedPureText : MonoBehaviour
    {
        public string localizedKey = "INSERT_KEY_HERE";
        Text textObject;

        void Start()
        {
            textObject = this.GetComponent<Text>();

            //Subscribe to the change language event
            LanguageManager languageManager = LanguageManager.Instance;
            languageManager.OnChangeLanguage += OnChangeLanguage;

            //Run the method one first time
            OnChangeLanguage(languageManager);
            /*
            if (!Localization.isLocalized) {
                Localization.Localize();
            }*/
        }

        void OnDestroy()
        {
            if (LanguageManager.HasInstance)
            {
                LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
            }
        }

        public void UpdateText(string textID)
        {
            localizedKey = textID;
            OnChangeLanguage(LanguageManager.Instance);
        }

        public void OnChangeLanguage(LanguageManager languageManager)
        {
            string text = languageManager.GetTextValue(localizedKey);
            if (textObject != null && text != "") {
                textObject.text = text;
            }
        }
    }
}