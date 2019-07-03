namespace DeviceManager
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class IOSUtil : MonoBehaviour
    {
        [DllImport("__Internal")]
        public static extern void openURL(string scheme);

        [DllImport("__Internal")]
        public static extern void setBrightness(float brightness);

        [DllImport("__Internal")]
        public static extern float getBrightness();

        [DllImport("__Internal")]
        public static extern void setVolume(float volume);

        [DllImport("__Internal")]
        public static extern float getVolume();

        [DllImport("__Internal")]
        public static extern string getDeviceID();

        public delegate void DialogDelegate(int buttonIndex);
        private static Action onConfirmClick;
        private static Action onCancelClick;
        public static void ShowDialog(string title, string message, string confirm, Action OnConfirm, string cancel, Action OnCancel)
        {
            _showDialog(title, message, confirm, cancel, AlertViewCallback);
            onConfirmClick = OnConfirm;
            onCancelClick = OnCancel;
        }

        [DllImport("__Internal")]
        public static extern void _showDialog(string title, string message, string confirm, string cancel, DialogDelegate callback);


        [AOT.MonoPInvokeCallback(typeof(DialogDelegate))]
        static void AlertViewCallback(int buttonIndex)
        {
            Debug.Log("AlertViewCallback " + buttonIndex);
            if (buttonIndex == 0 && onCancelClick != null)
            {
                onCancelClick();
            }
            else if (buttonIndex == 1 && onConfirmClick != null)
            {
                onConfirmClick();
            }
        }
    }
}