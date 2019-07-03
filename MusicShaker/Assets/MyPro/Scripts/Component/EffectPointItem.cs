using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPointItem : HitItem {

    public bool isHitShield;
    public Color[] stateColors;
    public MeshRenderer render;
    public GameObject beforeEffect;
    public GameObject afterEffect;

    public void Awake()
    {
        render = GetComponent<MeshRenderer>();
        Reset();
        CreateEffectNoPool("EffectDot",(go)=> { beforeEffect = go; beforeEffect.transform.parent = transform; beforeEffect.transform.localPosition = Vector3.zero; });
        CreateEffectNoPool("HitDot", (go) => { afterEffect = go; afterEffect.transform.parent = transform; afterEffect.transform.localPosition = Vector3.zero; afterEffect.SetActive(false);});
    }
    public override void Reset()
    {
        isHitShield = false;
        SetColor(0);
    }

    public override void OnEnterShield(Collider other)
    {
        isHitShield = true;
        if (isHitShield)
        {
            GrapherCountCollider();
        }
    }

    private void GrapherCountCollider()
    {
        int index = System.Convert.ToInt32(transform.name);
        PictureController.instance.currentGraph.GetComponent<GrapherManager>().SetPrefabStateByIndex(index, true);
        SetColor(1);
        beforeEffect.SetActive(false);
        afterEffect.SetActive(true);
    }


    public void SetColor(int i)
    {
        render.material.SetColor("_TintColor", stateColors[i]);
    }

}
