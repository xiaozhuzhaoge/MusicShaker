using System;
using System.Collections;
using System.Collections.Generic;
using DeviceManager;
using UnityEngine;
public class Demo : MonoBehaviour {

    // Use this for initialization
    void Start() {
        // get device id
        Debug.Log("Get device id: " + DeviceUtils.GetDeviceID());

        //TestPermission();
        //TestSetting();

        //TestQQLogin();
        //AndroidUtil.Reboot();
        //AndroidUtil.Shutdown();
        //TestAndroidThread();

        //DeviceUtils.ShowDialog("退出应用", "是否退出应用", "确定", delegate { Application.Quit(); }, "取消", null);
    }

    private void TestAndroidThread() {
        int i = 200;
        AndroidThread.RunOnNewThread(delegate {
            while (i > 0) {
                AndroidThread.Sleep(500);
                Debug.Log("AndroidUtil.InputTap(1500, 1000) " + i--);
                AndroidUtil.InputTap(1500, 1000);
            }
        });
    }

    private void TestSetting() {
        // brightness test
        Debug.Log("GetBrightness: " + DeviceUtils.GetBrightness());
        Action<float> GetBrightnessAction = delegate(float screenBrightness) { Debug.Log("GetBrightnessThreadSafty " + screenBrightness); };
        DeviceUtils.GetBrightnessThreadSafty(GetBrightnessAction);
        DeviceUtils.SetBrightness(0.9f);
        DeviceUtils.GetBrightnessThreadSafty(GetBrightnessAction);

        // volume test
        DeviceUtils.SetVolume(1.0f);
        Debug.Log("getVolume: " + DeviceUtils.GetVolume());
        DeviceUtils.SetVolume(0.3f);
        Debug.Log("SetVolume 0.3");
        Debug.Log("getVolume: " + DeviceUtils.GetVolume());
    }

    private void TestPermission() {
        string permission = "android.permission.ACCESS_COARSE_LOCATION";
        Debug.Log("IsPermissionGranted: " + AndroidPermissionsManager.IsPermissionGranted(permission));
        AndroidPermissionsManager.RequestPermission(permission).ThenAction((result) => {
            Debug.Log("ThenAction " + result.IsAllGranted);
            if (result.IsAllGranted) {
                Debug.Log(result.PermissionNames[0] + " is Granted");
            } else {
                if (AndroidPermissionsManager.IsRefuseShowRequestDialog(result.PermissionNames[0])) {
                    Debug.Log(result.PermissionNames[0] + " denied and refuse show request dialog");
                } else {
                    Debug.Log(result.PermissionNames[0] + " is Denied");
                }
            }
        });
    }

    private void TestQQLogin() {
        DeviceUtils.LoginQQ("Main Camera", "OnLoginMessage", "1106887012");
    }

    private void OnLoginMessage(string message) {
        Debug.Log("QQ Login Message: " + message);
        /*
        接受的值：
        success:
        {"ret":0,"openid":"B864A00529578F656CB1484E766B4F99","access_token":"AFE0B8880B2871B1ADEE39A5D1E908BF","pay_token":"DAC77FBBB8FC29F7B33E932FB811ED97","expires_in":7776000,"pf":"desktop_m_qq-10000144-android-2002-","pfkey":"b633117bfdba8dadf8ad6ba71e144818","msg":"","login_cost":130,"query_authority_cost":271,"authority_cost":0,"expires_time":1533553472991}
        
        failure:
        onError: error message

        cancel:
        onCancel
         */
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        AsyncTask.OnUpdate();
    }
}