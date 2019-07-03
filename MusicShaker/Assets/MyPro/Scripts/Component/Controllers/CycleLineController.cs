using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CycleLineController : ControllerBase {

    TweenRotation tt;
    public Vector3 fromAngle;
    public Vector3 toAngle;
    float startTime;
    public float delayTime;
    public float moveSpeed;

    public void GetTween()
    {
        if(tt == null)
            tt = gameObject.AddComponent<TweenRotation>();

    }

    public override void AnalysisData(string text)
    {
        Debug.Log(text);
        
            string[] datas = text.Split(',');
            if (datas.Length < 5)
            {
                Debug.LogError("输入数据长度错误" + datas.Length);
                return;
            }

            if (datas.Length == 5)
            {
                delayTime = Convert.ToSingle(datas[1]);
                tt.style = (UITweener.Style)Convert.ToInt32(datas[2]);
                tt.from = fromAngle = new Vector3(0,0,Convert.ToInt32(datas[3]));
                tt.to = toAngle = new Vector3(0, 0, Convert.ToInt32(datas[4]));
                fireSpeed = 1.5f;
                moveSpeed = 1.5f;
            }

            if (datas.Length == 6)
            {
                delayTime = Convert.ToSingle(datas[1]);
                tt.style = (UITweener.Style)Convert.ToInt32(datas[2]);
                tt.from = fromAngle = new Vector3(0, 0, Convert.ToInt32(datas[3]));
                tt.to = toAngle = new Vector3(0, 0, Convert.ToInt32(datas[4]));
                fireSpeed = Convert.ToSingle(datas[5]);
                moveSpeed = 1.5f;
            }

            if (datas.Length == 7)
            {
                delayTime = Convert.ToSingle(datas[1]);
                tt.style = (UITweener.Style)Convert.ToInt32(datas[2]);
                tt.from = fromAngle = new Vector3(0, 0, Convert.ToInt32(datas[3]));
                tt.to = toAngle = new Vector3(0, 0, Convert.ToInt32(datas[4]));
                fireSpeed = Convert.ToSingle(datas[5]);
                moveSpeed = Convert.ToSingle(datas[6]);
            }

    }

    public override void StartDo(string text)
    {
        GetTween();
        AnalysisData(text);

        tt.from = fromAngle;
        tt.to = toAngle;
        tt.duration = delayTime;

        tt.ResetToBeginning();
        tt.PlayForward();

        startTime = Time.time;
        StartCoroutine(waitForFrame());
    }

    IEnumerator waitForFrame()
    {
        int count = 0;
        while (Time.time - startTime <= delayTime)
        {
            if (count % 2 == 0)
                InitPoint();
            count++;
            yield return new WaitForFixedUpdate();
        }
    }
}
