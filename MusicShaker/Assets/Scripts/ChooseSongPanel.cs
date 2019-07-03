using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseSongPanel : UIManagerBase {

	private Button songBtn;
	private Button menuBtn;
	private Button settingBtn;

	private Text songName;
	private Text autorName;
	private Text lvText;
	private GameObject musicItem;//音乐列表Item
	private Transform musicItemParent;//音乐Item父节点

	// Use this for initialization
	void Start () {

//		songBtn = transform.Find ("Middle/01/Cover").GetComponent<Button> ();
		menuBtn = transform.Find ("Up/MenuBtn").GetComponent<Button> ();
		settingBtn = transform.Find ("Up/SettingBtn").GetComponent<Button> ();
//
//		songName = transform.Find ("Middle/01/Info/Name").GetComponent<Text> ();
//		autorName = transform.Find ("Middle/01/Info/Autor").GetComponent<Text> ();
//		autorName = transform.Find ("Middle/01/Info/Level").GetComponent<Text> ();

//		if (songBtn != null) {
//			songBtn.onClick.AddListener (OpenSongs);
//		}
		musicItem = transform.Find("Item").gameObject;
		musicItemParent = transform.Find ("SV/Content").transform;

		menuBtn.onClick.AddListener (MenuBtnClick);
		settingBtn.onClick.AddListener (SetBtnClick);

		InitMusicList ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InitMusicList()
	{
		int i = 0;
		Dictionary<string,MusicConfig> allMusics = MusicConfig.GetAllMusicWithoutGuides();

		foreach (var musicCfg in allMusics) {
			GameObject music = Instantiate (musicItem);
			music.transform.SetParent (musicItemParent);
            music.transform.Find("Cover").GetComponent<Image>().sprite = ResourceMgr.LoadResource<Sprite>("Images", musicCfg.Value.img); //封面
			music.transform.Find("Info/Name").GetComponent<Text>().text = "歌曲名："+musicCfg.Value.name;
			music.transform.Find ("Info/Autor").GetComponent<Text> ().text = "歌手名：" + musicCfg.Value.singerName;
			music.transform.Find ("Info/Level").GetComponent<Text> ().text = "难度:  " + musicCfg.Value.star ;
			music.SetActive (true);
			music.transform.transform.localPosition = new Vector3 (-108+i*150, -44.8f, 0.0f);
			i++;
			music.transform.localScale = 0.4f * Vector3.one;
			music.name = musicCfg.Value.name;
			music.transform.GetComponent<MusicItem> ().musicName = musicCfg.Value.name;
			music.transform.GetComponent<MusicItem> ().Init ();

		}
	}

	public void OpenSongs()
	{
//		Debug.Log ("点击歌曲");
//		ShowUI("SurePanel");
		//切换场景
		SceneManager.LoadScene("Play");
	}

	public void MenuBtnClick()
	{
		Debug.Log ("菜单");
	}

	public void SetBtnClick()
	{
		Debug.Log ("设置");
	}
}
