using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainPanel : UIManagerBase {

	private Button studyBtn;
	private Button startGame;

	// Use this for initialization
	void Start () {

		studyBtn = transform.Find ("Study").GetComponent<Button> ();
		startGame = transform.Find ("StartGame").GetComponent<Button>();

		studyBtn.onClick.AddListener (OnStudyBtnClick);
		startGame.onClick.AddListener (OnStartGameBtnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// 新手教学
	/// </summary>
	public void OnStudyBtnClick()
	{
		//设置下个场景要播放得歌曲，然后加载场景
		//SceneManager.LoadScene("Play");
		ShowUI("LeanPanel");
		gameObject.SetActive (false);
	}

	/// <summary>
	/// 开始游戏
	/// </summary>
	public void OnStartGameBtnClick()
	{
		//设置写个场景要播放得歌曲，加载场景
		//SceneManager.LoadScene("Play");
		ShowUI("ChooseSongPanel");
		gameObject.SetActive (false);
	}

}
