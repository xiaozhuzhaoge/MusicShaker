using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUV : MonoBehaviour {

    MeshRenderer render;
    Vector2 offset;
    public Vector2 scrollSpeed;

    // Use this for initialization
    void Start () {
        render = transform.GetComponent<MeshRenderer>();
    }
	
    void ScrollUV()
    {
        offset += scrollSpeed;
        render.material.SetTextureOffset("_MainTex", offset);
    }

	// Update is called once per frame
	void Update () {
        ScrollUV();
	}
}
