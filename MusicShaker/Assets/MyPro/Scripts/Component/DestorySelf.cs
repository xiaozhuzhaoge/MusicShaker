using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestorySelf : MonoBehaviour {

    public float delay;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, delay);
	}

}
