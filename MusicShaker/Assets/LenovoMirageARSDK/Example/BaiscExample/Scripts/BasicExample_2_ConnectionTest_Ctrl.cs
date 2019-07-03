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
    /// Example of the Peripheral Connection Test
    /// </summary>
    public class BasicExample_2_ConnectionTest_Ctrl : MonoBehaviour
    {
        #region Private Properties    
        /// <summary>
        /// Vision SDK
        /// </summary>
        private VisionSDK Sdk;

        /// <summary>
        /// The Universal Controller
        /// </summary>
        private ControllerPeripheral mUniversalController;

        /// <summary>
        /// The Saber Controller
        /// </summary>
        private ControllerPeripheral mSaberController;

        /// <summary>
        /// The HMD Controller
        /// </summary>
        private HmdPeripheral mHmdController;

        /// <summary>
        /// Controller Type
        /// </summary>
        private MirageAR_DeviceConstants.ControllerType controllerType;

        #endregion   

        #region Unity Method

        IEnumerator Start()
        {
            //Wait the SDK Init Complete            
            yield return new WaitUntil(() => VisionSDK.Instance.Inited);
            Sdk = VisionSDK.Instance;

            // Wait 1 Second,to Make Sure Controller Is Connection Complete
            yield return new WaitForSeconds(1f);
            //Check the State of the Peripheral 
            CheckPeripheralState();

            // Connection Events
            Sdk.Connections.OnPeripheralStateChange += OnPeripheralStateChange;
        }      

        private void OnDestroy()
        {
            if (Sdk == null) return;
            Sdk.Connections.OnPeripheralStateChange -= OnPeripheralStateChange;
        }

        #endregion  //Unity Method

        #region Private Methods 

        /// <summary>
        /// OnPeripheralStateChange,Update State of Peripheral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArguments"></param>
        private void OnPeripheralStateChange(object sender, PeripheralStateChangeEventArgs eventArguments)
        {
            Sdk.Logger.Log("Is " + eventArguments.Peripheral.Name + " Connected? " + eventArguments.Connected);

            //Connected
            if (eventArguments.Connected)
            {
                // Peripheral Type
                if (eventArguments.Peripheral is HmdPeripheral)
                {
                    mHmdController = (HmdPeripheral)eventArguments.Peripheral;

                    //Output the Info of Hmd
                    OutputHmdInfo(mHmdController);
                }
                else
                {
                    if (eventArguments.Peripheral.GetModelName() == MirageAR_DeviceConstants.CONTROLLER_UNIVERSAL_MODELNAME)
                    {
                        controllerType = MirageAR_DeviceConstants.ControllerType.UniversalController;
                        mUniversalController = (ControllerPeripheral)eventArguments.Peripheral;
                    }
                    else if (eventArguments.Peripheral.GetModelName() == MirageAR_DeviceConstants.CONTROLLER_SABER_MODELNAME)
                    {
                        controllerType = MirageAR_DeviceConstants.ControllerType.SaberController;
                        mSaberController = (ControllerPeripheral)eventArguments.Peripheral;
                    }
                    else
                    {
                        Sdk.Logger.Log("Unknow Controller" + mUniversalController.GetDeviceName());
                    }

                    //Output the Info of Controller
                    OutputControllerInfo(eventArguments.Peripheral);
                }
            }
            else //DisConnected
            {
                if (eventArguments.Peripheral is HmdPeripheral)
                {
                    mHmdController = null;
                }
                else
                {
                    if (eventArguments.Peripheral.GetModelName() == MirageAR_DeviceConstants.CONTROLLER_UNIVERSAL_MODELNAME)
                    {
                        mUniversalController = null;
                    }
                    else if (eventArguments.Peripheral.GetModelName() == MirageAR_DeviceConstants.CONTROLLER_SABER_MODELNAME)
                    {
                        mSaberController = null;
                    }
                }
            }
        }

        /// <summary>
        /// Check the State of the Peripheral 
        /// </summary>
        private void CheckPeripheralState()
        {
            //Check The Cotroller Peripheral State
            Peripheral controllerPeripheral = Sdk.Connections.GetPeripheral("XCobra-0");
            if (controllerPeripheral != null && controllerPeripheral.Connected)
            {
                if (controllerPeripheral.GetModelName() == MirageAR_DeviceConstants.CONTROLLER_UNIVERSAL_MODELNAME)
                {
                    controllerType = MirageAR_DeviceConstants.ControllerType.UniversalController;
                    mUniversalController = (ControllerPeripheral)controllerPeripheral;

                }
                else if (controllerPeripheral.GetModelName() == MirageAR_DeviceConstants.CONTROLLER_SABER_MODELNAME)
                {
                    controllerType = MirageAR_DeviceConstants.ControllerType.SaberController;
                    mSaberController = (ControllerPeripheral)controllerPeripheral;
                }
                else
                {
                    Sdk.Logger.Log("Unknow Controller" + controllerPeripheral.GetDeviceName());
                }

                //Output the Info of Controller
                OutputControllerInfo(controllerPeripheral);
            }

            //Check The Hmd Peripheral State
            HmdPeripheral hmdPeripheral = (HmdPeripheral)Sdk.Connections.GetPeripheral("XHawk-0");

            if (hmdPeripheral != null && hmdPeripheral.Connected)
            {
                mHmdController = hmdPeripheral;

                //Output the Info of Hmd
                OutputHmdInfo(mHmdController);
            }
        }

        /// <summary>
        /// Output the Info of Controller
        /// </summary>
        private void OutputControllerInfo(Peripheral peripheral)
        {
            if (!peripheral.Connected) return;

            Sdk.Logger.Log("Controller Device Name: " + peripheral.GetDeviceName());
            Sdk.Logger.Log("Controller FW Info: " + peripheral.GetFirmwareVersion());
            Sdk.Logger.Log("Controller Modle Name: " + peripheral.GetModelName());
            Sdk.Logger.Log("Controller Serial Number: " + peripheral.GetSerialNumber());
            Sdk.Logger.Log("Controller Battery: " + peripheral.GetBatteryLevel());
        }

        /// <summary>
        /// Output the Info of Hmd
        /// </summary>
        private void OutputHmdInfo(HmdPeripheral hmdPeripheral)
        {
            if (!hmdPeripheral.Connected) return;

            Sdk.Logger.Log("HMD Device Name: " + hmdPeripheral.GetDeviceName());
            Sdk.Logger.Log("HMD FW Info: " + hmdPeripheral.GetFirmwareVersion());
            Sdk.Logger.Log("HMD Modle Name: " + hmdPeripheral.GetModelName());
            Sdk.Logger.Log("HMD Serial Number: " + hmdPeripheral.GetSerialNumber());
            Sdk.Logger.Log("HMD Battery: " + hmdPeripheral.GetBatteryLevel());
        }

        #endregion //Private Methods 
    }
}


