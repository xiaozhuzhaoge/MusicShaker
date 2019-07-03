using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum HitState
{
    Normal = 0,
    Perfect = 1,
    Good = 2,
    Bad = 3
}

public class PointItem : HitItem {

    public HitState state;
    public bool isHitShield;
    Rigidbody rig;
    public Color[] stateColors;
    public MeshRenderer render;

    public void Awake()
    {
        rig = transform.GetComponent<Rigidbody>();
        render = GetComponent<MeshRenderer>();
    }

    public void Start()
    {
        state = HitState.Normal;
    }

    public override void Reset()
    {
        isHitShield = false;
        rig.useGravity = false;
        rig.isKinematic = true;
        state = HitState.Normal;
        SetColor();
        gameObject.SetActive(true);
    }

    public override void Show()
    {
        transform.position = startPoint.position;
        transform.DOMove(endPoint.position, speed);
        Reset();
    }

    public override void OnEnterShield(Collider other)
    {
        isHitShield = true;
        rig.useGravity = true;
        rig.isKinematic = false;
        transform.DOPause();
        CreateEffect("ItemBroken", (go) => {
            go.transform.position = transform.position;
            go.GetComponent<ParticleSystem>().Play();
        });

        CreateEffect("Broken", (go) =>
        {
            go.transform.position = transform.position;
            go.GetComponent<ParticleSystem>().Play();
        });

        if (isHitShield)
        {
            DataManager.SetScoreByHitStateType(state);
            isHitShield = false;
        }

        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-0.5f,0.5f),0,1) * 10,ForceMode.Impulse);

        StateView.instance.Show(state);
        //gameObject.SetActive(false);
    }

    /// <summary>
    /// 在这个区间内触碰是perfect 之后计算时间超过时间段分数递减
    /// </summary>
    /// <param name="other"></param>
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
        StateView.instance.Show(state);
        DataManager.SetScoreByHitStateType(state);
        gameObject.SetActive(false);
    }


    public void SetColor()
    {
        render.material.SetColor("_TintColor", stateColors[(int)state]);  
    }

}
