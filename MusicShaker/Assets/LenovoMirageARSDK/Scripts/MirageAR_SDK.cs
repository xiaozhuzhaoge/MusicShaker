using DeviceManager;
using System.Collections;
using UnityEngine;
using Ximmerse.Vision;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// Lenovosdk Info
    /// </summary>
    public static class MirageAR_ServiceHelper
    {
        /// <summary>
        /// MirageAR Service Connected flag
        /// </summary>
        public static bool mMirageARServiceConnected;

        /// <summary>
        /// To Start MirageAR Service
        /// </summary>
        public static void InitMirageARService()
        {
            Ximmerse.InputSystem.XDevicePlugin.Init();
        }

        /// <summary>
        /// The MirageAR Service Connected Event
        /// </summary>
        public static void MirageARServiceConnected()
        {
            mMirageARServiceConnected = true;
        }

        /// <summary>
        /// The MirageAR Service Disconnected Event
        /// </summary>
        public static void MirageServiceDisconnected()
        {
            mMirageARServiceConnected = false;
        }
    }

    /// <summary>
    /// Lenovo SDK Ctrl
    /// </summary>
    public class MirageAR_SDK : MonoBehaviour
    {
        #region Private Properties

        /// <summary>
        /// Long Press Recenter Duration
        /// </summary>
        private float mRecenterPressDuration = 2;

        /// <summary>
        /// Long Press APP Restart Duration
        /// </summary>
        private float mRestartPressDuration = 3;

        /// <summary>
        /// Button Click Event Space Time(Second),Less Than Space Time Count As A Click Event
        /// </summary>
        private float mBtnClickSpaceTime = 0.5f;

        /// <summary>
        /// Save Home Button Down Time
        /// </summary>
        private float mHomeBtnDownTime;

        /// <summary>
        /// MirageAR InputListener
        /// </summary>
        private MirageAR_InputListener mInputListener;

        /// <summary>
        /// Mirage Controller Peripheral
        /// </summary>        
        private ControllerPeripheral mControllerPeripheral;

        /// <summary>
        /// Common Menu
        /// </summary>
        private GameObject mCommonMenu;

        #endregion

        #region Public Static Properties

        /// <summary>
        /// MirageAR SDK Version
        /// </summary>
        [HideInInspector]
        public static string MirageARSDKVersion = "1.0.10";

        /// <summary>
        /// Launcher Package Name
        /// </summary>
#if UNITY_IOS
        [HideInInspector]
        public const string mLauncherPackageName = "com.naocy.HuoWuiOS";
#else
        [HideInInspector]
        public const string mLauncherPackageName = "com.naocy.HuoWu";
#endif

        #endregion

        #region Public Properties     

        /// <summary>
        /// Whether has Mirage Logger
        /// </summary>
        public bool mHasMirageARLogger;

        /// <summary>
        /// Mirage Logger
        /// </summary>                
        public MirageAR_Logger mMirageARLogger;

        /// <summary>
        /// If the 2D or 3D mode
        /// </summary>
        public bool mIs2DMode;

        #endregion

        #region Attribute

        /// <summary>
        /// LenovoSDK Instance
        /// </summary>
        public static MirageAR_SDK Instance { get; set; }

        /// <summary>
        /// Run As Client,Client Start Will Skip The 2D Setting Scenes
        /// </summary>
        public bool MISClient
        {
            get
            {
#if UNITY_ANDROID        
                // TODO: fix Android Get isClient Value Error
                //return Ximmerse.InputSystem.XDevicePlugin.CheckIsClient();
                return MirageAR_SDK.Instance.mControllerPeripheral.Connected;
#else                 
                return Ximmerse.InputSystem.XDevicePlugin.CheckIsClient();
#endif
            }
        }

        /// <summary>
        /// ControllerPeripheral
        /// </summary>
        public ControllerPeripheral ControllerPeripheral
        {
            get
            {
                if (mControllerPeripheral != null)
                {
                    return mControllerPeripheral;
                }
                else
                {
                    Peripheral peripheral = VisionSDK.Instance.Connections.GetPeripheral(MirageAR_DeviceConstants.PERIPHERAL_CONTROLLER_NAME);
                    return mControllerPeripheral = (ControllerPeripheral)peripheral;
                }
            }

        }

        /// <summary>
        /// HmdPeripheral
        /// </summary>
        public HmdPeripheral HmdPeripheral
        {
            get
            {
                Peripheral peripheral = VisionSDK.Instance.Connections.GetPeripheral(MirageAR_DeviceConstants.PERIPHERAL_HMD_NAME);
                return (HmdPeripheral)peripheral;
            }
        }

        #endregion

        #region Unity Method    

        private void Awake()
        {
            //Output SDK Version
            Debug.LogWarning("MirageAR SDK Version:" + MirageARSDKVersion);

            //Set the Instance
            if (Instance == null)
            {
                Instance = this;
            }
        }

        IEnumerator Start()
        {

#if UNITY_ANDROID && !UNITY_EDITOR

            MirageAR_ServiceHelper.InitMirageARService();

            //Wait the MirageAR Service Connected Succeed
            if (!MirageAR_ServiceHelper.mMirageARServiceConnected)
            {
                //Wait until Android Service Init Complete
                yield return new WaitUntil(() => MirageAR_ServiceHelper.mMirageARServiceConnected);
            }

#elif UNITY_IOS && !UNITY_EDITOR
            //TODO:The URLScheme callback will later than Unity Init Event,So Wait a While
            yield return new WaitForSeconds(0.2f);
#endif

            if (mIs2DMode)
            {

                VisionSDK.Instance.Init(true, mMirageARLogger != null ? mMirageARLogger : null);

                // Add a controller
                mControllerPeripheral = new ControllerPeripheral("XCobra-0");
                VisionSDK.Instance.Connections.AddPeripheral(mControllerPeripheral);
            }
            else
            {
                VisionSDK.Instance.Init(false, mMirageARLogger != null ? mMirageARLogger : null);

                // Init Self Define Devices Setting
                Init_Setting();

                // Enable Mag Correction, it defaults off.
                VisionSDK.Instance.StereoCamera.UseMagnetometerCorrection = false;

                // If you want to use prediction for the HMD (Phone).
                VisionSDK.Instance.Tracking.Hmd.UsePositionPrediction = true;
                VisionSDK.Instance.Tracking.Hmd.UseRotationPrediction = true;

                //create controllerPeripheral
                mControllerPeripheral = new ControllerPeripheral("XCobra-0");
                mControllerPeripheral.UsePositionPrediction = true;

                VisionSDK.Instance.Connections.AddPeripheral(mControllerPeripheral);

                // Button Home Events Bind
                mInputListener = MirageAR_InputListener.Instance;
                if (mInputListener != null)
                {
                    mInputListener.HomePressDown += MInputListener_HomePressDown;
                    mInputListener.HomePressUp += MInputListener_HomePressUp;
                }

                VisionSDK.Instance.Connections.OnPeripheralStateChange += OnPeripheralStateChange;
            }

            yield return null;
        }

        private void Update()
        {
            //Permission Module Depende On This 
            AsyncTask.OnUpdate();
        }


        private void OnDestroy()
        {
            // Button Home Events UnBind
            if (mInputListener != null)
            {
                mInputListener.HomePressDown -= MInputListener_HomePressDown;
                mInputListener.HomePressUp -= MInputListener_HomePressUp;
            }
        }

        #endregion

        #region Private Methods   

        private void OnPeripheralStateChange(object sender, PeripheralStateChangeEventArgs eventArguments)
        {
            Debug.Log("OnPeripheralStateChange: " + eventArguments.Peripheral + " " + eventArguments.Connected);
        }

        private void CalibrationStateChanged(CalibrationState state)
        {
            Debug.Log("CalibrationStateChanged: ");
        }

        #endregion

        #region Event Bind   

        /// <summary>
        /// Home Button Press Down Event
        /// </summary>
        private void MInputListener_HomePressDown()
        {
            Invoke("Recenter", mRecenterPressDuration);

            // Save Home Button Start Press Time
            mHomeBtnDownTime = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// Home Button Press Up Event
        /// </summary>
        private void MInputListener_HomePressUp()
        {
            if (Time.realtimeSinceStartup - mHomeBtnDownTime < mBtnClickSpaceTime)
            {
                //Home Btn Click Event:Jump To Launcher
                //if (Application.identifier != mLauncherPackageName)
                if (true)
                {
                    if (mCommonMenu != null)
                    {
                        mCommonMenu.SetActive(!mCommonMenu.activeInHierarchy);                       
                    }
                    else
                    {
                        mCommonMenu = GameObject.Instantiate(Resources.Load<GameObject>("CommonMenu"));
                        VisionSDK.Instance.TrackedNodes.Add(mCommonMenu.GetComponent<VisionTrackedNode>());
                    }
                }
            }

            if (IsInvoking("Recenter"))
            {
                CancelInvoke("Recenter");
            }
        }

        #endregion

        #region Private Event      

        /// <summary>
        /// Init Self Define Devices Setting
        /// </summary>
        private void Init_Setting()
        {
#if UNITY_ANDROID&&!UNITY_EDITOR
            string value= MirageAR_DeviceApi.GetServiceString("SelectModel");
            if (value=="")
            {
                return;
            }
            else
            {
                // Load Profiles And Apply
                TextAsset profile = Resources.Load<TextAsset>("Profiles/" + value.Replace(".txt", ""));
                VisionSDK.Instance.Settings.DeviceSettings = JsonUtility.FromJson<DeviceSettings>(profile.text);
            }
#endif //!UNITY_EDITOR
        }

        #endregion

        #region Public Event

        /// <summary>
        /// Vibrate Event
        /// </summary>
        /// <param name="strength">The Vibrate Strength</param>
        /// <param name="duration">The Vibrate Duration</param>
        public void Vibrate(int strength = 75, float duration = 0.1f)
        {
            if (mControllerPeripheral == null) return;

            mControllerPeripheral.Vibrate(strength, duration);
        }

        /// <summary>
        /// Re-centers the forward vector for the player the next frame the beacon is seen.
        /// </summary>
        public void Recenter()
        {
            if (VisionSDK.Instance == null) return;

            VisionSDK.Instance.Tracking.Recenter(true);
        }

        #endregion
    }
}
