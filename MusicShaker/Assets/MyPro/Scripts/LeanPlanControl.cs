using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeanPlanControl : MonoBehaviour {

	// Use this for initialization
    //void Start () {
		
    //}
	
    //// Update is called once per frame
    //void Update () {
	 
    public void SetLine()
    {
        MusicMgr.selectedMusicName = "line";
        SceneManager.LoadScene("Play");
    }

    public void SetGraph()
    {
        MusicMgr.selectedMusicName = "graph";
        SceneManager.LoadScene("Play");
    }

    public void SetPoint()
    {
        MusicMgr.selectedMusicName = "point";
        SceneManager.LoadScene("Play");
    }
}
