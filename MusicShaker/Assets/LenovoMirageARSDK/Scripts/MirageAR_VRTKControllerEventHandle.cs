using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// Execute VRTK Controller Evets
    /// </summary>
    [RequireComponent(typeof(VRTK_ControllerEvents))]
    public class MirageAR_VRTKControllerEventHandle : MonoBehaviour
    {

        #region Button Event Bind

        /// <summary>
        /// Trigger Button Press Event Bind
        /// </summary>
        public void TriggerPressEventBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().TriggerPressed += eventHandler;
        }

        /// <summary>
        ///  Trigger Button Press Event UnBind
        /// </summary>
        /// <param name="eventHandler"></param>
        public void TriggerPressEventUnBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().TriggerPressed -= eventHandler;
        }

        /// <summary>
        /// Trigger Button Release Event Bind
        /// </summary>
        public void TriggerReleaseEventBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().TriggerReleased += eventHandler;
        }

        /// <summary>
        ///  Trigger Button Release Event UnBind
        /// </summary>
        /// <param name="eventHandler"></param>
        public void TriggerReleaseEventUnBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().TriggerReleased -= eventHandler;
        }

        /// <summary>
        /// Touchpad Button Press Event Bind
        /// </summary>
        public void TouchpadPressedEventBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().TouchpadPressed += eventHandler;
        }

        /// <summary>
        ///  Touchpad Button Press Event UnBind
        /// </summary>
        /// <param name="eventHandler"></param>
        public void TouchpadPressedEventUnBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().TouchpadPressed -= eventHandler;
        }

        /// <summary>
        /// Touchpad Button Release Event Bind
        /// </summary>
        public void TouchpadReleaseEventBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().TouchpadReleased += eventHandler;
        }

        /// <summary>
        ///  Touchpad Button Release Event UnBind
        /// </summary>
        /// <param name="eventHandler"></param>
        public void TouchpadReleaseEventUnBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().TouchpadReleased -= eventHandler;
        }


        /// <summary>
        /// ButtonOne Button Release Event Bind
        /// </summary>
        public void ButtonOnePressEventBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += eventHandler;
        }

        /// <summary>
        /// ButtonOne Button Release Event UnBind
        /// </summary>
        public void ButtonOnePressEventUnBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().ButtonOnePressed -= eventHandler;
        }

        /// <summary>
        /// ButtonTwo Button Release Event Bind
        /// </summary>
        public void ButtonTwoPressEventBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += eventHandler;
        }

        /// <summary>
        /// ButtonTwo Button Release Event UnBind
        /// </summary>
        public void ButtonTwoPressEventUnBind(ControllerInteractionEventHandler eventHandler)
        {
            GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed -= eventHandler;
        }

        #endregion //Button Event Bind
    }
}
