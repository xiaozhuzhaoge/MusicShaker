// Mirage System|SDK_Mirage|002
namespace VRTK
{
    /// <summary>
    /// The Mirage System SDK script provides dummy functions for system functions.
    /// </summary>    
#if UNITY_IOS //@EDIT:适配VR模式为Null，适配平台为Android，iOS
    [SDK_Description("Mirage", SDK_MirageDefines.ScriptingDefineSymbol, null, "iOS")]
#else
    [SDK_Description("Mirage", SDK_MirageDefines.ScriptingDefineSymbol, null, "Android")]
#endif
    public class SDK_MirageSystem
#if VRTK_DEFINE_SDK_MIRAGE
        : SDK_BaseSystem
#else
        : SDK_FallbackSystem
#endif
    {
#if VRTK_DEFINE_SDK_MIRAGE
        /// <summary>
        /// The IsDisplayOnDesktop method returns true if the display is extending the desktop.
        /// </summary>
        /// <returns>Returns true if the display is extending the desktop</returns>
        public override bool IsDisplayOnDesktop()
        {
            return false;
        }

        /// <summary>
        /// The ShouldAppRenderWithLowResources method is used to determine if the Unity app should use low resource mode. Typically true when the dashboard is showing.
        /// </summary>
        /// <returns>Returns true if the Unity app should render with low resources.</returns>
        public override bool ShouldAppRenderWithLowResources()
        {
            return false;
        }

        /// <summary>
        /// The ForceInterleavedReprojectionOn method determines whether Interleaved Reprojection should be forced on or off.
        /// </summary>
        /// <param name="force">If true then Interleaved Reprojection will be forced on, if false it will not be forced on.</param>
        public override void ForceInterleavedReprojectionOn(bool force)
        {
        }
#endif
    }
}