using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public partial class ConfigInfo : MonoBehaviour
{
    Dictionary<string, MusicConfig> _musics = new Dictionary<string, MusicConfig>();

    public static void musicConfig(List<MusicConfig> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            ConfigInfo.instance._musics.Add(list[i].name, list[i]);
        }
    }

    public Dictionary<string, MusicConfig> musics
    {
        get
        {
            PreLoadConfig("musics", _musics.Count == 0);
            return _musics;
        }
    }
}
