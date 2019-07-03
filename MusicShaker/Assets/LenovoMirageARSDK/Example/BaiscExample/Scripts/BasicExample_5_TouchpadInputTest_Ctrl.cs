using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ximmerse.Example;
using Ximmerse.InputSystem;
using Ximmerse.Vision;

namespace LenovoMirageARSDK.Example
{
    /// <summary>
    /// The Example of LenovoMirageARSDK
    /// Example of the UniversalController Input
    /// </summary>
    public class BasicExample_5_TouchpadInputTest_Ctrl : MonoBehaviour
    {
        #region Private Properties

        /// <summary>
        /// The VisionSDK
        /// </summary>
        private VisionSDK Sdk;

        /// <summary>
        /// MirageAR InputListener
        /// </summary>
        private MirageAR_InputListener mInputListener;

        /// <summary>
        /// Trigger Press
        /// </summary>
        private bool mIsTriggerPress=false;

        #endregion //Private Properties      

        #region Unity Method

        IEnumerator Start()
        {
            //Wait the SDK Init Complete            
            yield return new WaitUntil(() => VisionSDK.Instance.Inited);
            Sdk = VisionSDK.Instance;

            mInputListener = MirageAR_InputListener.Instance;

            if (mInputListener != null)
            {
                // Touchpad Touch Input Bind
                mInputListener.TouchpadTouch += MInputListener_TouchpadTouch;

                // Touchpad Swipe Input Bind
                mInputListener.TouchpadSwipeUp += MInputListener_TouchpadSwipeUp;
                mInputListener.TouchpadSwipeDown += MInputListener_TouchpadSwipeDown;
                mInputListener.TouchpadSwipeLeft += MInputListener_TouchpadSwipeLeft;
                mInputListener.TouchpadSwipeRight += MInputListener_TouchpadSwipeRight;
                // Touchpad to Dpad Input Bind
                mInputListener.TouchpadDpadCenter += MInputListener_TouchpadDpadCenter;
                mInputListener.TouchpadDpadUp += MInputListener_TouchpadDpadUp;
                mInputListener.TouchpadDpadDown += MInputListener_TouchpadDpadDown;
                mInputListener.TouchpadDpadLeft += MInputListener_TouchpadDpadLeft;
                mInputListener.TouchpadDpadRight += MInputListener_TouchpadDpadRight;

                // Trigger Input Bind
                mInputListener.TriggerPressDown += MInputListener_TriggerPressDown;
                mInputListener.TriggerPressUp += MInputListener_TriggerPressUp;
            }
        }

        private void OnDestroy()
        {
            if (mInputListener != null)
            {
                // Touchpad Touch Input UnBind
                mInputListener.TouchpadTouch -= MInputListener_TouchpadTouch;

                // Touchpad Swipe Input UnBind
                mInputListener.TouchpadSwipeUp -= MInputListener_TouchpadSwipeUp;
                mInputListener.TouchpadSwipeDown -= MInputListener_TouchpadSwipeDown;
                mInputListener.TouchpadSwipeLeft -= MInputListener_TouchpadSwipeLeft;
                mInputListener.TouchpadSwipeRight -= MInputListener_TouchpadSwipeRight;
                // Touchpad to Dpad Input UnBind
                mInputListener.TouchpadDpadCenter -= MInputListener_TouchpadDpadCenter;
                mInputListener.TouchpadDpadUp -= MInputListener_TouchpadDpadUp;
                mInputListener.TouchpadDpadDown -= MInputListener_TouchpadDpadDown;
                mInputListener.TouchpadDpadLeft -= MInputListener_TouchpadDpadLeft;
                mInputListener.TouchpadDpadRight -= MInputListener_TouchpadDpadRight;

                // Trigger Input UnBind
                mInputListener.TriggerPressDown -= MInputListener_TriggerPressDown;
                mInputListener.TriggerPressUp -= MInputListener_TriggerPressUp;
            }           
        }

        #region Touchpad Touch Input Bind

        private void MInputListener_TouchpadTouch(Vector2 touchPos)
        {
            // Press Trigger And Touch Touchpad
            if (mIsTriggerPress)
            {
                Sdk.Logger.Log(string.Format("Touchpad Touch:Touch Position={0}", touchPos));
            }            
        }

        #endregion //Touchpad Touch Input Bind

        #endregion //Unity Method

        #region Touchpad Swipe Input Bind

        private void MInputListener_TouchpadSwipeUp()
        {
            Sdk.Logger.Log("Gesture:Touchpad SwipeUp");
        }

        private void MInputListener_TouchpadSwipeDown()
        {
            Sdk.Logger.Log("Gesture:Touchpad SwipeDown");
        }

        private void MInputListener_TouchpadSwipeLeft()
        {
            Sdk.Logger.Log("Gesture:Touchpad SwipeLeft");
        }

        private void MInputListener_TouchpadSwipeRight()
        {
            Sdk.Logger.Log("Gesture:Touchpad SwipeRight");            
        }

        #endregion //Touchpad Swipe Input Bind

        #region Touchpad to Dpad Input Bind

        private void MInputListener_TouchpadDpadCenter()
        {
            Sdk.Logger.Log("Gesture:DpadClick");
        }

        private void MInputListener_TouchpadDpadUp()
        {
            Sdk.Logger.Log("Gesture:DpadUp");
        }

        private void MInputListener_TouchpadDpadDown()
        {
            Sdk.Logger.Log("Gesture:DpadDown");
        }

        private void MInputListener_TouchpadDpadLeft()
        {
            Sdk.Logger.Log("Gesture:DpadLeft");
        }

        private void MInputListener_TouchpadDpadRight()
        {
            Sdk.Logger.Log("Gesture:DpadRight");
        }

        #endregion //Touchpad to Dpad Input Bind

        #region Trigger Down Up

        private void MInputListener_TriggerPressDown()
        {
            mIsTriggerPress = true;
        }

        private void MInputListener_TriggerPressUp()
        {
            mIsTriggerPress = false;
        }

        #endregion //Trigger Down Up

    }
}


