using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeviceManager
{
    /// <summary>
    /// Manages Android permissions for the Unity application.
    /// </summary>
    public class AndroidPermissionsManager : AndroidJavaProxy
    {
        private static AndroidPermissionsManager s_Instance;
        private static AndroidJavaObject s_Activity;
        private static AndroidJavaObject s_PermissionService;
        private static AsyncTask<AndroidPermissionsRequestResult> s_CurrentRequest = null;
        private static Action<AndroidPermissionsRequestResult> s_OnPermissionsRequestFinished;
        private static List<string> s_PermissionNames=new List<string>();
        private static List<bool> s_GrantResults=new List<bool>();

        /// @cond EXCLUDE_FROM_DOXYGEN
        /// <summary>
        /// Constructs a new AndroidPermissionsManager.
        /// </summary>
        public AndroidPermissionsManager() : base("com.naocy.androidutil.UnityAndroidPermissions$IPermissionRequestResult")
        {
        }

        /// @endcond

        /// <summary>
        /// Checks if an Android permission is granted to the application.
        /// </summary>
        /// <param name="permissionName">The full name of the Android permission to check (e.g.
        /// android.permission.CAMERA).</param>
        /// <returns><c>true</c> if <c>permissionName</c> is granted to the application, otherwise
        /// <c>false</c>.</returns>
        public static bool IsPermissionGranted(string permissionName)
        {
            if (Application.isEditor)
            {
                return true;
            }

            return GetPermissionsService().Call<bool>("IsPermissionGranted", GetUnityActivity(), permissionName);
        }

        public static AsyncTask<AndroidPermissionsRequestResult> RequestPermission(string permissionName)
        {
            //Init the Permission Result
            InitPermissionResult();

            if (AndroidPermissionsManager.IsPermissionGranted(permissionName))
            {
                return new AsyncTask<AndroidPermissionsRequestResult>(new AndroidPermissionsRequestResult(
                    new string[] { permissionName }, new bool[] { true }));
            }

            if (s_CurrentRequest != null)
            {
                Debug.LogError("Attempted to make simultaneous Android permissions requests.");
                return null;
            }

            GetPermissionsService().Call("RequestPermissionAsync", GetUnityActivity(),
                new[] { permissionName }, GetInstance());
            s_CurrentRequest = new AsyncTask<AndroidPermissionsRequestResult>(out s_OnPermissionsRequestFinished);

            return s_CurrentRequest;

        }

        /// <summary>
        /// Requests an Android permission from the user.
        /// </summary>
        /// <param name="permissionName">The permission to be requested (e.g. android.permission.CAMERA).</param>
        /// <returns>An asynchronous task the completes when the user has accepted/rejected the requested permission
        /// and yields a {@link AndroidPermissionsRequestResult} that summarizes the result.  If this method is called
        /// when another permissions request is pending <c>null</c> will be returned instead.</returns>
        public static AsyncTask<AndroidPermissionsRequestResult> RequestPermissions(string[] permissionNames)
        {
            //Init the Permission Result
            InitPermissionResult();

            if (s_CurrentRequest != null)
            {
                Debug.LogError("Attempted to make simultaneous Android permissions requests.");
                return null;
            }

            GetPermissionsService().Call("RequestPermissionAsync", GetUnityActivity(),
                permissionNames, GetInstance());
            s_CurrentRequest = new AsyncTask<AndroidPermissionsRequestResult>(out s_OnPermissionsRequestFinished);

            return s_CurrentRequest;
        }

        public static bool IsRefuseShowRequestDialog(string permission)
        {
            return !GetUnityActivity().Call<bool>("shouldShowRequestPermissionRationale", permission);
        }

        /// @cond EXCLUDE_FROM_DOXYGEN
        /// <summary>
        /// Callback fired when a permission is granted.
        /// </summary>
        /// <param name="permissionName">The name of the permission that was granted.</param>
        public virtual void OnPermissionGranted(string permissionName)
        {
            //Debug.Log("@OnPermissionGranted="+ permissionName);

            s_PermissionNames.Add(permissionName);
            s_GrantResults.Add(true);
        }

        /// @endcond

        /// @cond EXCLUDE_FROM_DOXYGEN
        /// <summary>
        /// Callback fired when a permission is denied.
        /// </summary>
        /// <param name="permissionName">The name of the permission that was denied.</param>
        public virtual void OnPermissionDenied(string permissionName)
        {
            //Debug.Log("@OnPermissionDenied="+ permissionName);

            s_PermissionNames.Add(permissionName);
            s_GrantResults.Add(false);
        }

        /// @endcond

        /// @cond EXCLUDE_FROM_DOXYGEN
        /// <summary>
        /// Callback fired when permission is complete
        /// </summary>
        public virtual void OnPermissionComplete()
        {
            _OnPermissionResult();
        }

        /// @endcond

        /// @cond EXCLUDE_FROM_DOXYGEN
        /// <summary>
        /// Callback fired on an Android activity result (unused part of UnityAndroidPermissions interface).
        /// </summary>
        public virtual void OnActivityResult()
        {
        }

        private static AndroidPermissionsManager GetInstance()
        {
            if (s_Instance == null)
            {
                s_Instance = new AndroidPermissionsManager();
            }

            return s_Instance;
        }

        private static AndroidJavaObject GetUnityActivity()
        {
            if (s_Activity == null)
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                s_Activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }

            return s_Activity;
        }

        private static AndroidJavaObject GetPermissionsService()
        {
            if (s_PermissionService == null)
            {
                s_PermissionService = new AndroidJavaObject("com.naocy.androidutil.UnityAndroidPermissions");
            }

            return s_PermissionService;
        }

        /// @endcond

        /// <summary>
        /// Callback fired on an Android permission result.
        /// </summary>
        private void _OnPermissionResult()
        {
            if (s_OnPermissionsRequestFinished == null)
            {
                Debug.LogErrorFormat("AndroidPermissionsManager received an unexpected permissions result");
                return;
            }

            // Cache completion method and reset request state.
            var onRequestFinished = s_OnPermissionsRequestFinished;
            s_CurrentRequest = null;
            s_OnPermissionsRequestFinished = null;

            onRequestFinished(new AndroidPermissionsRequestResult(s_PermissionNames.ToArray(),
                s_GrantResults.ToArray()));

            //Init the Permission Result
            InitPermissionResult();
        }

        /// <summary>
        /// Init the Permission Result
        /// </summary>
        private static void InitPermissionResult()
        {
            s_PermissionNames.Clear();
            s_GrantResults.Clear(); ;
        }
    }
}
