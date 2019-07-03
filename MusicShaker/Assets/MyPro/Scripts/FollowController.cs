using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirzaBeig.ParticleSystems.Demos;

public class FollowController : MonoBehaviour {

    public Transform controller;
    public float speed;

    private void Awake()
    {
#if UNITY_EDITOR
        transform.position = new Vector3(-0.01f, 1.24f, 0.46f);
        transform.GetComponent<FollowMouse>().enabled = true;
        enabled = false;
#endif
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, controller.position,Time.deltaTime * speed);
    }
 

}
