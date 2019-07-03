namespace DeviceManager {
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    public class AndroidUtil {
        private static AndroidJavaObject mActivity;
        private static AndroidJavaClass mLoginUtil;
        private static AndroidJavaClass mUtil = new AndroidJavaClass("com.naocy.androidutil.AndroidUtil");

        public static void OpenApp(string packageName) {
            mUtil.CallStatic("openApplication", GetActivity(), packageName);
        }

        public static int GetSystemBrightness() {
            return mUtil.CallStatic<int>("getSystemBrightness", GetActivity());
        }

        public static AndroidJavaObject GetActivity() {
            if (mActivity == null) {
                mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            }
            return mActivity;
        }
        /// <summary>
        /// reboot device, need root device
        /// </summary>
        public static void Reboot() {
            mUtil.CallStatic("Execute", "reboot");
        }

        /// <summary>
        /// shutdown device, need root device
        /// </summary>
        public static void Shutdown() {
            mUtil.CallStatic("Execute", "reboot -p");
        }

        /// <summary>
        /// intput key down
        /// </summary>
        /// <param name="key">HOME:3, BACK:4</param>
        public static void InputKeyEvent(int key) {
            mUtil.CallStatic("Execute", "input keyevent " + key);
        }

        /// <summary>
        /// input touch
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public static void InputTap(int x, int y) {
            mUtil.CallStatic("Execute", "input tap " + x + " " + y);
        }

        public static void LoginQQ(string unityObject, string unityMethod, string qqAppId) {
            GetLoginUtil().CallStatic("QQLogin", unityObject, unityMethod, GetActivity(), qqAppId);
        }

        private static AndroidJavaClass GetLoginUtil() {
            if (mLoginUtil == null) {
                mLoginUtil = new AndroidJavaClass("com.naocy.androidutil.sharesdk.UnityThirdPartyLoginUtil");
            }
            return mLoginUtil;
        }

    }
}