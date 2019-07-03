using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControllerBase : MonoBehaviour {

    public string ControllerType;
    public Transform startPoint;
    public Transform endPoint;
    public Action beforeDo;
    public Action afterDo;
    public string prefabName;

    /// <summary>
    /// 控制器运动速度
    /// </summary>
    public float fireSpeed;

    public virtual void Start()
    {
        RootControllerManager.instance.AddControllerToMgr(ControllerType,this);
    }

    /// <summary>
    /// 开始执行生成显示的功能
    /// </summary>
    public virtual void StartDo(string text)
    {
        AnalysisData(text);
        if (beforeDo != null)
            beforeDo();
        InitView();
        if (afterDo != null)
            afterDo();
    }

    /// <summary>
    /// 解析输入数据结构
    /// </summary>
    /// <param name="text"></param>
    public virtual void AnalysisData(string text)
    {

    }

    /// <summary>
    /// 生成格子
    /// </summary>
    protected virtual void InitPoint()
    {
        if (prefabName.Equals(""))
            prefabName = "PointItem";
        //用加载得到的资源对象，实例化游戏对象，实现游戏物体的动态加载
        var go = PoolManager.Instance.GetFromCache("Prefabs", prefabName);

        HitItem item = go.GetComponent<HitItem>();
        item.startPoint = startPoint;
        item.endPoint = endPoint;
        item.speed = fireSpeed;
        item.Reset();
        item.Show();
    }


    /// <summary>
    /// 用于初始化显示
    /// </summary>
    public virtual void InitView()
    {
        InitPoint();
    }
}
