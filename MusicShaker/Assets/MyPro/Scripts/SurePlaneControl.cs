using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurePlaneControl : MonoBehaviour {
    public Text NameText;
    public Text SingerText;
    public Image[] LevelImage;
    public Text SumTimeText;
    public Text ScoreHistroyText;

	private Button startBtn;
	// Use this for initialization
	void Start () {
		MusicConfig Config = MusicConfig.GetMusic("HandClap");
		startBtn = transform.Find ("Button").GetComponent<Button> ();
     
        NameText.text = Config.name;
        SingerText.text = Config.name;

        for(int i = 0; i < LevelImage.Length; i++)
            LevelImage[i].enabled = false;
        for(int i = 0; i < Config.star; i++)
            LevelImage[i].enabled = true;

		startBtn.onClick.AddListener (OnClickStartButton);


	}
	
	// Update is called once per frame
    //void Update () {
		
    //}

    public void OnClickStartButton()
    { 
        //切换场景
        SceneManager.LoadScene("Play");
    }
}
