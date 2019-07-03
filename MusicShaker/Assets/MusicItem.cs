using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicItem : MonoBehaviour {

	public string musicName;
	private Button musicBtn;

	void Awake()
	{
		musicBtn = transform.Find ("Cover").GetComponent<Button> ();

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init()
	{
		musicBtn.onClick.AddListener (OnClickMusicEvent);
	}

	public void OnClickMusicEvent()
	{
		MusicMgr.selectedMusicName = musicName;
		SceneManager.LoadScene("Play");
	}
}
