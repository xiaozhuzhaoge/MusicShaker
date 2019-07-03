using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum ItemType
{
   Item = 0,
   ScrollLine = 1,
   Graphic = 2,
   CycleLine = 3
}

public class HitItem : MonoBehaviour {

    public Transform startPoint;
    public Transform endPoint;
    public float speed;
    public ItemType itemtype;

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            OnEnterShield(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Perfect"))
        {
            OnEnterPerfect(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Good"))
        {
            OnEnterGood(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Bad"))
        {
            OnEnterBad(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            OnExitShield(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {

            OnExitPerfect(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Good"))
        {
            OnExitGood(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Bad"))
        {
            OnExitBad(other);
        }
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            OnStayShield(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Perfect"))
        {

            OnStayPerfect(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Good"))
        {
            OnStayGood(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Bad"))
        {
            OnStayBad(other);
        }
    }

    public virtual void OnEnterPerfect(Collider other)
    {

    }

    public virtual void OnStayPerfect(Collider other)
    {

    }

    public virtual void OnExitPerfect(Collider other)
    {

    }

    public virtual void OnEnterGood(Collider other)
    {

    }

    public virtual void OnStayGood(Collider other)
    {

    }

    public virtual void OnExitGood(Collider other)
    {

    }

    public virtual void OnEnterBad(Collider other)
    {

    }

    public virtual void OnStayBad(Collider other)
    {

    }

    public virtual void OnExitBad(Collider other)
    {

    }

    public virtual void OnEnterShield(Collider other)
    {

    }

    public virtual void OnStayShield(Collider other)
    {

    }

    public virtual void OnExitShield(Collider other)
    {

    }

    
    public void CreateEffect(string effectName,Action<GameObject> callback = null)
    {
        var go = PoolManager.Instance.GetFromCache("Effects", effectName);
        if(callback != null)
        {
            callback(go);
        }
    }

    public void CreateEffectNoPool(string effectName, Action<GameObject> callback = null)
    {
        var go = ResourceMgr.LoadResource("Effects", effectName);
        if (callback != null)
        {
            callback(go);
        }
    }

    public virtual void Show()
    {
      
    }

    /// <summary>
    /// 重置
    /// </summary>
    public virtual void Reset()
    {

    }


}
