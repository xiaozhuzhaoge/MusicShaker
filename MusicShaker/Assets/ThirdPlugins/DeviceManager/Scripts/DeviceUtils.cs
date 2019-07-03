namespace DeviceManager
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DeviceManager;
    using System;

    public class DeviceUtils
    {
        /// <summary>
        /// Is App Installed
        /// </summary>
        /// <returns></returns>
        public static bool IsApplicationInstalled(string name)
        {
            //TODO 
            return false;
        }

        /// <summary>
        /// Open Other App
        /// </summary>
        /// <param name="name">Target App Name</param>
        public static void OpenApplication(string name)
        {
#if UNITY_ANDROID&&!UNITY_EDITOR
            AndroidUtil.OpenApp(name);
#elif UNITY_IOS&&!UNITY_EDITOR
#endif
        }

        #region Android Permission

        /// <summary>
        /// Check The Permission Is Or Not Granted
        /// </summary>
        /// <param name="permission">The Target Permission</param>
        /// <returns></returns>
        public static bool CheckPermission(string permission)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return AndroidPermissionsManager.IsPermissionGranted(permission);
#else
            return false;
#endif
        }

        /// <summary>
        /// Request Target Permission
        /// </summary>
        /// <param name="permission">The Target Permission</param>
        /// <returns></returns>
        public static AsyncTask<AndroidPermissionsRequestResult> RequestPermission(string permission)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return AndroidPermissionsManager.RequestPermission(permission);
#else
            return null;
#endif            
        }

        /// <summary>
        /// Request Target Permissions
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static AsyncTask<AndroidPermissionsRequestResult> RequestPermissions(string[] permissions)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return AndroidPermissionsManager.RequestPermissions(permissions);
#else
            return null;
#endif            
        }

        /// <summary>
        /// Check Is Refause Show Request Dialog(Click Don't Show Again)
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static bool IsRefuseShowRequestDialog(string permission)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return AndroidPermissionsManager.IsRefuseShowRequestDialog(permission);
#else
            return false;
#endif            
        }

        #endregion  //Android Permission        

        /// Set screen brightness
        /// brightness: 0-1f
        public static void SetBrightness(float brightness)
        {
#if UNITY_ANDROID
            AndroidWindowManager.SetActivityBrightness(brightness);
#elif UNITY_IOS
            IOSUtil.setBrightness(brightness);
#endif
        }

        /// Get screen brightness, value is not correct after call SetBrightness immediately in Android
        /// return: 0.0-1.0f
        public static float GetBrightness()
        {
#if UNITY_ANDROID
            return AndroidWindowManager.GetActivityBrightness();
#elif UNITY_IOS
            return IOSUtil.getBrightness();
#else
            return -1;
#endif
        }
        /// Get screen brightness, value is correct after call SetBrightness immediately in Android
        /// return: 0.0-1.0f
        public static void GetBrightnessThreadSafty(Action<float> getBrightnessAction)
        {
#if UNITY_ANDROID
            AndroidWindowManager.GetActivityBrightness(getBrightnessAction);
#elif UNITY_IOS
            getBrightnessAction(IOSUtil.getBrightness());
#endif
        }

        public static void SetVolume(float volume)
        {
#if UNITY_ANDROID
            AndroidAudioManager.SetStreamVolume(AndroidAudioManager.STREAM_MUSIC, (int)(15 * volume + 0.01), 0);
#elif UNITY_IOS
            IOSUtil.setVolume(volume);
#endif
        }

        public static float GetVolume()
        {
#if UNITY_ANDROID
            return (float)AndroidAudioManager.GetStreamVolume(AndroidAudioManager.STREAM_MUSIC) / 15;
#elif UNITY_IOS
            return IOSUtil.getVolume();
#else
            return -1;
#endif
        }

        public static string GetDeviceID()
        {
#if UNITY_ANDROID
            return new AndroidJavaClass("android.os.Build").GetStatic<string>("SERIAL");
#elif UNITY_IOS
            return IOSUtil.getDeviceID();
#else
            return "";
#endif
        }
        /// <summary>
        /// login qq
        /// </summary>
        /// <param name="unityObject">unity object name which accept login message </param>
        /// <param name="unityMethod">unity object name which accept login message</param>
        /// <param name="qqAppId">QQ app id, 需要同时设置AndroidManifest： data android:scheme="tencent1106887012"</param>
        public static void LoginQQ(string unityObject, string unityMethod, string qqAppId)
        {
            AndroidUtil.LoginQQ(unityObject, unityMethod, qqAppId);
        }

        public static void ShowDialog(string title, string message, string confirm, Action OnConfirm, string cancel, Action OnCancel)
        {
#if UNITY_ANDROID
            new AndroidDialogBuilder()
                .SetTitle(title)
                .SetMessage(message)
                .SetPositiveButton(confirm, OnConfirm)
                .SetNegativeButton(cancel, OnCancel)
                .Create().Show();
#else
            IOSUtil.ShowDialog(title, message, confirm, OnConfirm, cancel, OnCancel);
#endif
        }
    }
}
