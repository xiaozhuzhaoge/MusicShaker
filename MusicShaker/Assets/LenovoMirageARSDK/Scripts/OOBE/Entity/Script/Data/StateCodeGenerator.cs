using System.IO;
using System.Text;
using UnityEngine;

namespace LenovoMirageARSDK.OOBE
{
    public static class StateCodeGenerator
    {
        public static void CreateCode(string name)
        {
            Generator(name);
        }

        private static void Generator(string go)
        {
            string strDlg = go;
            string strFilePath = string.Format("{0}/{1}.cs", StateEditorPathConfig.StateGeneratorPath, strDlg);

            if (!File.Exists(strFilePath))
            {
                StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.AppendLine("namespace QFramework");
                strBuilder.AppendLine("{");
                strBuilder.Append("\t").AppendLine("using UnityEngine;").AppendLine();
                strBuilder.Append("\t").AppendFormat("public class {0} : State", strDlg).AppendLine();
                strBuilder.Append("\t").AppendLine("{");
                strBuilder.Append("\t\t").AppendFormat("public {0}(IEntity parentEntity):base(parentEntity)", strDlg).AppendLine();
                strBuilder.Append("\t\t").AppendLine("{");
                strBuilder.Append("\t\t\t").AppendFormat("stateID = {0};",'"'+strDlg+'"').AppendLine();
                strBuilder.Append("\t\t").AppendLine("}");

                strBuilder.AppendLine();

                strBuilder.Append("\t\t").AppendFormat("public {0}() : base()", strDlg).AppendLine();
                strBuilder.Append("\t\t").AppendLine("{");
                strBuilder.Append("\t\t\t").AppendFormat("stateID = {0};", '"' + strDlg + '"').AppendLine();
                strBuilder.Append("\t\t").AppendLine("}");

                strBuilder.AppendLine();

                strBuilder.Append("\t\t").AppendLine("public override void EnterState()");
                strBuilder.Append("\t\t").AppendLine("{");
                strBuilder.AppendLine();
                strBuilder.Append("\t\t").AppendLine("}");

                strBuilder.AppendLine();

                strBuilder.Append("\t\t").AppendFormat("public override void ExitState()", strDlg).AppendLine();
                strBuilder.Append("\t\t").AppendLine("{");
                strBuilder.AppendLine();
                strBuilder.Append("\t\t").AppendLine("}");

                strBuilder.AppendLine();

                strBuilder.Append("\t\t").AppendLine("public override void update()");
                strBuilder.Append("\t\t").AppendLine("{");
                strBuilder.AppendLine();
                strBuilder.Append("\t\t").AppendLine("}");

                strBuilder.AppendLine();
                strBuilder.Append("\t}").AppendLine();
                strBuilder.Append("}");

                sw.Write(strBuilder);
                sw.Flush();
                sw.Close();

                Debug.Log("Success Create State Code");
            }
        }

        public static void insertcoding(ScriptData CurrentScript)
        {
            string path = string.Format("{0}/{1}.cs", StateEditorPathConfig.ScriptAbsolutePath, CurrentScript.Name);

            if (!File.Exists(path))
            {
                return;
            }

            string ScriptContent = File.ReadAllText(path);

            string strFilePath = string.Format("{0}/{1}.cs", StateEditorPathConfig.ScriptGeneratorPath, CurrentScript.Name); ;

            StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
            StringBuilder strBuilder = new StringBuilder();

            string keySubStr = "//--";
            int keyBeginIndex = ScriptContent.IndexOf(keySubStr);
            int KeyEndIndex = ScriptContent.LastIndexOf(keySubStr);

            if (keyBeginIndex != -1 && KeyEndIndex != -1)
            {
                string halfone = ScriptContent.Substring(0, keyBeginIndex + 4);
                string halftwo = ScriptContent.Substring(KeyEndIndex, ScriptContent.Length - KeyEndIndex);

                ScriptContent = halfone + halftwo;
            }

            StringBuilder KeystrBuilder = new StringBuilder();

            KeystrBuilder.AppendFormat("\t\t\t").AppendLine();
            KeystrBuilder.Append("\t\t").AppendLine("public enum Transtion");
            KeystrBuilder.Append("\t\t").AppendLine("{");

            for (int i = 0; i < CurrentScript.m_transition.Count; i++)
            {
                KeystrBuilder.Append("\t\t\t").AppendLine(CurrentScript.m_transition[i] + ",");
            }

            KeystrBuilder.Append("\t\t").Append("}");

            if (keyBeginIndex != -1)
                ScriptContent = ScriptContent.Insert(keyBeginIndex + 4, KeystrBuilder.ToString());

            string substr = "//**";
            int BeginIndex = ScriptContent.IndexOf(substr);
            int index = ScriptContent.LastIndexOf(substr);

            if (BeginIndex != -1 && index != -1)
            {
                string halfone = ScriptContent.Substring(0, BeginIndex);
                int spaceIndex = halfone.LastIndexOf("{");
                halfone = halfone.Substring(0, spaceIndex + 1);
                string halftwo = ScriptContent.Substring(index, ScriptContent.Length - index);
                ScriptContent = halfone + halftwo;
            }

            index = ScriptContent.LastIndexOf(substr);

            foreach (var item in CurrentScript.m_AllState)
            {
                string name = item.ID;
                strBuilder.AppendFormat("\t\t\t").AppendLine();
                strBuilder.Append("\t\t\t").AppendLine("//**");
                strBuilder.Append("\t\t\t").AppendLine(name + " m_" + name + "= new " + name + "(this);");
                foreach (var nextdata in item.Relation.m_NextData)
                {
                    strBuilder.Append("\t\t\t").AppendFormat("m_{0}.AddTransition({1},{2});", name, '"' + nextdata.key + '"', '"' + nextdata.data.ID + '"').AppendLine();
                }
                strBuilder.Append("\t\t\t").AppendFormat("AddState({0});", "m_" + name);
                strBuilder.AppendLine().Append("\t\t\t");
            }

            string newString = ScriptContent.Insert(index, strBuilder.ToString());

            sw.Write(newString);

            sw.Flush();
            sw.Close();
        }
    }
}
