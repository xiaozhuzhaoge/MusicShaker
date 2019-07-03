using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace LenovoMirageARSDK.Editor
{
    /// <summary>
    /// Listene the Build Target Change and Before Build 
    /// </summary>
    public class MirageAR_PreferenceBuildSettingListener : UnityEditor.Build.IActiveBuildTargetChanged, UnityEditor.Build.IPreprocessBuild
    {
        #region Interface Implement

        /// <summary>
        /// Interface Fuc:execute on BuildTarget Changed
        /// </summary>
        /// <param name="previousTarget"></param>
        /// <param name="newTarget"></param>
        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            //Debug.Log("OnActiveBuildTargetChanged");
            Debug.Log(string.Format("Switched build target from {0} to {1}", previousTarget, newTarget));

            if (!MirageAR_PreferenceBuildSetting.CheckBuildSettings())
            {
                MirageAR_PreferenceBuildSetting.Enter();
            }
        }

        public int callbackOrder { get { return 0; } }

        /// <summary>
        /// Interface Fuc:execute before Build
        /// </summary>
        /// <param name="target"></param>
        /// <param name="path"></param>
        public void OnPreprocessBuild(BuildTarget target, string path)
        {

            if (!MirageAR_PreferenceBuildSetting.CheckBuildSettings())
            {
                MirageAR_PreferenceBuildSetting.Enter();

                //throw new System.Exception("Your BuildSetting is Not Correct,Please Fix First!");
                throw new UnityEditor.BuildPlayerWindow.BuildMethodException("Your BuildSetting is Not Correct,Please Fix First!");              
            }
        }

        #endregion Interface Implement
    }  
}


