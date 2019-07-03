using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
public class MyBuildPostprocessor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        #if UNITY_IOS
        if (target != BuildTarget.iOS) return;
        // 初始化
        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromString(File.ReadAllText(projPath));
        string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

        // 添加framework
        pbxProject.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
        File.WriteAllText(projPath, pbxProject.WriteToString());
        #endif
    }
}
