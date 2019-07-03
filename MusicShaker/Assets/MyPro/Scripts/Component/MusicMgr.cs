using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;


public class MusicMgr : MonoBehaviour {

    private static MusicMgr instance;
    public static MusicMgr Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                instance = go.AddComponent<MusicMgr>();
            }
            return instance;
        }

    }

    public static MusicConfig currentMusic;
    public static string selectedMusicName;
    public string musicName;
    private Koreographer grapher;
    private SimpleMusicPlayer smp;
    public Koreography musicTrack;
    public MoveUV groundView;

    public void Start() { }

    public void Awake()
    {
        instance = this;
        grapher = transform.GetComponent<Koreographer>();
        smp = transform.GetComponent<SimpleMusicPlayer>();
        groundView.enabled = false;
        //Todo
        Invoke("PlayMusic", 3);
    }
  
    /// <summary>
    /// 播放当前音乐
    /// </summary>
    /// <param name="music"></param>
    [ContextMenu("Play")]
    public void PlayMusic()
    {
        if (!string.IsNullOrEmpty(selectedMusicName))
            musicName = selectedMusicName;

        currentMusic = MusicConfig.GetMusic(musicName);
        if (currentMusic != null)
        {
            Debug.Log("播放音乐" + musicName); 
            RootControllerManager.instance.eventID = currentMusic.Event;
            RootControllerManager.instance.RegisterEvent(currentMusic.Event);
            musicTrack = ResourceMgr.LoadResource<Koreography>("MusicTracks", currentMusic.Koreography);
            
            smp.LoadSong(musicTrack, 0, false);

            smp.Play();
            groundView.enabled = true;
        }
        else
        {
            string eve = musicName + "Track";
            RootControllerManager.instance.eventID = eve;
            RootControllerManager.instance.RegisterEvent(eve);
            musicTrack = ResourceMgr.LoadResource<Koreography>("MusicTracks", musicName);
            smp.LoadSong(musicTrack, 0, false);
            smp.Play();
            groundView.enabled = true;
            grapher.EventDelayInSeconds = -1.5f;
        }
      
    }

    public void StopMusic()
    {
        smp.Stop();
        groundView.enabled = false;
        ///Todo 结算界面显示
    }

    
}
