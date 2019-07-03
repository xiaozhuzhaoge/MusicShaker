using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StateView : MonoBehaviour {

    public static StateView instance;  
    public Sprite[] images;
    public Image image;
    public TweenScale scale;

    public void Awake()
    {
        instance = this;
        scale = gameObject.AddComponent<TweenScale>();
        scale.from = Vector3.one;
        scale.to = Vector3.zero;
        scale.duration = 2f;
        image = transform.GetComponent<Image>();
    }

    public void Show(HitState state)
    {
        scale.ResetToBeginning();
        scale.PlayForward();
        
        switch (state)
        {
            case HitState.Perfect:
                image.sprite = images[0];
                break;
            case HitState.Good:
                image.sprite = images[1];
                break;
            case HitState.Bad:
                image.sprite = images[2];
                break;
        }
    }
}
