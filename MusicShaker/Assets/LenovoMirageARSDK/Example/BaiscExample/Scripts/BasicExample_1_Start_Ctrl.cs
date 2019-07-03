using DeviceManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ximmerse.InputSystem;
using Ximmerse.Vision;

namespace LenovoMirageARSDK.Example
{
    /// <summary>
    /// The Example of LenovoMirageARSDK
    /// Example of The 2D StartScene
    /// </summary>
    public class BasicExample_1_Start_Ctrl : MonoBehaviour
    {
        #region Public Properties
        /// <summary>
        /// The Setting Canvas
        /// </summary>
        public GameObject SettingCanvas;

        /// <summary>
        /// The pair button.
        /// </summary>
        public Button PairButton;

        /// <summary>
		/// The start button.
		/// </summary>
		public Button StartGameButton;

        #endregion

        #region Unity Method

        private void Awake()
        {
            SettingCanvas.SetActive(false);
        }

        private static ControllerInput GetControllerInput(Peripheral peripheral)
        {
            ControllerInput input = peripheral.ControllerInput;

            if (peripheral is HmdPeripheral)
            {
                input = ((HmdPeripheral)peripheral).Input;
            }

            return input;
        }

        IEnumerator Start()
        {
            //Wait the SDK Init Complete
            yield return new WaitUntil(() => VisionSDK.Instance.Inited);

            //Init the Periphera Connected State
            InitPeripheraState();

            //Check Whether is Client:true-Enter the Game,false-Go to Setting Page
            if (MirageAR_SDK.Instance.MISClient)
            {
                StartGame();
            }
            else
            {
#if UNITY_ANDROID&&!UNITY_EDITOR
                //Add Permission Check
                if (!CheckMiragePermission()) MirageAR_AndroidPermissionManager.RequestMirageARPermissions();                
#endif

                //Open Setting(Pairing) UI
                SettingCanvas.SetActive(true);
            }

            VisionSDK.Instance.Connections.OnPeripheralStateChange += OnPeripheralStateChange;
        }

        private void OnDestroy()
        {
            VisionSDK.Instance.Connections.OnPeripheralStateChange -= OnPeripheralStateChange;
        }

        #endregion

        #region Public Methods

        public void StartGame()
        {
            //Load The Second Scene
            SceneManager.LoadScene(1);
        }

        public void StartPairing()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
                //Add Permission Check
            if (CheckMiragePermission())
            {
                // Start scanning for peripherals
                VisionSDK.Instance.Tracking.StartPairing();

                // Update text
                PairButton.GetComponentInChildren<Text>().text = "Looking...";

                // Stop scanning in 10 seconds
                Invoke("StopPairing", 10.0f);
            }
            else
            {
                MirageAR_AndroidPermissionManager.RequestMirageARPermissions();
            }  
#else
            // Start scanning for peripherals
            VisionSDK.Instance.Tracking.StartPairing();

            // Update text
            PairButton.GetComponentInChildren<Text>().text = "Looking...";

            // Stop scanning in 10 seconds
            Invoke("StopPairing", 10.0f);
#endif           
        }

        public void StopPairing()
        {
            // Stop scanning
            VisionSDK.Instance.Tracking.StopPairing();

            // Update text
            PairButton.GetComponentInChildren<Text>().text = "Start Pairing";
        }

#endregion

#region Private Methods

        /// <summary>
        /// Init the Periphera Connected State
        /// </summary>
        private void InitPeripheraState()
        {
            //Init the Hmd Peripheral State
            HmdPeripheral hmdPeripheral = VisionSDK.Instance.Connections.GetPeripheral("XHawk-0") as HmdPeripheral;
            if (hmdPeripheral != null)
            {
                StartGameButton.GetComponent<Image>().color = (hmdPeripheral.Connected) ? Color.green : Color.white;
            }

            //Init the Controller Periphera State
            Peripheral controllerPeripheral = VisionSDK.Instance.Connections.GetPeripheral("XCobra-0");
            if (controllerPeripheral != null)
            {
                PairButton.GetComponent<Image>().color = (controllerPeripheral.Connected) ? Color.green : Color.white;
            }
        }

        private void OnPeripheralStateChange(object sender, PeripheralStateChangeEventArgs eventArguments)
        {
            Debug.Log("OnPeripheralStateChange: " + eventArguments.Peripheral + " " + eventArguments.Connected);

            // Peripheral Type
            if (eventArguments.Peripheral is HmdPeripheral)
            {
                // Update the Start Demo Button color
                StartGameButton.GetComponent<Image>().color = (eventArguments.Connected) ? Color.green : Color.white;
            }
            else
            {
                if (eventArguments.Connected)
                {
                    // Stop Pairing now that it is found.
                    CancelInvoke("StopPairing");
                    StopPairing();

                    // Update the pair button color
                    PairButton.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    // Update the pair button color
                    PairButton.GetComponent<Image>().color = Color.white;
                }
            }
        }

        /// <summary>
        /// Check Mirage Needed Permission
        /// </summary>
        private bool CheckMiragePermission()
        {
            bool value = true;
            foreach (var item in MirageAR_AndroidPermissionManager.MirageARPermissions)
            {
                if (!DeviceUtils.CheckPermission(item)) value = false;
            }
            return value;
        }

#endregion

    }

}
