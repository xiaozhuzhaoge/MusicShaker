using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using System;
using UnityEngine.SceneManagement;

public class RootControllerManager : MonoBehaviour
{
    public static RootControllerManager instance;
    public string eventID;
    Dictionary<string, ControllerBase> controllers = new Dictionary<string, ControllerBase>();

    public void AddControllerToMgr(string name, ControllerBase cb)
    {
        if (!controllers.ContainsKey(name))
            controllers.Add(name, cb);
        Debug.Log(controllers.Count);
    }

    public ControllerBase GetConrtollerFromMgr(string name)
    {
        if (controllers.ContainsKey(name))
            return controllers[name];
        else
            return null;
    }


    public void Awake()
    {
        instance = this;
    }

    public void RegisterEvent(string eventID)
    {
        Koreographer.Instance.RegisterForEvents(eventID, SetMusicView);
    }

    private void SetMusicView(KoreographyEvent koreoEvent)
    {
        string eventValue = koreoEvent.GetTextValue();
        if (eventValue.Equals("end"))
        {
            MusicMgr.Instance.StopMusic();
            ScoreView.instance.ShowText("CurrentScore ：" + DataManager.CurMusicScore);
            ComboView.instance.ShowText("MaxCombo ：" + DataManager.CurMaxCombo);
            DataManager.SaveMusicData(MusicMgr.selectedMusicName);
            
            Invoke("ChangeScene", 3);
            return;
        }

        if (!eventValue.Equals(""))
        {
            string[] data = eventValue.Split(',');
            string type = data[0];
            GetConrtollerFromMgr(type).StartDo(eventValue);
        }
        else
        {
            GetConrtollerFromMgr("point").StartDo(eventValue);
        }
    }


    public void ChangeScene()
    {
        SceneManager.LoadScene("UITest");
    }
}
