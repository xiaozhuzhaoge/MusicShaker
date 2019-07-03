using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour {

    public Text score;
    public static ScoreView instance;
    public TweenScale scale;

    public void Awake()
    {
        scale = gameObject.AddComponent<TweenScale>();
        scale.from = Vector3.one * 0.8f;
        scale.to = Vector3.one;
        scale.duration = 0.3f;
        instance = this;
    }
 
    /// <summary>
    /// 展示分值
    /// </summary>
    public void ShowScore()
    {
        scale.ResetToBeginning();
        scale.PlayForward();
        score.text = "Score: "+  DataManager.CurMusicScore.ToString();
    }

    public void ShowText(string text)
    {
        score.text = text;
    }
}
