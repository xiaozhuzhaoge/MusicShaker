using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PrefabGrapherItem : HitItem 
{
    public SpriteRenderer render;
    public GrapherManager gm;

    void Awake()
    {
        gm = transform.gameObject.AddComponent<GrapherManager>();
        Reset();
        render = transform.Find("image").GetComponent<SpriteRenderer>();
    }

    public void SetImage(string image)
    {
        if(render != null)
        {
            Debug.Log("读取图片 " + image);
            render.sprite = ResourceMgr.LoadResource<Sprite>("Graphs", image);
        }
    }

    public override void OnEnterBad(Collider other)
    {
        base.OnEnterBad(other);
        Destroy(gameObject);
    }

    public override void Reset()
    {
        gm.childCount = transform.childCount - 1;
        gm.InitDic();
    }
    
   
}
