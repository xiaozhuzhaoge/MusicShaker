using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr : MonoBehaviour {

    private static ResourceMgr instance;

    public static ResourceMgr Instance
    {
        get { if (instance == null) {
                GameObject rm = new GameObject();
                instance = rm.AddComponent<ResourceMgr>();
            }
            return instance;
        }
    }
     
    public static GameObject LoadResource(string path, string name)
    {
        //ToDO  load from ab

        //Load from Resources
        try
        {
            Debug.Log(path + " " + name);
            return Instantiate<GameObject>(LoadResource<GameObject>(path, name));
        }
        catch
        {
            return null;
        }
       
    }

    public static AudioClip LoadAudio(string path,string name)
    {
        //ToDO  load from ab

        //Load from Resources
        return LoadResource<AudioClip>(path,name);
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T LoadResource<T>(string path,string name) where T : Object
    {
        return Resources.Load<T>(path + "/" + name);
    }
}
