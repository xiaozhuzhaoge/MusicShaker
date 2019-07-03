using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

namespace LenovoMirageARSDK.Example
{
    /// <summary>
    /// 手柄Tooltips设置案例
    /// </summary>
    public class Example_TootipSetting : MonoBehaviour
    {

        #region Unity Method

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => VRTK_SDKManager.instance.loadedSetup != null);

            SetTooltips_Trigger("Trigger");
            SetTooltips_Touchpad("Touchpad");
            SetTooltips_App("App");
            SetTooltips_Home("Home");
        }

        private void OnDisable()
        {
            SetTooltips_Trigger("");
            SetTooltips_Trigger("");
            SetTooltips_Trigger("");
            SetTooltips_Trigger("");
        }

        #endregion

        #region Tooltip Setting

        /// <summary>
        /// 设置手柄Touchpad的Tooltip文字，
        /// </summary>
        /// <param name="text">显示的文字，为空则隐藏此Tooltip</param>
        private void SetTooltips_Touchpad(string text)
        {
            if (VRTK_SDKManager.instance == null) return;

            if (VRTK_SDKManager.instance.scriptAliasRightController.gameObject)
            {
                VRTK_SDKManager.instance.scriptAliasRightController.gameObject.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip, text);
            }
        }

        /// <summary>
        /// 设置手柄Trigger的Tooltip文字，
        /// </summary>
        /// <param name="text">显示的文字，为空则隐藏此Tooltip</param>
        private void SetTooltips_Trigger(string text)
        {
            if (VRTK_SDKManager.instance == null) return;

            if (VRTK_SDKManager.instance.scriptAliasRightController.gameObject)
            {
                VRTK_SDKManager.instance.scriptAliasRightController.gameObject.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, text);
            }
        }

        /// <summary>
        /// 设置手柄App键的Tooltip文字，
        /// </summary>
        /// <param name="text">显示的文字，为空则隐藏此Tooltip</param>
        private void SetTooltips_App(string text)
        {
            if (VRTK_SDKManager.instance == null) return;

            if (VRTK_SDKManager.instance.scriptAliasRightController.gameObject)
            {
                VRTK_SDKManager.instance.scriptAliasRightController.gameObject.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.ButtonOneTooltip, text);
            }
        }

        /// <summary>
        /// 设置手柄Home的Tooltip文字，
        /// </summary>
        /// <param name="text">显示的文字，为空则隐藏此Tooltip</param>
        private void SetTooltips_Home(string text)
        {
            if (VRTK_SDKManager.instance == null) return;

            if (VRTK_SDKManager.instance.scriptAliasRightController.gameObject)
            {
                VRTK_SDKManager.instance.scriptAliasRightController.gameObject.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.ButtonTwoTooltip, text);
            }
        }

        #endregion
    }
}



