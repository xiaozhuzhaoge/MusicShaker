using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SonicBloom.Koreo;
using System;

public class RandomPointController : ControllerBase {

    public Transform LineOneStart;
    public Transform LineOneEnd;
    public Transform LineTwoStart;
    public Transform LineTwoEnd;

    // Use this for initialization
    public override void Start () 
    {
        base.Start();
        beforeDo = () => { SetWhichLineStartAndEndPoint(LineOneStart, LineOneEnd, LineTwoStart, LineTwoEnd); };
    }

    public override void AnalysisData(string text)
    {
        
    }

    public void SetWhichLineStartAndEndPoint(Transform line_1_start, Transform line_1_End, Transform line_2_start, Transform line_2_End)
    {
        System.Random rd = new System.Random();
        int a = rd.Next(0, 4);
        if (a % 2 == 0)
        {
            startPoint = line_1_start;
            endPoint = line_1_End;
        }
        else
        {
            startPoint = line_2_start;
            endPoint = line_2_End;
        }
    }

}
