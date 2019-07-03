using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ximmerse.Vision;
using System;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// MirageAR Input Listenter
    /// </summary>
    public class MirageAR_InputListener : MonoBehaviour
    {
        #region Private Properties

        /// <summary>
        /// Is Enable Input Check
        /// </summary>
        private bool mEnableInputCheck = false;

        /// <summary>
        /// VisionSDK
        /// </summary>
        private VisionSDK Sdk;       

        /// <summary>
        /// MirageAR_InputListener Instance 
        /// </summary>
        private static MirageAR_InputListener instance = null;

        #endregion //Private Properties

        #region Public Static Properties

        /// <summary>
        /// The Instance of MirageAR_InputListener
        /// </summary>
        public static MirageAR_InputListener Instance
        {
            get
            {
                if (instance == null)
                {
                    var gameObjec = new GameObject("[MirageAR_InputListener]");
                    instance = gameObjec.AddComponent<MirageAR_InputListener>();
                    GameObject.DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }

        /// <summary>
        /// Button Event Event Handele
        /// </summary>
        public delegate void ButtonEventHandler();
        /// <summary>
        /// Touchpad Touch Event Handele
        /// </summary>
        /// <returns></returns>
        public delegate void TouchpadTouchHandler(Vector2 touchPos);

        // Hmd Button Input Delegate
        public event ButtonEventHandler HmdSelectPressDown;
        public event ButtonEventHandler HmdSelectPress;
        public event ButtonEventHandler HmdSelectPressUp;       
        public event ButtonEventHandler HmdBackPressDown;
        public event ButtonEventHandler HmdBackPress;
        public event ButtonEventHandler HmdBackPressUp;
        public event ButtonEventHandler HmdMenuPressDown;
        public event ButtonEventHandler HmdMenuPress;
        public event ButtonEventHandler HmdMenuPressUp;

        // Saber Controller Button Input Delegate
        public event ButtonEventHandler SaberActivatePressDown;
        public event ButtonEventHandler SaberActivatePress;
        public event ButtonEventHandler SaberActivatePressUp;
        public event ButtonEventHandler SaberControlPressDown;
        public event ButtonEventHandler SaberControlPress;
        public event ButtonEventHandler SaberControlPressUp;

        // Universal Controller Button Input Delegate       
        public event ButtonEventHandler AppPressDown;
        public event ButtonEventHandler AppPress;
        public event ButtonEventHandler AppPressUp;
        public event ButtonEventHandler HomePressDown;
        public event ButtonEventHandler HomePress;
        public event ButtonEventHandler HomePressUp;
        public event ButtonEventHandler TriggerPressDown;
        public event ButtonEventHandler TriggerPress;
        public event ButtonEventHandler TriggerPressUp;
        public event ButtonEventHandler TouchpadPressDown;
        public event ButtonEventHandler TouchpadPress;
        public event ButtonEventHandler TouchpadPressUp;

        // Touchpad Gesture Input Delegate
        // Touchpad Touch
        public event TouchpadTouchHandler TouchpadTouch;
        // Touchpad Swipe
        public event ButtonEventHandler TouchpadSwipeUp;
        public event ButtonEventHandler TouchpadSwipeDown;
        public event ButtonEventHandler TouchpadSwipeLeft;
        public event ButtonEventHandler TouchpadSwipeRight;
        // Touchpad to Dpad
        public event ButtonEventHandler TouchpadDpadCenter;
        public event ButtonEventHandler TouchpadDpadUp;
        public event ButtonEventHandler TouchpadDpadDown;
        public event ButtonEventHandler TouchpadDpadLeft;
        public event ButtonEventHandler TouchpadDpadRight;

        #endregion //Public Static Properties

        #region Unity Method

        private IEnumerator Start()
        {
            //Wait the SDK Init Complete            
            yield return new WaitUntil(() => VisionSDK.Instance.Inited);

            Sdk = VisionSDK.Instance;

            // Wait 1 Second,to Make Sure Controller Is Connect Complete
            //yield return new WaitForSeconds(1f);

            mEnableInputCheck = true;
        }

        private void Update()
        {
            if (!mEnableInputCheck) return;
            try
            {
                for (int i = 0; i < Sdk.Connections.Peripherals.Count; i++)
                {
                    if (Sdk.Connections.Peripherals[i] is HmdPeripheral)
                    {
                        CheckHmdInput((HmdPeripheral)Sdk.Connections.Peripherals[i]);
                    }
                    else if (Sdk.Connections.Peripherals[i] is ControllerPeripheral)
                    {
                        if (Sdk.Connections.Peripherals[i].GetModelName() == MirageAR_DeviceConstants.CONTROLLER_UNIVERSAL_MODELNAME)
                        {
                            CheckUniversalControllerInput((ControllerPeripheral)Sdk.Connections.Peripherals[i]);
                        }
                        else if (Sdk.Connections.Peripherals[i].GetModelName() == MirageAR_DeviceConstants.CONTROLLER_SABER_MODELNAME)
                        {
                            CheckSaberControllerInput((ControllerPeripheral)Sdk.Connections.Peripherals[i]);
                        }
                    }
                }
            }
            catch (Exception e){
               
            }
            
        }


        #endregion //Unity Method       

        #region Button Input Check

        /// <summary>
        /// Check the Input of Hmd
        /// </summary>
        private void CheckHmdInput(HmdPeripheral mHmdController)
        {
            // Check Is Connected
            if (!(mHmdController.Input.connectionState == Ximmerse.InputSystem.DeviceConnectionState.Connected)) return;

            #region HmdSelect Button Events

            if (mHmdController.Input.GetButtonDown((uint)ButtonType.HmdSelect))
            { 
                if (HmdSelectPressDown!=null)
                {
                    HmdSelectPressDown();
                }
            }

            if (mHmdController.Input.GetButton((uint)ButtonType.HmdSelect))
            {
                if (HmdSelectPress!=null)
                {
                    HmdSelectPress();
                }
            }

            if (mHmdController.Input.GetButtonUp((uint)ButtonType.HmdSelect))
            {
                if (HmdSelectPressUp!=null)
                {
                    HmdSelectPressUp();
                }
            }

            #endregion  //HmdSelect Button Events

            #region HmdBack Button Events

            if (mHmdController.Input.GetButtonDown((uint)ButtonType.HmdBack))
            {                

                if (HmdBackPressDown!=null)
                {
                    HmdBackPressDown();
                }
            }

            if (mHmdController.Input.GetButton((uint)ButtonType.HmdBack))
            {                

                if (HmdBackPress!=null)
                {
                    HmdBackPress();
                }
            
            }

            if (mHmdController.Input.GetButtonUp((uint)ButtonType.HmdBack))
            {                

                if (HmdBackPressUp!=null)
                {
                    HmdBackPressUp();
                }
            }

            #endregion  //HmdBack Button Events

            #region HmdMenu Button Events

            if (mHmdController.Input.GetButtonDown((uint)ButtonType.HmdMenu))
            {

                if (HmdMenuPressDown!=null)
                {
                    HmdMenuPressDown();
                }
            }

            if (mHmdController.Input.GetButton((uint)ButtonType.HmdMenu))
            {

                if (HmdMenuPress!=null)
                {
                    HmdMenuPress();
                }
            }

            if (mHmdController.Input.GetButtonUp((uint)ButtonType.HmdMenu))
            {

                if (HmdMenuPressUp!=null)
                {
                    HmdMenuPressUp();
                }
            }

            #endregion  //HmdMenu Button Events
        }

        /// <summary>
        /// Check the Input of Saber Controller
        /// </summary>
        private void CheckSaberControllerInput(ControllerPeripheral mSaberController)
        {
            // Check Is Connected
            if (!(mSaberController.ControllerInput.connectionState == Ximmerse.InputSystem.DeviceConnectionState.Connected)) return;

            #region SaberActivate Button Events

            if (mSaberController.ControllerInput.GetButtonDown((uint)ButtonType.SaberActivate))
            {                

                if (SaberActivatePressDown!=null)
                {
                    SaberActivatePressDown();
                }
            }

            if (mSaberController.ControllerInput.GetButton((uint)ButtonType.SaberActivate))
            {               

                if (SaberActivatePress!=null)
                {
                    SaberActivatePress();
                }
            }

            if (mSaberController.ControllerInput.GetButtonUp((uint)ButtonType.SaberActivate))
            {                

                if (SaberActivatePressUp!=null)
                {
                    SaberActivatePressUp();
                }
            }

            #endregion  //SaberActivate Button Events

            #region SaberControl Button Events

            if (mSaberController.ControllerInput.GetButtonDown((uint)ButtonType.SaberControl))
            {                

                if (SaberControlPressDown!=null)
                {
                    SaberControlPressDown();
                }
            }

            if (mSaberController.ControllerInput.GetButton((uint)ButtonType.SaberControl))
            {                

                if (SaberControlPress!=null)
                {
                    SaberControlPress();
                }
            }

            if (mSaberController.ControllerInput.GetButtonUp((uint)ButtonType.SaberControl))
            {                

                if (SaberControlPressUp!=null)
                {
                    SaberControlPressUp();
                }
            }

            #endregion  //SaberControl Button Events
        }

        /// <summary>
        /// Check the Input of Universal Controller
        /// </summary>
        private void CheckUniversalControllerInput(ControllerPeripheral mUniversalController)
        {
            // Check Is Connected
            if (!(mUniversalController.ControllerInput.connectionState == Ximmerse.InputSystem.DeviceConnectionState.Connected)) return;

            #region App Button Events 

            if (mUniversalController.ControllerInput.GetButtonDown((uint)ButtonType.App))
            {
                if (AppPressDown != null)
                {
                    AppPressDown();
                }
            }

            if (mUniversalController.ControllerInput.GetButton((uint)ButtonType.App))
            {
                if (AppPress!=null)
                {
                    AppPress();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonUp((uint)ButtonType.App))
            {
                if (AppPressUp!=null)
                {
                    AppPressUp();
                }
            }

            #endregion  //App Button Events 

            #region Home Button Events 

            if (mUniversalController.ControllerInput.GetButtonDown((uint)ButtonType.Home))
            {
                if (HomePressDown!=null)
                {
                    HomePressDown();
                }               
            }

            if (mUniversalController.ControllerInput.GetButton((uint)ButtonType.Home))
            {
                if (HomePress!=null)
                {
                    HomePress();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonUp((uint)ButtonType.Home))
            {
                if (HomePressUp!=null)
                {
                    HomePressUp();
                }
            }

            #endregion  //Home Button Events 

            #region Trigger Button Events 

            if (mUniversalController.ControllerInput.GetButtonDown((uint)ButtonType.Trigger))
            {
                if (TriggerPressDown!=null)
                {
                    TriggerPressDown();
                }
            }

            //Trigger Not Support Axis Value
            if (mUniversalController.ControllerInput.GetButton((uint)ButtonType.Trigger))
            {
                if (TriggerPress!=null)
                {
                    TriggerPress();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonUp((uint)ButtonType.Trigger))
            {
                if (TriggerPressUp!=null)
                {
                    TriggerPressUp();
                }
            }

            #endregion  //Trigger Button Events 

            #region Touchpad Button Events

            if (mUniversalController.ControllerInput.GetButtonDown((uint)ButtonType.Touchpad))
            {
                if (TouchpadPressDown!=null)
                {
                    TouchpadPressDown();
                }
            }

            if (mUniversalController.ControllerInput.GetButton((uint)ButtonType.Touchpad))
            {
                if (TouchpadPress!=null)
                {
                    TouchpadPress();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonUp((uint)ButtonType.Touchpad))
            {
                if (TouchpadPressUp!=null)
                {
                    TouchpadPressUp();
                }
            }
            #endregion  //Touchpad Button Events           

            #region Touchpad Gesture Event     

            //Touchpad Touch
            if (mUniversalController.ControllerInput.GetButton((uint)TouchpadGestureType.TouchpadTouch))
            {
                Vector2 touchPosValue = mUniversalController.ControllerInput.touchPos;                

                if (TouchpadTouch!=null)
                {
                    TouchpadTouch(touchPosValue);
                }
            }

            //Touchpad Swip Gesture
            if (mUniversalController.ControllerInput.GetButtonUp((uint)TouchpadGestureType.SwipeUp))
            {
                if (TouchpadSwipeUp!=null)
                {
                    TouchpadSwipeUp();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonUp((uint)TouchpadGestureType.SwipeDown))
            {
                if (TouchpadSwipeDown!=null)
                {
                    TouchpadSwipeDown();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonUp((uint)TouchpadGestureType.SwipeRight))
            {
                if (TouchpadSwipeRight!=null)
                {
                    TouchpadSwipeRight();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonUp((uint)TouchpadGestureType.SwipeLeft))
            {
                if (TouchpadSwipeLeft!=null)
                {
                    TouchpadSwipeLeft();
                }
            }

            //Touchpad Corner Click Gesture
            if (mUniversalController.ControllerInput.GetButtonDown((uint)TouchpadGestureType.DpadClick))
            {
                if (TouchpadDpadCenter!=null)
                {
                    TouchpadDpadCenter();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonDown((uint)TouchpadGestureType.DpadUp))
            {
                if (TouchpadDpadUp!=null)
                {
                    TouchpadDpadUp();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonDown((uint)TouchpadGestureType.DpadDown))
            {
                if (TouchpadDpadDown!=null)
                {
                    TouchpadDpadDown();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonDown((uint)TouchpadGestureType.DpadRight))
            {
                if (TouchpadDpadRight!=null)
                {
                    TouchpadDpadRight();
                }
            }

            if (mUniversalController.ControllerInput.GetButtonDown((uint)TouchpadGestureType.DpadLeft))
            {
                if (TouchpadDpadLeft!=null)
                {
                    TouchpadDpadLeft();
                }
            }

            #endregion  //Touchpad Gesture Event
        }

        #endregion //Button Input Check


    }

}

