using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MusicStepEffect : MonoBehaviour {

    public static MusicStepEffect instance;
    public AudioSource asu;
    float[] samples = new float[64];
    public Color[] colors;
    public float total;
    public MeshRenderer render;
    public Light[] lights;
    public ParticleSystem[] ps;
    public GameObject[] effects;


    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
        if (asu != null)
        {
            MusicEffect();
        }
	}

    void MusicEffect()
    {
        float value = 0;
        asu.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        for (int i = 0; i < samples.Length; i++)
        {
            value += samples[i];
        }
        total = value;
        ChangeColor(total);
    }

    void ChangeColor(float to)
    {
        Color currentColor = Color.white;
        if (to > 0 && to < 0.05f)
        {
            currentColor = colors[0];
        }
        else if (to >= 0.05f && to < 0.1f)
        {
            currentColor = colors[1];
        }
        else if (to >= 0.1f && to < 0.2f)
        {
            currentColor = colors[2];
        }
        else if (to >= 0.2f && to < 0.3f)
        {
            currentColor = colors[3];
        }
        else if (to >= 0.3f && to < 0.4f)
        {
            currentColor = colors[4];
        }
        else if (to >= 0.4 && to < 0.5f)
        {
            currentColor = colors[5];
        }
        render.material.SetColor("_TintColor", currentColor);
 
        ps.ToList().ForEach(go => {
            go.startColor = currentColor;
        });
    }

    public void CreateEffect(int combo)
    {
        if(combo < 20)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].SetActive(false);
            }
        }
        if (combo >= 20)
        {
            effects[0].SetActive(true);
        }
        if (combo >= 50)
        {
            effects[1].SetActive(true);
        }
        if (combo >= 70)
        {
            effects[2].SetActive(true);
        }
        if (combo >= 100)
        {
            effects[3].SetActive(true);
        }
        
 
    }
}
