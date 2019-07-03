using DeviceManager;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// Manager for Android to Ask Permission
    /// </summary>
    public class MirageAR_AndroidPermissionManager
    {
        #region Public Static Properties

        /// <summary>
        /// The Permission MirageAR Needed
        /// </summary>
        public static string[] MirageARPermissions
        {
            get
            {
                return mirageARPermissions;
            }          
        }

        #endregion

        #region Private Properties
        /// <summary>
        /// The Permission MirageAR Needed
        /// </summary>
        private static string[] mirageARPermissions = {
        "android.permission.READ_EXTERNAL_STORAGE",
        "android.permission.WRITE_EXTERNAL_STORAGE",
        "android.permission.ACCESS_FINE_LOCATION",
        "android.permission.ACCESS_COARSE_LOCATION"
        };
        #endregion

        #region Public Method
        /// <summary>
        /// Request MirageAR Permissions
        /// </summary>
        public static void RequestMirageARPermissions()
        {
            DeviceUtils.RequestPermissions(MirageARPermissions);
        }

        #endregion


    }
}


