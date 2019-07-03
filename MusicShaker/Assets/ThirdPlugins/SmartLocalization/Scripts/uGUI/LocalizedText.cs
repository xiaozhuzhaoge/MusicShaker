namespace SmartLocalization.Editor
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    [RequireComponent (typeof (TextMesh))]
    public class LocalizedText : MonoBehaviour 
    {
	    public string localizedKey = "INSERT_KEY_HERE";
        TextMesh textObject;
	
	    void Start () 
	    {
		    textObject = this.GetComponent<TextMesh>();
	
		    //Subscribe to the change language event
		    LanguageManager languageManager = LanguageManager.Instance;
		    languageManager.OnChangeLanguage += OnChangeLanguage;
		
		    //Run the method one first time
		    OnChangeLanguage(languageManager);

            /*if (!Localization.isLocalized) {
                Localization.Localize();
            }*/
        }
	
	    void OnDestroy()
	    {
		    if(LanguageManager.HasInstance)
		    {
			    LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
		    }
	    }
	
	    void OnChangeLanguage(LanguageManager languageManager)
	    {
            string text = languageManager.GetTextValue(localizedKey);
            if (textObject != null && text != "") {
                textObject.text = text;
            }
	    }
    }
}