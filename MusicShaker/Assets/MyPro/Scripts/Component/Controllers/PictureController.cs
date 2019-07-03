using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PictureController : ControllerBase 
{
    public static PictureController instance;
    public string grapherName;
    public Transform Show_Grapher_Prefab_pos;
    public GameObject currentGraph;

    private void Awake()
    {
        instance = this;
    }

    public override void AnalysisData(string text)
    {
        string[] datas = text.Split(',');
        if (datas.Length < 3)
        {
            Debug.LogError("输入数据长度错误" + datas.Length);
            return;
        }

        if (datas.Length == 3)
        {
            fireSpeed = Convert.ToSingle(datas[1]);
            grapherName = Convert.ToString(datas[2]);
            prefabName = "grapher";
        }
 
    }

    protected override void InitPoint()
    {
        base.InitPoint();
        InitLinkPrefab();
    }

    private void InitLinkPrefab()
    {
        currentGraph = ResourceMgr.LoadResource("Prefabs", "Link"+ UnityEngine.Random.Range(1,4));
        if (currentGraph == null)
            return;
        currentGraph.transform.position = Show_Grapher_Prefab_pos.position;
        currentGraph.GetComponent<PrefabGrapherItem>().SetImage(grapherName);
        DestorySelf self = currentGraph.AddComponent<DestorySelf>();
        self.delay = fireSpeed;
    }

}
