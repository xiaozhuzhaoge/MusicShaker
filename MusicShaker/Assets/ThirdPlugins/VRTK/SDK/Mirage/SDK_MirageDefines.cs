// Mirage Defines|SDK_Mirage|001

namespace VRTK
{
    /// <summary>
    /// Handles all the scripting define symbols for the Mirage SDK.
    /// </summary>
    public static class SDK_MirageDefines
    {
        /// <summary>
        /// The scripting define symbol for the Mirage SDK.
        /// </summary>
        public const string ScriptingDefineSymbol = SDK_ScriptingDefineSymbolPredicateAttribute.RemovableSymbolPrefix + "SDK_MIRAGE";

#if UNITY_IOS
        [SDK_ScriptingDefineSymbolPredicate(ScriptingDefineSymbol, "iOS")]
#else        
        [SDK_ScriptingDefineSymbolPredicate(ScriptingDefineSymbol, "Android")]
#endif
        private static bool IsMirageAvailable()
        {
            //@EDIT:启用VRTK_MirageSDK
            return VRTK_SharedMethods.GetTypeUnknownAssembly("Ximmerse.InputSystem.XDevicePlugin") != null;
        }
    }
}