using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MusicConfig : ConfigMode
{
    public string name;//音乐名字
    public int star;//星级
    public string des;//音乐简介
    public string img;
    public string Koreography;
    public string Event;
    public string singerName;
    public string length;
    public bool isGuide;

    #region IConfig implementation

    public MusicConfig()
    {
    }

    public MusicConfig(SimpleJson.JsonObject o)
    {
        Init(o);
    }

    public override void Init(SimpleJson.JsonObject o)
    {
        base.Init(o);
        this.id = Convert.ToInt32(o["id"]);
        this.name = Convert.ToString(o["name"]);
        this.star = Convert.ToInt32(o["star"]);
        this.des = Convert.ToString(o["description"]);
        this.img = Convert.ToString(o["img"]);
        this.Koreography = Convert.ToString(o["Koreography"]);
        this.Event = Convert.ToString(o["Event"]);
        this.singerName = Convert.ToString(o["singerName"]);
        this.length = Convert.ToString(o["length"]);
        this.isGuide = Convert.ToBoolean(o["isGuide"]);
    }

    /// <summary>
    /// 获取音乐数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static MusicConfig GetMusic(string name)
    {
        if (ConfigInfo.instance.musics.ContainsKey(name))
            return ConfigInfo.instance.musics[name];
        else
            return null;
    }


    /// <summary>
    /// 获取所有音乐信息
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string,MusicConfig> GetAllMusic()
    {
        return ConfigInfo.instance.musics;
    }

    /// <summary>
    /// 获取所有音乐信息排除引导音乐
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string,MusicConfig> GetAllMusicWithoutGuides()
    {
        Dictionary<string, MusicConfig> configs = new Dictionary<string, MusicConfig>();
        foreach(var data in GetAllMusic())
        {
            if (!data.Value.isGuide)
                configs.Add(data.Key, data.Value);
        }
        return configs;
    }

    #endregion


}
