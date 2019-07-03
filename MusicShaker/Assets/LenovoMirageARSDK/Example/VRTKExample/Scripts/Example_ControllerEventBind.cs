using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using Ximmerse.Vision;

namespace LenovoMirageARSDK.Example
{
    /// <summary>
    /// 手柄控制器按键绑定案例
    /// </summary>
    public class Example_ControllerEventBind : MonoBehaviour
    {
        public Text text;

        #region Unity Method

        private IEnumerator Start()
        {
            
            yield return new WaitUntil(()=>(VisionSDK.Instance!=null));

            //按键绑定
            VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<MirageAR_VRTKControllerEventHandle>().TouchpadPressedEventBind(TouchpadPressed);
            VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<MirageAR_VRTKControllerEventHandle>().TriggerPressEventBind(TriggerPressed);
            VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<MirageAR_VRTKControllerEventHandle>().ButtonOnePressEventBind(AppPress);
            VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<MirageAR_VRTKControllerEventHandle>().ButtonTwoPressEventBind(HomePress);
        }
      

        private void OnDisable()
        {
            //按键解绑
            if (!VRTK_SDKManager.instance) return;
            VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<MirageAR_VRTKControllerEventHandle>().TouchpadPressedEventUnBind(TouchpadPressed);
            VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<MirageAR_VRTKControllerEventHandle>().TriggerPressEventUnBind(TriggerPressed);
            VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<MirageAR_VRTKControllerEventHandle>().ButtonOnePressEventUnBind(AppPress);
            VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<MirageAR_VRTKControllerEventHandle>().ButtonTwoPressEventUnBind(HomePress);
        }

        #endregion


        #region Bind Event

        private void TouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            Debug.Log("@@TouchpadPressed");
            text.text = "TouchpadPressed";
        }

        private void TriggerPressed(object sender, ControllerInteractionEventArgs e)
        {
            Debug.Log("@@TriggerPressed");
            text.text = "TriggerPressed";
        }

        private void AppPress(object sender, ControllerInteractionEventArgs e)
        {
            Debug.Log("@@AppPress");
            text.text = "AppPress";
        }

        private void HomePress(object sender, ControllerInteractionEventArgs e)
        {
            Debug.Log("@@HomePress");
            text.text = "HomePress";
        }

        #endregion
    }
}


