using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboView : MonoBehaviour {

    public Text combo;
    public static ComboView instance;
    public TweenScale scale;

    public void Awake()
    {
        instance = this;
        scale = gameObject.AddComponent<TweenScale>();
        scale.from = Vector3.one * 0.8f;
        scale.to = Vector3.one * 1;
        scale.duration = 0.3f;
    }

   
    /// <summary>
    /// 展示分值
    /// </summary>
    public void ShowScore()
    {
        scale.ResetToBeginning();
        scale.PlayForward();
        combo.text = "Combo: " + DataManager.Combo.ToString();
    }

    public void ShowText(string text)
    {
        combo.text = text;
    }
}
