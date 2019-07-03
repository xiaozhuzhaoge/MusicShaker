using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapherManager : MonoBehaviour
{
    public  Dictionary<int, bool> info = new Dictionary<int, bool>();
    public int childCount;


    public bool  IsAllColliderTrigger()
    {
        if (info.Count == 0) InitDic();
        for (int i = 1; i <= info.Count; i++)
        {
            if (info[i] == false) { return false;}           
        }
        return true;
    }

    public void InitDic()
    {
        for (int i = 1; i <= childCount; i++)
        {
            info[i] = false;
        }
    }

    public void SetPrefabStateByIndex(int index,bool state)
    {
        info[index] = state;
    }

}
