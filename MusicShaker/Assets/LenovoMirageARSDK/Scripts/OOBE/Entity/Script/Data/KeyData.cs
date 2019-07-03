using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace LenovoMirageARSDK.OOBE
{

    [System.Serializable]
    public class KeyData
    {
        public static string path = StateEditorPathConfig.ScriptGeneratorPath + "/Key.json";

        [SerializeField]
        private List<string> KeyList = new List<string>();

        public KeyData()
        {
        }

        public KeyData ReadData()
        {
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                KeyData tempData = JsonUtility.FromJson<KeyData>(text);
                KeyList.AddRange(tempData.KeyList);
            }
            return this;
        }

        public KeyData ReadData(ScriptData m_ScriptData)
        {
            KeyList.AddRange(m_ScriptData.m_transition);
            return this;
        }

        public void AddKey(string content)
        {
            if (KeyList == null)
            {
                KeyList = new List<string>();
            }
            if (!KeyList.Contains(content))
                KeyList.Add(content);
        }

        public List<string> GetKey()
        {
            return KeyList;
        }

        public void DeleKey(string key)
        {
            if (KeyList == null)
            {
                return;
            }
            if (KeyList.Contains(key))
            {
                KeyList.Remove(key);
            }
        }

        public void SaveData()
        {
            string jsonText = JsonUtility.ToJson(this);
            File.WriteAllText(path, jsonText);
        }

    }
}
