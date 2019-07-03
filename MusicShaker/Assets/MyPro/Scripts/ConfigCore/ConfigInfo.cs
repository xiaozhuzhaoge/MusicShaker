using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;
using System.Linq;
using System.Reflection;
public partial class ConfigInfo : MonoBehaviour {

    public static ConfigInfo instance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
            Init();
            ReadAllConfigsFuc();

        }
        else {
            Destroy(gameObject);
        }
    }

    public static void Init()
    {
        RegisterConfigHandler<MusicConfig>("Configs/json/musics", musicConfig);
    }
 
}
