using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RealGrpaherItem : HitItem
{
    public HitState state;
    public SpriteRenderer render;
    public Color[] stateColors;
    public bool isHitShield;
 
    void Awake()
    {
        render =  transform.FindChild("image").GetComponent<SpriteRenderer>();
    }
    public override void Show()
    {
        Reset();
        transform.position = startPoint.position;
        transform.DOMove(endPoint.position, speed);
        render.sprite = ResourceMgr.LoadResource<Sprite>("Graphs", PictureController.instance.grapherName);
    }
    public override void Reset()
    {
        base.Reset();
        gameObject.SetActive(true);
    }

    public override void OnStayShield(Collider other)
    {
        isHitShield = true;
        if (PictureController.instance.currentGraph.GetComponent<GrapherManager>().IsAllColliderTrigger() && isHitShield)
        {
            CreateEffect("ItemBroken", (go) =>
            {
                go.transform.position = transform.position;
                go.GetComponent<ParticleSystem>().Play();
            });

            transform.DOMove(startPoint.position, 2.4f).OnComplete(() => {
                gameObject.SetActive(false);
            });

            transform.DORotate(new Vector3(0, 720, 0),2.4f);
            StateView.instance.Show(state);
            DataManager.SetScoreByHitStateType(state);
            isHitShield = false;
        }
  
    }

    public override void OnEnterPerfect(Collider other)
    {
        state = HitState.Perfect;
        SetColor();
    }

    public override void OnEnterGood(Collider other)
    {
        state = HitState.Good;
        SetColor();
     
    }
    public override void OnEnterBad(Collider other)
    {
        state = HitState.Bad;
        DataManager.SetScoreByHitStateType(state);
        StateView.instance.Show(state);
    }

    public void SetColor()
    {
        //if (render == null || stateColors == null) return;
        //render.material.SetColor("_TintColor", stateColors[(int)state]);
    }

    private void FixedUpdate()
    {
        if(endPoint != null)
        if(transform.position == endPoint.position)
        {
            gameObject.SetActive(false);
        }
    }

    private void DoReverseMove()
    {
        transform.DOMove(startPoint.position, speed);
    }
    
}
