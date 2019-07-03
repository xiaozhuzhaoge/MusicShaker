using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;

public class ConfigReader
{
	public static string ReadConfig (string name, string content)
	{
		string _content = null;
		if (String.IsNullOrEmpty (content)) {
			TextAsset ta = Resources.Load<TextAsset> (name);
            //Debug.Log(name);
		    _content = ta.text;
            //try {
            //    _content = Utility.Parse (ta.text);
            //}
            //catch (Exception e) {
            //    Debug.LogError ("config name:" + name);
            //    Debug.LogException (e);
            //}
		}
		else {
			_content = content;
		}
		return _content;
	}
	
	public static object ReadJsonConfig (string path, string content)
	{
	    object output;
        string _content = ReadConfig(path, content);
        var result = SimpleJson.SimpleJson.TryDeserializeObject(_content, out output);
        if (!result)
        {
            throw new Exception("cant parse json:" + path);
        }
		return output;
	}
	
	public static UnityEngine.Object[] ReadDir (string path)
	{
		return Resources.LoadAll (path);
	}
	
	public static Hashtable cache = new Hashtable ();
	
	public static void ReadObject2JsonObject (string configName, bool useCache, Action<JsonObject> callback, string content=null)
	{
		if (cache.ContainsKey (configName) && useCache) {
			callback ((JsonObject)cache [configName]);
			return;
		}
		JsonObject temp = (JsonObject)ReadJsonConfig (configName, content);
		cache.Remove (configName);
		cache.Add (configName, temp);
		callback (temp);
	}
	
	public static void ReadArray2Class<T> (string configName, bool useCache, Action<List<T>> callback, string content=null)where T: IConfig, new()
	{
		if (cache.ContainsKey (configName) && useCache) {
			callback ((List<T>)cache [configName]);
			return;
		}
		List<T> infos = new List<T> ();
		JsonArray temp = (JsonArray)ReadJsonConfig (configName, content);
		foreach (var val in temp) {
			JsonObject jo = (JsonObject)val;
			T item = new T ();
			item.Init (jo);
			infos.Add (item);
		}
		
		cache.Remove (configName);
		cache.Add (configName, infos);
		callback (infos);
	}
	
	public static void ReadObject2Class<T> (string configName, bool useCache, Action<List<T>> callback, string content=null) where T: IConfig,  new()
	{
		if (cache.ContainsKey (configName) && useCache) {
			callback ((List<T>)cache [configName]);
			return;
		}
		List<T> infos = new List<T> ();
		try {
			JsonObject temp = (JsonObject)ReadJsonConfig (configName, content);
			foreach (var val in temp) {
				JsonObject jo = (JsonObject)val.Value;
				T item = new T ();
				item.Init (jo);
				infos.Add (item);
			}
		} catch (InvalidCastException e) {
			Debug.LogError ("ReadObject2Class config name:" + configName);
			Debug.LogException (e);
		}       
		
		cache.Remove (configName);
		cache.Add (configName, infos);
		callback (infos);
	}
	
	public static Dictionary<string, string> configsDic = new Dictionary<string, string>();

    public static void LoadAllConfig(JsonObject configs)
	{
		foreach (var kv in configs) {
            Debug.Log(kv.Key + "  " + kv.Value);
			string configName = kv.Key;
            JsonObject o = ((JsonObject)kv.Value);
			string configPath = (string)(o["path"]);
            if(o.ContainsKey("ClassName"))
            {
                  string configClass = (string)(o["ClassName"]);
                  classDic.Add(configName,configClass);
            }
            configsDic.Add(configName, ReadConfig(configPath, null));

		}
	}

    public static void LoadConfigs(BoolAction<string> filter, JsonObject configs)
	{		
		foreach (var kv in configs) {
			string configName = kv.Key;
			string configPath = (string)(((JsonObject)kv.Value)["path"]);
			if(filter == null || filter (configName)){
                configsDic.Add(configName, ReadConfig(configPath, null));
			}
		}
	}
	
	
	public static void UpdateConfig(string configName, string config)
	{
        configsDic[configName] = config;
	}

    public static Dictionary<string, string> classDic = new Dictionary<string, string>();
}
