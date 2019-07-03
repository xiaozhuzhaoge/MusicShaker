namespace DeviceManager
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AndroidWindowManager
    {
        private static AndroidJavaObject s_window = AndroidUtil.GetActivity().Call<AndroidJavaObject>("getWindow");
        /** 
         * Set screen brightness
         * 
         * @param paramFloat 0-1.0f 
         */
        public static void SetActivityBrightness(float paramFloat)
        {
            //AndroidUtil.GetActivity().Call("runOnUiThread", new SetBrightnessRunnable(paramFloat));
            AndroidUtil.GetActivity().Call("runOnUiThread", new AndroidRunnable(delegate ()
            {
                AndroidJavaObject layoutParams = GetWindow().Call<AndroidJavaObject>("getAttributes");
                layoutParams.Set("screenBrightness", paramFloat);
                GetWindow().Call("setAttributes", layoutParams);
            }));
        }

        /** 
        * Get screen brightness
        * 
        * getBrightnessAction param float 0-1.0f 
        */
        public static void GetActivityBrightness(Action<float> getBrightnessAction)
        {
            AndroidUtil.GetActivity().Call("runOnUiThread", new GetBrightnessRunnable(getBrightnessAction));
        }

        public static float GetActivityBrightness()
        {
            AndroidJavaObject layoutParams = GetWindow().Call<AndroidJavaObject>("getAttributes");
            return layoutParams.Get<float>("screenBrightness");
        }

        public class SetBrightnessRunnable : AndroidJavaProxy, Runnable
        {
            float _paramFloat;
            public SetBrightnessRunnable(float paramFloat) : base("java.lang.Runnable")
            {
                Debug.Log("SetBrightnessRunnable " + paramFloat);
                _paramFloat = paramFloat;
            }

            public void run()
            {
                AndroidJavaObject layoutParams = GetWindow().Call<AndroidJavaObject>("getAttributes");
                layoutParams.Set("screenBrightness", _paramFloat);
                GetWindow().Call("setAttributes", layoutParams);
            }
        }
        public class GetBrightnessRunnable : AndroidJavaProxy, Runnable
        {
            Action<float> _getBrightnessAction;
            public GetBrightnessRunnable(Action<float> getBrightnessAction) : base("java.lang.Runnable")
            {
                _getBrightnessAction = getBrightnessAction;
            }

            public void run()
            {
                AndroidJavaObject layoutParams = GetWindow().Call<AndroidJavaObject>("getAttributes");
                _getBrightnessAction(layoutParams.Get<float>("screenBrightness"));
            }
        }
        private static AndroidJavaObject GetWindow()
        {
            if (s_window == null)
            {
                s_window = AndroidUtil.GetActivity().Call<AndroidJavaObject>("getWindow");
            }
            return s_window;
        }
    }
}