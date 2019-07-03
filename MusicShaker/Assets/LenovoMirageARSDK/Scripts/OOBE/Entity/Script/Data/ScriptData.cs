using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace LenovoMirageARSDK.OOBE
{
    [System.Serializable]
    public class ScriptData
    {
        private static string ScriptPath = StateEditorPathConfig.ScriptGeneratorPath;

        public string Name { get { return name; } set { name = value; } }
        [SerializeField]
        private string name;

        public static string masterName = "MainControl";

        [SerializeField]
        public List<StateData> m_AllState=new List<StateData> ();

        [SerializeField]
        public List<string> m_transition = new List<string>();

        private ScriptData() { }
        public ScriptData(string name)
        {
            this.name = name;
            m_AllState = new List<LenovoMirageARSDK.OOBE.StateData>();
        }

        public void SetMain()
        {
            Name = masterName;        
        }

        public static string[] GetScriptList()
        {
            DirectoryInfo info = new DirectoryInfo(ScriptPath);
            FileInfo[] finfo = info.GetFiles("*.cs");

            string[] m_ailist = new string[finfo.Length];
            for (int i = 0; i < finfo.Length; i++)
            {
                m_ailist[i] = finfo[i].Name;
            }
            return m_ailist;
        }
    }
}