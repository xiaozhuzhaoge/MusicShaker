
namespace LenovoMirageARSDK
{
    /// <summary>
    /// The Device Constants Info
    /// </summary>
    public class MirageAR_DeviceConstants
    {
        #region Conttroller Model Name
        public const string CONTROLLER_UNIVERSAL_MODELNAME = "Mirage-2B";
        public const string CONTROLLER_SABER_MODELNAME = "DIS_SAB_REY_01";
        #endregion

        #region Peripheral Name
        public const string PERIPHERAL_HMD_NAME = "XHawk-0";
        public const string PERIPHERAL_CONTROLLER_NAME = "XCobra-0";
        #endregion



        /// <summary>
        /// The type of Controller
        /// </summary>
        public enum ControllerType
        {
            /// <summary>
            /// Null controller.
            /// </summary>
            None = 0,

            /// <summary>
            /// The Universal Controller.
            /// </summary>
            UniversalController,

            /// <summary>
            /// The Saber Controller.
            /// </summary>
            SaberController,
        }

    }
}
