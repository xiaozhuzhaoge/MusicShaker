using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

public class GetData : MonoBehaviour
{

    public string TxtName = "HandClapKoreographyTrack";
    // Update is called once per frame
    void Update()
    {

    }

    public void Start()
    {
        StartCoroutine(ontest());

    }

    /// <summary>
    /// 将二进制转成字符串
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string jiema(string s)
    {
        System.Text.RegularExpressions.CaptureCollection cs =
            System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
        byte[] data = new byte[cs.Count];
        for (int i = 0; i < cs.Count; i++)
        {
            data[i] = Convert.ToByte(cs[i].Value, 2);
        }
        return Encoding.Unicode.GetString(data, 0, data.Length);
    }

    public string ByteToString(byte[] inputBytes)
    {
        StringBuilder temp = new StringBuilder(2048);
        foreach (byte tempByte in inputBytes)
        {
            temp.Append(tempByte > 15 ?
            Convert.ToString(tempByte, 2) : '0' + Convert.ToString(tempByte, 2));
        }
        return temp.ToString();
    }
    IEnumerator ontest()
    {
        string persistentDataPath = Application.persistentDataPath + "/Configs/" + TxtName + ".txt";
        Debug.Log(" persistentDataPath : " + persistentDataPath);
        if (!File.Exists(persistentDataPath))
        {
            string fileName = Application.streamingAssetsPath + "/Configs/" + TxtName + ".txt";
            Debug.Log("  fileName :" + fileName);
            if (File.Exists(fileName))
            {
                WWW www = new WWW(fileName);
                //MusicPointMapper.instance.GetData(www.text);


                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.Log("==============load not error==========");
                }
                else
                {
                    Debug.Log("=============load error==========");
                    yield break;
                }
                if (www.isDone)
                {
                    Debug.Log("==============isDone==========");
                    File.WriteAllBytes(persistentDataPath, www.bytes);


                }
                else
                {
                    Debug.Log("==============not isDone==========");
                }
            }
            else
            {
                Debug.Log(fileName + " is not exists");
            }
        }
        else
        {
            Debug.Log(persistentDataPath + " is exists");
        }


    }


}
