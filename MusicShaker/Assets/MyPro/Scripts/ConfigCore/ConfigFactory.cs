using UnityEngine;
using System.Collections;
using SimpleJson;
using System.Collections.Generic;
using System;

public enum WorkPlatform
{
	PC = 1,
	Android = 2,
	IOS = 3,
	WP = 4
}

public class ConfigFactory {

	public static ConfigFactory instance;
    public static int fps = 30;
    public static string version;
    public static WorkPlatform platform;
    public static string HttpHost;
    public static string WebSocketHost;
    /// <summary>
    /// UI Perfabs Cache
    /// </summary>
    public static Dictionary<string, Dictionary<string, List<string>>> sundries = new Dictionary<string, Dictionary<string, List<string>>>();
    public static JsonObject SystemConfigs;


	/// <summary>
	/// Read System Configs form "config" json text
	/// </summary>

	public static void ReadSystemConfig()
	{
        JsonObject SystemConfigs = (JsonObject)ConfigReader.ReadJsonConfig("Configs/config", null);
        HttpHost = Utility.CTString(SystemConfigs["HttpHost"]);
        WebSocketHost = Utility.CTString(SystemConfigs["WebSocketHost"]);
        version = Utility.CTString(SystemConfigs["version"]);
        fps = Utility.CTInt(SystemConfigs["fps"]);
        platform = (WorkPlatform)Utility.CTInt(SystemConfigs["platform"]);
        JsonObject jsonConfigs = (JsonObject)((JsonObject)ConfigReader.ReadJsonConfig("Configs/version", null))["configs"];
        ConfigReader.LoadAllConfig(jsonConfigs);
	}
    /// <summary>
    /// Init System Based Configs
    /// </summary>
    public static void SetSystemConfigs()
    {
        Application.targetFrameRate = fps;
    }


    /// <summary>
    /// Init UI Perfabs by configs
    /// </summary>
    public static void InitResourceConfig()
    {
        JsonObject resourceConfig = (JsonObject)ConfigReader.ReadJsonConfig("Configs/resource", null);
        foreach (var name_config in resourceConfig)
        {
            foreach (var kv in (JsonObject)name_config.Value)
            {
                JsonArray obj = (JsonArray)kv.Value;
                List<string> items = new List<string>();
                foreach (var g in obj)
                {
                    items.Add((string)g);
                }

                if (!sundries.ContainsKey(name_config.Key))
                {
                    sundries.Add(name_config.Key, new Dictionary<string, List<string>>());
                }

                var config = sundries[name_config.Key];

                if (!config.ContainsKey(kv.Key))
                {
                    config.Add(kv.Key, new List<string>());
                }

                config[kv.Key] = items;
            }
        }
    }

}
