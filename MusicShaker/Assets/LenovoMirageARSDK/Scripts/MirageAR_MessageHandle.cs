using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ximmerse.InputSystem;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// To Receive Android/iOS Sended Message
    /// </summary>
    public class MirageAR_MessageHandle : MonoBehaviour
    {

#if UNITY_ANDROID

#elif UNITY_IOS
        public void OnOpenWithUrl(string url)
        {
            Debug.Log("OnOpenWithUrl:"+url);

            //Set the IsClient Value
            XDevicePlugin.IsClient = true;
        }
#endif
    }
}



