using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalRandomColor : MonoBehaviour {

    public Color[] colors;

	// Use this for initialization
	void Start () {

        GetComponent<ParticleSystem>().startColor = colors[Random.Range(0,colors.Length)];

    }
	
}
