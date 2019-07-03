using UnityEngine;
using System.Collections;
using System;

public class ConfigMode : IConfig {

    public int id;


    public ConfigMode ()
	{
	}

    public ConfigMode(SimpleJson.JsonObject o)
	{
        Debug.Log(o);
		Init (o);
	}

	public virtual void Init (SimpleJson.JsonObject o)
	{
        if(o.ContainsKey("id"))
        id = Convert.ToInt32(o["id"]);
	}
}
