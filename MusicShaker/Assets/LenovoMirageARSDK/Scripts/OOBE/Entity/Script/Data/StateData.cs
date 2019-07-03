using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LenovoMirageARSDK.OOBE
{
    [System.Serializable]
    public class relationData
    {
        [SerializeField]
        public string key;
        [SerializeField]
        public StateData data;

        public relationData(string key,StateData data)
        {
            this.key = key;
            this.data = data;
        }           
    }

    [System.Serializable]
    public class relation
    {
        [SerializeField]
        public List<relationData> m_NextData=new List<relationData> ();
        public bool isChangKey;
    }
    
    [System.Serializable]
    public class StateData {

        private static string ScriptPath= StateEditorPathConfig.StateGeneratorPath;

        public string ID { set { iD = value; } get { return iD; } }
        [SerializeField]
        private string iD;

        [SerializeField]
        private bool canChange=true;
        public bool CanChange { get { return canChange; }  set { canChange = value; } }

        public Rect ViewPositon { get { return m_position; } set { m_position = value; } }
        [SerializeField]
        private Rect m_position;

        public relation Relation { get {return m_Lation; } set { m_Lation = value; } }
        [SerializeField]
        private relation m_Lation = new relation ();

        [SerializeField]
        private string[] singleAddID = {"GameEnter","Enter"};
        private bool isSingle=false;

        private StateData() { }
        public StateData(string ID)
        {
            this.ID = ID;
            foreach (var item in singleAddID)
            {
                if (item.Equals(iD))
                {
                    isSingle = true;
                    break;
                }
            }
        }

        public void AddData()
        {

        }

        /// <summary>
        /// 获得所有状态脚本
        /// </summary>
        /// <returns></returns>
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

        public void DeleteState()
        {
            string path = StateEditorPathConfig.StateGeneratorPath + "/" + ID + ".cs";
            if (File.Exists(path))
            {               
                File.Delete(path);
            }
        }

        public static void DeleteStateForName(string name)
        {
            string path = StateEditorPathConfig.StateGeneratorPath + "/" + name;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
