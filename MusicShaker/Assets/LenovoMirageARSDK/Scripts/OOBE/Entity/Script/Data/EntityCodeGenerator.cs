using UnityEngine;
using System.IO;
using System.Text;

namespace LenovoMirageARSDK.OOBE
{
    public static class EntityCodeGenerator
    {
        public static ScriptData CreateCode(string go)
        {
            ScriptData newData = new ScriptData(go);
            if (null != go)
            {
                CreateEntityCode(go);
                GeneratorJson(newData);
            }
            return newData;
        }

        static void CreateEntityCode(string go)
        {
            string strDlg = go;
            string strFilePath = string.Format("{0}/{1}.cs", StateEditorPathConfig.ScriptGeneratorPath, strDlg);

            if (File.Exists(strFilePath) == false)
            {
                StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.AppendLine("namespace QFramework");
                strBuilder.AppendLine("{");
                strBuilder.Append("\t").AppendLine("using System;");
                strBuilder.Append("\t").AppendLine("using System.Collections.Generic;");
                strBuilder.Append("\t").AppendLine("using UnityEngine;");
                strBuilder.Append("\t").AppendLine("using UnityEngine.AI;");
                strBuilder.Append("\t").AppendLine("using UnityEngine; ").AppendLine();

                strBuilder.Append("\t").AppendFormat("public class {0}: Entity", strDlg).AppendLine();
                strBuilder.Append("\t").AppendLine("{");

                strBuilder.Append("\t\t").AppendLine("//--");
                strBuilder.Append("\t\t").AppendLine("//--");

                strBuilder.Append("\t\t").AppendFormat("public {0}() : base()", strDlg).AppendLine();
                strBuilder.Append("\t\t").AppendLine("{");
                strBuilder.AppendLine();
                strBuilder.Append("\t\t").AppendLine("}");

                strBuilder.AppendLine();

                strBuilder.Append("\t\t").AppendFormat("public {0}(object m_Obj) : base(m_Obj)", strDlg).AppendLine();
                strBuilder.Append("\t\t").AppendLine("{");
                strBuilder.AppendLine();
                strBuilder.Append("\t\t").AppendLine("}");

                strBuilder.AppendLine();

                strBuilder.Append("\t\t").AppendLine("protected override void InitState()");
                strBuilder.Append("\t\t").AppendLine("{");
                strBuilder.Append("\t\t\t").AppendLine("//**");
                strBuilder.Append("\t\t").AppendLine("}");

                strBuilder.AppendLine();

                strBuilder.Append("\t\t").AppendLine("protected override void OnDestroy()");
                strBuilder.Append("\t\t").AppendLine("{");
                strBuilder.AppendLine();
                strBuilder.Append("\t\t").AppendLine("}");

                strBuilder.AppendLine();
                strBuilder.Append("\t}").AppendLine();
                strBuilder.Append("}");

                sw.Write(strBuilder);
                sw.Flush();
                sw.Close();

                Debug.Log("Success Create EntityObject Code");
            }
        }

        public static void GeneratorJson(ScriptData m_data)
        {
            string path = Application.dataPath.Replace("Assets", StateEditorPathConfig.ScriptGeneratorPath + "/" + m_data.Name + ".json");
            File.WriteAllText(path, JsonUtility.ToJson(m_data));
        }

        public static ScriptData GetData(string name)
        {
            name = name.Replace(".cs", "");
            string path = Application.dataPath.Replace("Assets", StateEditorPathConfig.ScriptGeneratorPath + "/" + name + ".json");

            if (!File.Exists(path))
                return new LenovoMirageARSDK.OOBE.ScriptData(name);

            return JsonUtility.FromJson<ScriptData>(File.ReadAllText(path));
        }
    }
}