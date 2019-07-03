using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierData  {

    private int mLastI = 0;
    private static float STEP_SIZE = 1.0f / 10;
    private Vector3 point1;
    private Vector3 point2;

    public float getInterpolation(float input,Vector3 point1,Vector3 point2)
    {
        float t = input;

        if (input == 0)
        {
            mLastI = 0;
        }
        double tempX=0;
        for (int i = mLastI; i < 10; i++)
        {
            t = i * STEP_SIZE;
            tempX = cubicEquation(t, point1.x, point2.x);
            if (tempX >= input)
            {
                mLastI = i;
                break;
            }
        }
        double value = cubicEquation(t, point1.y, point2.y);
        Debug.Log("x"+(float)tempX);
        if (input == 1)
        {
            mLastI = 0;
        }
        return (float)value;
    }


    public static double cubicEquation(double t, double p1, double p2)
    {
        double u = 1 - t;
        double tt = t * t;
        double uu = u * u;
        double ttt = tt * t;
        return 3 * uu * t * p1 + 3 * u * tt * p2 + ttt;
    }

    //public static float getT(float num1, float num2, float num3, float num4, Vector3 point)
    //{
    //    float T;
    //    //(num1 / T) - 3 * num1 + 3 * num1 * T * T - num1 * T * T + 3 * num2 - 6 * num2 * T + 3 * T * T * num2 + 3 * num3 * T - 3 * num3 * T * T + num4 * T * T;
    //}
}
