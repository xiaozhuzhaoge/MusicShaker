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
    /// Example of the Hmd,SaberController,UniversalController Input
    /// </summary>
    public class BasicExample_4_InputTest_Ctrl : MonoBehaviour
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
                // Hmd Button Input Bind
                mInputListener.HmdSelectPressDown += MInputListener_HmdSelectPressDown;
                mInputListener.HmdSelectPress += MInputListener_HmdSelectPress;                
                mInputListener.HmdSelectPressUp += MInputListener_HmdSelectPressUp;
                mInputListener.HmdBackPressDown += MInputListener_HmdBackPressDown;
                mInputListener.HmdBackPress += MInputListener_HmdBackPress;                
                mInputListener.HmdBackPressUp += MInputListener_HmdBackPressUp;
                mInputListener.HmdMenuPressDown += MInputListener_HmdMenuPressDown;
                mInputListener.HmdMenuPress += MInputListener_HmdMenuPress;                
                mInputListener.HmdMenuPressUp += MInputListener_HmdMenuPressUp;

                // Saber Controller Button Input Bind
                mInputListener.SaberActivatePressDown += MInputListener_SaberActivatePressDown;
                mInputListener.SaberActivatePress += MInputListener_SaberActivatePress;
                mInputListener.SaberActivatePressUp += MInputListener_SaberActivatePressUp;
                mInputListener.SaberControlPressDown += MInputListener_SaberControlPressDown;
                mInputListener.SaberControlPress += MInputListener_SaberControlPress;
                mInputListener.SaberControlPressUp += MInputListener_SaberControlPressUp;

                // Universal Controller Button Input Bind
                mInputListener.AppPressDown += MInputListener_AppPressDown;
                mInputListener.AppPress += MInputListener_AppPress;                
                mInputListener.AppPressUp += MInputListener_AppPressUp;
                mInputListener.HomePressDown += MInputListener_HomePressDown;
                mInputListener.HomePress += MInputListener_HomePress;
                mInputListener.HomePressUp += MInputListener_HomePressUp;
                mInputListener.TriggerPressDown += MInputListener_TriggerPressDown;
                mInputListener.TriggerPress += MInputListener_TriggerPress;
                mInputListener.TriggerPressUp += MInputListener_TriggerPressUp;
                mInputListener.TouchpadPressDown += MInputListener_TouchpadPressDown;
                mInputListener.TouchpadPress += MInputListener_TouchpadPress;            
                mInputListener.TouchpadPressUp += MInputListener_TouchpadPressUp;
            }
        }

        private void OnDestroy()
        {
            if (mInputListener != null)
            {
                // Hmd Button Input UnBind
                mInputListener.HmdSelectPressDown -= MInputListener_HmdSelectPressDown;
                mInputListener.HmdSelectPress -= MInputListener_HmdSelectPress;
                mInputListener.HmdSelectPressUp -= MInputListener_HmdSelectPressUp;
                mInputListener.HmdBackPressDown -= MInputListener_HmdBackPressDown;
                mInputListener.HmdBackPress -= MInputListener_HmdBackPress;
                mInputListener.HmdBackPressUp -= MInputListener_HmdBackPressUp;
                mInputListener.HmdMenuPressDown -= MInputListener_HmdMenuPressDown;
                mInputListener.HmdMenuPress -= MInputListener_HmdMenuPress;
                mInputListener.HmdMenuPressUp -= MInputListener_HmdMenuPressUp;

                // Saber Controller Button Input UnBind
                mInputListener.SaberActivatePressDown -= MInputListener_SaberActivatePressDown;
                mInputListener.SaberActivatePress -= MInputListener_SaberActivatePress;
                mInputListener.SaberActivatePressUp -= MInputListener_SaberActivatePressUp;
                mInputListener.SaberControlPressDown -= MInputListener_SaberControlPressDown;
                mInputListener.SaberControlPress -= MInputListener_SaberControlPress;
                mInputListener.SaberControlPressUp -= MInputListener_SaberControlPressUp;

                // Universal Controller Button Input UnBind
                mInputListener.AppPressDown -= MInputListener_AppPressDown;
                mInputListener.AppPress -= MInputListener_AppPress;
                mInputListener.AppPressUp -= MInputListener_AppPressUp;
                mInputListener.HomePressDown -= MInputListener_HomePressDown;
                mInputListener.HomePress -= MInputListener_HomePress;
                mInputListener.HomePressUp -= MInputListener_HomePressUp;
                mInputListener.TriggerPressDown -= MInputListener_TriggerPressDown;
                mInputListener.TriggerPress -= MInputListener_TriggerPress;
                mInputListener.TriggerPressUp -= MInputListener_TriggerPressUp;
                mInputListener.TouchpadPressDown -= MInputListener_TouchpadPressDown;
                mInputListener.TouchpadPress -= MInputListener_TouchpadPress;
                mInputListener.TouchpadPressUp -= MInputListener_TouchpadPressUp;
            }
        }

        #endregion  //Unity Method

        #region Hmd Button Input Bind

        private void MInputListener_HmdSelectPressDown()
        {
            Sdk.Logger.Log("Button:HmdSelect Down");
        }

        private void MInputListener_HmdSelectPress()
        {
            //Sdk.Logger.Log("Button:HmdSelect Pressing");
        }

        private void MInputListener_HmdSelectPressUp()
        {
            Sdk.Logger.Log("Button:HmdSelect Up");
        }

        private void MInputListener_HmdBackPressDown()
        {
            Sdk.Logger.Log("Button:HmdBack Down");
        }

        private void MInputListener_HmdBackPress()
        {
            //Sdk.Logger.Log("Button:HmdBack Pressing");
        }

        private void MInputListener_HmdBackPressUp()
        {
            Sdk.Logger.Log("Button:HmdBack Up");
        }

        private void MInputListener_HmdMenuPressDown()
        {
            Sdk.Logger.Log("Button:HmdMenu Down");            
        }      

        private void MInputListener_HmdMenuPress()
        {
            //Sdk.Logger.Log("Button:HmdMenu Pressing");
        }

        private void MInputListener_HmdMenuPressUp()
        {            
            Sdk.Logger.Log("Button:HmdMenu Up");
        }

        #endregion //Hmd Button Input Bind

        #region Saber Controller Button Input Bind

        private void MInputListener_SaberActivatePressDown()
        {
            Sdk.Logger.Log("Button:SaberActivate Down");
        }

        private void MInputListener_SaberActivatePress()
        {
            //Sdk.Logger.Log("Button:SaberActivate Pressing");
        }

        private void MInputListener_SaberActivatePressUp()
        {
            Sdk.Logger.Log("Button:SaberActivate Up");
        }

        private void MInputListener_SaberControlPressDown()
        {
            Sdk.Logger.Log("Button:SaberControl Down");
        }

        private void MInputListener_SaberControlPress()
        {
            //Sdk.Logger.Log("Button:SaberControl Pressing");
        }

        private void MInputListener_SaberControlPressUp()
        {
            Sdk.Logger.Log("Button:SaberControl Up");
        }

        #endregion //Saber Controller Button Input Bind

        #region Universal Controller Input Bind

        private void MInputListener_AppPressDown()
        {
            Sdk.Logger.Log("Button:App Down");
        }

        private void MInputListener_AppPress()
        {
            //Sdk.Logger.Log("Button:App Pressing");
        }

        private void MInputListener_AppPressUp()
        {
            Sdk.Logger.Log("Button:App Up");
        }

        private void MInputListener_HomePressDown()
        {
            Sdk.Logger.Log("Button:Home Down");
        }

        private void MInputListener_HomePress()
        {
            //Sdk.Logger.Log("Button:Home Pressing");
        }

        private void MInputListener_HomePressUp()
        {
            Sdk.Logger.Log("Button:Home Up");
        }

        private void MInputListener_TriggerPressDown()
        {
            Sdk.Logger.Log("Button:Trigger Down");
        }

        private void MInputListener_TriggerPress()
        {
            //Sdk.Logger.Log("Button:Trigger Pressing");
        }

        private void MInputListener_TriggerPressUp()
        {
            Sdk.Logger.Log("Button:Trigger Up");
        }

        private void MInputListener_TouchpadPressDown()
        {
            Sdk.Logger.Log("Button:Touchpad Down");
        }

        private void MInputListener_TouchpadPress()
        {
            //Sdk.Logger.Log("Button:Touchpad Pressing)");
        }

        private void MInputListener_TouchpadPressUp()
        {
            Sdk.Logger.Log("Button:Touchpad Up");
        }

        #endregion //Universal Controller Input Bind

    }
}


