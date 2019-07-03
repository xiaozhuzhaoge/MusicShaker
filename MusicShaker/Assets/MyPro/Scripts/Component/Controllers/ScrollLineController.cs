using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ScrollLineController : ControllerBase
{
    TweenTransform tt;
    public Transform leftBound;
    public Transform rightBound;
    public float delayTime;
    public float moveSpeed;

    public static ScrollLineController instance;
 
    public void GetTween()
    {
        if(tt == null)
            tt = gameObject.AddComponent<TweenTransform>();

        tt.from = leftBound;
        tt.to = rightBound;
    }


    float startTime;

    /// <summary>
    /// 分析数据 滚动线 数据结构基础  scroll,发射延迟时间,发射盒子多少秒移动到终点,发射器自身移动速度
    /// </summary>
    /// <param name="text"></param>
    public override void AnalysisData(string text)
    {
        Debug.Log(text);
       
            string[] datas = text.Split(',');
            if(datas.Length < 3)
            {
                Debug.LogError("输入数据长度错误" + datas.Length);
                return;
            }
 
            if(datas.Length == 3)
            {
                delayTime = Convert.ToSingle(datas[1]);
                tt.style = (UITweener.Style)Convert.ToInt32(datas[2]);
                fireSpeed = 1.5f;
                moveSpeed = 1.5f;
            }

            if(datas.Length == 4)
            {
                delayTime = Convert.ToSingle(datas[1]);
                tt.style = (UITweener.Style)Convert.ToInt32(datas[2]);
                fireSpeed = Convert.ToSingle(datas[3]);
                moveSpeed = 1.5f;
            }

            if (datas.Length == 5)
            {
                delayTime = Convert.ToSingle(datas[1]);
                tt.style = (UITweener.Style)Convert.ToInt32(datas[2]);
                fireSpeed = Convert.ToSingle(datas[3]);
                moveSpeed = Convert.ToSingle(datas[4]);
            }

       
       
    }

    public override void StartDo(string text)
    {
        GetTween();
        AnalysisData(text);
        tt.ResetToBeginning();
        tt.PlayForward();

        startTime = Time.time;
        StartCoroutine(waitForFrame());
    }


    IEnumerator waitForFrame()
    {
        int count = 0;
        while(Time.time - startTime <= delayTime)
        {
            if (count % 2 == 0)
                InitPoint();
            count++;
            yield return new WaitForFixedUpdate();
        }
    }
}
