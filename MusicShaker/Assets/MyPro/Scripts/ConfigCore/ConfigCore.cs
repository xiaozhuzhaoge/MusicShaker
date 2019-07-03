using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJson;

public partial class ConfigInfo : MonoBehaviour {

    public static List<string> preGameConfigs = new List<string>();
    /// <summary>
    /// 创建配置文件加载
    /// </summary>
    public static void BuildLoginConfigRef()
    {
        ConfigInfo.instance = new ConfigInfo();
        foreach (var configName in preGameConfigs)
        {
            HandleConfig(configName, ConfigReader.configsDic[configName]);
        }
    }
    /// <summary>
    /// 预加载功能
    /// </summary>
    /// <param name="configName">配置文件名</param>
    /// <param name="shouldLoad">是否加载</param>
    public static void PreLoadConfig(string configName, bool shouldLoad)
    {
        if (!shouldLoad) return;

        if (ConfigReader.configsDic.ContainsKey(configName))
        {
            var val = ConfigReader.configsDic[configName];
            HandleConfig(configName, val);
        }
    }
    /// <summary>
    /// 注册配置文件句柄
    /// </summary>
    /// <typeparam name="T">配置文件类型</typeparam>
    /// <param name="configName">配置文件名</param>
    /// <param name="handler">数据链句柄</param>
    static void RegisterConfigHandler<T>(string configName, Action<List<T>> handler) where T : ConfigMode, new()
    {
        configHandlers.Add(configName, (string config) =>
        {
            ConfigReader.ReadArray2Class<T>(configName, false, handler, config);
        });
    }

    public Dictionary<string,string> keyValue = new Dictionary<string, string>(); 

    public static void ReadAllConfigsFuc()
    {
        foreach (var data in configHandlers)
        {
            string v = "";
            data.Value.Invoke(v);
        }
    }

    /// <summary>
    /// 异步加载配置文件句柄
    /// </summary>
    static Dictionary<string, Action<String>> configHandlers = new Dictionary<string, Action<String>>();
    static void HandleConfig(string configName, string content)
    {
        try
        {
            if (configHandlers.ContainsKey(configName))
            {
                configHandlers[configName](content);
            }
            else
            {
                //Debug.Log("configName not found!=>" + configName);
            }

        }
        catch (Exception e)
        {
            //Debug.Log("Exception=>HandleConfig: configName:" + configName + "===");
            //Debug.LogException(e);

            var dir = Application.persistentDataPath + "/ConfigData";
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
        }

    }

}
