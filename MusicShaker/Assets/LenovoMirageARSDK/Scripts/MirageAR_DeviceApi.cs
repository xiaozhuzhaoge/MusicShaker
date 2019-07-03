using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// Mirage Device API:
    /// </summary>
    public class MirageAR_DeviceApi
    {
#if UNITY_ANDROID

        private static AndroidJavaObject s_Activity;
        private static AndroidJavaClass s_MirageApi = new AndroidJavaClass("com.naocy.mirageapi.MirageApi");

        private static AndroidJavaObject GetActivity()
        {
            if (s_Activity == null)
            {
                s_Activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            }
            return s_Activity;
        }

        // com.naocy.mirageapi.MirageListener
        public interface MirageListener
        {
            void onServiceConnected(int hwInitReturn);
            void onServiceDisconnected();
        }

        public static int Init(int deviceVersion, int launchMode, MirageListener listener)
        {
            Debug.Log("MirageApi Init");
            Debug.Log("CallStatic setListener");
            s_MirageApi.CallStatic("setListener", listener);
            Debug.Log("CallStatic init");
            return s_MirageApi.CallStatic<int>("init", GetActivity(), deviceVersion, launchMode);
        }

        public static int Exit()
        {
            return s_MirageApi.CallStatic<int>("exit");
        }

        public static int OnResume()
        {
            return s_MirageApi.CallStatic<int>("onResume");
        }

        public static int OnPause()
        {
            return s_MirageApi.CallStatic<int>("onPause");
        }

        public static int GetInputDeviceHandle(string deviceName)
        {
            return s_MirageApi.CallStatic<int>("getInputDeviceHandle", deviceName);
        }

        public static void SetTrackerPose(int which, float height, float depth, float pitch)
        {
            s_MirageApi.CallStatic<int>("setTrackerPose", which, height, depth, pitch);
        }

        /* 
			public static int GetInputState(int which, ControllerState state)
			{

			}
			public static int getInputState(int which, TrackerState state)
			{
				return MirageClient.instance.getInputState(which, state);
			}
*/

        public static int SendMessage(int which, int Msg, int wParam, int lParam)
        {
            return s_MirageApi.CallStatic<int>("sendMessage", which, Msg, wParam, lParam);
        }

        public static int sendRecenterMessage(int which, float wParam, int lParam)
        {
            return s_MirageApi.CallStatic<int>("sendRecenterMessage", which, wParam, lParam);
        }

        public static int UpdateInputState(int which)
        {
            return s_MirageApi.CallStatic<int>("updateInputState", which);
        }

        public static bool GetBool(int which, int fieldID, bool defaultValue)
        {
            return s_MirageApi.CallStatic<bool>("getBool", which, fieldID, defaultValue);
        }

        public static void SetBool(int which, int fieldID, bool value)
        {
            s_MirageApi.CallStatic<bool>("setBool", which, fieldID, value);
        }

        public static int GetInt(int which, int fieldID, int defaultValue)
        {
            return s_MirageApi.CallStatic<int>("getInt", which, fieldID, defaultValue);
        }

        public static void SetInt(int which, int fieldID, int value)
        {
            s_MirageApi.CallStatic("setInt", which, fieldID, value);
        }

        public static float GetFloat(int which, int fieldID, float defaultValue)
        {
            return s_MirageApi.CallStatic<float>("getFloat", which, fieldID, defaultValue);
        }

        public static void SetFloat(int which, int fieldID, float value)
        {
            s_MirageApi.CallStatic("setFloat", which, fieldID, value);
        }

        public static string GetString(int which, int fieldID, string defaultValue)
        {
            return s_MirageApi.CallStatic<string>("getString", which, fieldID, defaultValue);
        }

        public static void SetString(int which, int fieldID, string value)
        {
            s_MirageApi.CallStatic<string>("setString", which, fieldID, value);
        }

        public static int GetNodePosition(int which, int history, int node, Vector3 vector3)
        {
            //return s_MirageApi
            return -1;
        }
        /*
                public static int GetAllState(ControllerState controller1, Vector3 node3Vector, Vector3 node5Vector)
                {
                    //return s_MirageApi
                    return -1;
                }
         */
        public static int GetControllerCount()
        {
            return s_MirageApi.CallStatic<int>("getControllerCount");
        }

        public static void SetControllerCount(int count)
        {
            s_MirageApi.CallStatic("setControllerCount", count);
        }

        public static void SetControllerPairingMode(bool enable)
        {
            s_MirageApi.CallStatic("setControllerPairingMode", enable);
        }

        private static bool IsClient()
        {
            return s_MirageApi.CallStatic<bool>("isClient");
        }

        public static void SaveBrightness(float brightness)
        {
            s_MirageApi.CallStatic("saveBrightness", brightness);
        }

        public static float GetBrightness()
        {
            return s_MirageApi.CallStatic<float>("getBrightness");
        }

        public static void PutServiceBool(string key, bool value)
        {
            s_MirageApi.CallStatic("putServiceBool", key, value);
        }

        public static bool GetServiceBool(string key, bool defaule)
        {
            return s_MirageApi.CallStatic<bool>("getServiceBool", key, defaule);
        }
        public static void PutServiceInt(string key, int value)
        {
            s_MirageApi.CallStatic("putServiceInt", key, value);
        }

        public static int GetServiceInt(string key, int defaule)
        {
            return s_MirageApi.CallStatic<int>("getServiceInt", key, defaule);
        }

        public static void PutServiceFloat(string key, float value)
        {
            s_MirageApi.CallStatic("putServiceFloat", key, value);
        }

        public static float GetServiceFloat(string key, float defaule)
        {
            return s_MirageApi.CallStatic<float>("getServiceFloat", key, defaule);
        }

        public static void PutServiceString(string key, string value)
        {
            s_MirageApi.CallStatic("putServiceString", key, value);
        }

        public static string GetServiceString(string key)
        {
            return s_MirageApi.CallStatic<string>("getServiceString", key);
        }
        /// <summary>
        /// check AR Head usb permission in Service
        /// </summary>
        /// <returns>-1: call fail; 0: AR Head not find; 1: no permission; 2: has permission </returns>
        public static int CheckARHeadPermission()
        {
            return s_MirageApi.CallStatic<int>("checkARHeadPermission");
        }
        /// <summary>
        /// request AR Head usb permission in Service
        /// </summary>
        public static void RequestARHeadPermission()
        {
            s_MirageApi.CallStatic("requestARHeadPermission");
        }
        /// <summary>
        /// check bluetooth(location) permission to connect AR Controller in Service
        /// </summary>
        /// <returns>-1: call fail; 0: no permission; 1: has permission</returns>
        public static int CheckARControllerPermission()
        {
            return s_MirageApi.CallStatic<int>("checkARControllerPermission");
        }

#endif  //UNITY_ANDROID
    }

#if UNITY_ANDROID

    #region MirageInitListener

    /// <summary>
    /// Mirage Init Listener
    /// </summary>
    public class MirageInitListener : AndroidJavaProxy, MirageAR_DeviceApi.MirageListener
    {
        public MirageInitListener() : base("com.naocy.mirageapi.MirageListener") { }
        public void onServiceConnected(int hwInitReturn)
        {
            Debug.Log("onServiceConnected");
            MirageAR_ServiceHelper.MirageARServiceConnected();
        }
        public void onServiceDisconnected()
        {
            Debug.Log("onServiceDisconnected");
            MirageAR_ServiceHelper.MirageServiceDisconnected();
        }
    }

    #endregion //MirageInitListener

#endif  //UNITY_ANDROID

}

