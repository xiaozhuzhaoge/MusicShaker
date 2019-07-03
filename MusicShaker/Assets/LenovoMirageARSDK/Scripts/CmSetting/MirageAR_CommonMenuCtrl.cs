using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DeviceManager;

namespace LenovoMirageARSDK
{
    /// <summary>
    /// CommonMenu Ctrl
    /// </summary>
    public class MirageAR_CommonMenuCtrl : MonoBehaviour
    {
        #region Private Properties
        
        private Canvas uiCanvas;
        // Btns
        private GameObject m_BtnCancel;
        private GameObject m_BtnQuit;
        // Infos
        private GameObject m_HmdInfo_Connected;
        private GameObject m_HmdInfo_DisConnected;
        private Text m_HmdInfo_Text;
        private GameObject m_ControllerInfo_Connected;
        private GameObject m_ControllerInfo_DisConnected;
        private Text m_ControllerInfo_Text;
        // Dashboard
        private GameObject m_VolumeDashboard;
        private GameObject m_VolumeDashboard_Title;
        private GameObject m_VolumeDashboard_CircleBg;
        private GameObject m_VolumeDashboard_CircleFront;
        private GameObject m_BrightnessDashboard;
        private GameObject m_BrightnessDashboard_Title;
        private GameObject m_BrightnessDashboard_CircleBg;
        private GameObject m_BrightnessDashboard_CircleFront;

        /// <summary>
        /// The Connect State of Hmd
        /// </summary>
        private bool m_HmdConnect;
        
        /// <summary>
        /// The Connect State of Controller
        /// </summary>
        private bool m_ControllerConnect;

        /// <summary>
        /// Low Level Color
        /// </summary>
        private Color m_LowLevelColor = Color.red;

        /// <summary>
        /// Normal Level Color
        /// </summary>
        private Color m_NormalLevelColor = Color.white;

        /// <summary>
        /// Update Freqency
        /// </summary>
        private const float m_UpdateFreq=2.0f;

        /// <summary>
        /// Dashboard Anim Duration
        /// </summary>
        private const float m_DashboardAnimDuration = 0.3f;

        /// <summary>
        /// backup older VRTKInputActivationMode
        /// </summary>
        private VRTK.VRTK_UIPointer.ActivationMethods bakVRTKInputActivationMode;

        #endregion //Private Properties

        #region Unity Method

        private void Awake()
        {
            uiCanvas = gameObject.transform.Find("Canvas").GetComponent<Canvas>();
            m_BtnCancel = gameObject.transform.Find("Canvas/Panel/Buttons/Btn_Cancel").gameObject;
            m_BtnQuit = gameObject.transform.Find("Canvas/Panel/Buttons/Btn_Quit").gameObject;
            m_HmdInfo_Connected = gameObject.transform.Find("Canvas/Panel/Infos/HmdInfo/Image/Connected").gameObject;
            m_HmdInfo_DisConnected = gameObject.transform.Find("Canvas/Panel/Infos/HmdInfo/Image/DisConnected").gameObject;
            m_HmdInfo_Text = gameObject.transform.Find("Canvas/Panel/Infos/HmdInfo/Text").GetComponent<Text>();
            m_ControllerInfo_Connected = gameObject.transform.Find("Canvas/Panel/Infos/ControllerInfo/Image/Connected").gameObject;
            m_ControllerInfo_DisConnected = gameObject.transform.Find("Canvas/Panel/Infos/ControllerInfo/Image/DisConnected").gameObject;
            m_ControllerInfo_Text = gameObject.transform.Find("Canvas/Panel/Infos/ControllerInfo/Text").GetComponent<Text>();
            m_VolumeDashboard = gameObject.transform.Find("Canvas/Panel/Dashboard/Volume").gameObject;
            m_VolumeDashboard_Title = gameObject.transform.Find("Canvas/Panel/Dashboard/Volume/Title").gameObject;
            m_VolumeDashboard_CircleBg = gameObject.transform.Find("Canvas/Panel/Dashboard/Volume/CircleBg").gameObject;
            m_VolumeDashboard_CircleFront = gameObject.transform.Find("Canvas/Panel/Dashboard/Volume/CircleFront").gameObject;            
            m_BrightnessDashboard = gameObject.transform.Find("Canvas/Panel/Dashboard/Brightness").gameObject;
            m_BrightnessDashboard_Title = gameObject.transform.Find("Canvas/Panel/Dashboard/Brightness/Title").gameObject;
            m_BrightnessDashboard_CircleBg = gameObject.transform.Find("Canvas/Panel/Dashboard/Brightness/CircleBg").gameObject;
            m_BrightnessDashboard_CircleFront = gameObject.transform.Find("Canvas/Panel/Dashboard/Brightness/CircleFront").gameObject;
        } 

        private void OnEnable()
        {
            InvokeRepeating("UpdateHmdInfo", 0, m_UpdateFreq);
            InvokeRepeating("UpdateComtrollerInfo", 0, m_UpdateFreq);

            MirageAR_UIEventListener.Get(m_BtnCancel).onClickWithData += BtnCancel_Onclick;
            MirageAR_UIEventListener.Get(m_BtnQuit).onClickWithData += BtnQuit_Onclick;

            MirageAR_UIEventListener.Get(m_VolumeDashboard_CircleFront).onClickWithData += VolumeDashboard_CircleFront_OnClickWithData;
            MirageAR_UIEventListener.Get(m_VolumeDashboard_CircleFront).onPointerEnter += VolumeDashboard_CircleFront_OnPointerEnter;
            MirageAR_UIEventListener.Get(m_VolumeDashboard_CircleFront).onPointerExit += VolumeDashboard_CircleFront_OnPointerExit;
            MirageAR_UIEventListener.Get(m_BrightnessDashboard_CircleFront).onClickWithData += BrightnessDashboard_CircleFront_OnClickWithData;
            MirageAR_UIEventListener.Get(m_BrightnessDashboard_CircleFront).onPointerEnter += BrightnessDashboard_CircleFront_OnPointerEnter;
            MirageAR_UIEventListener.Get(m_BrightnessDashboard_CircleFront).onPointerExit += BrightnessDashboard_CircleFront_OnPointerExit;

            // Set VRTK Input Activation Mode 
            SetVRTKInputActivationMode();
        }

        private void OnDisable()
        {
            CancelInvoke("UpdateHmdInfo");
            CancelInvoke("UpdateComtrollerInfo");

            MirageAR_UIEventListener.Get(m_BtnCancel).onClickWithData -= BtnCancel_Onclick;
            MirageAR_UIEventListener.Get(m_BtnQuit).onClickWithData -= BtnQuit_Onclick;

            MirageAR_UIEventListener.Get(m_VolumeDashboard_CircleFront).onClickWithData -= VolumeDashboard_CircleFront_OnClickWithData;
            MirageAR_UIEventListener.Get(m_VolumeDashboard_CircleFront).onPointerEnter -= VolumeDashboard_CircleFront_OnPointerEnter;
            MirageAR_UIEventListener.Get(m_VolumeDashboard_CircleFront).onPointerExit -= VolumeDashboard_CircleFront_OnPointerExit;
            MirageAR_UIEventListener.Get(m_BrightnessDashboard_CircleFront).onClickWithData -= BrightnessDashboard_CircleFront_OnClickWithData;
            MirageAR_UIEventListener.Get(m_BrightnessDashboard_CircleFront).onPointerEnter -= BrightnessDashboard_CircleFront_OnPointerEnter;
            MirageAR_UIEventListener.Get(m_BrightnessDashboard_CircleFront).onPointerExit -= BrightnessDashboard_CircleFront_OnPointerExit;

            // Recover VRTK Input Activation Mode
            RecoverVRTKInputActivationMode();
        }

        #endregion //Unity Method

        #region Private Method      

        /// <summary>
        /// Set VRTK Input Activation Mode 
        /// </summary>
        private void SetVRTKInputActivationMode()
        {
            // VRTK Used
            if (VRTK.VRTK_SDKManager.instance.loadedSetup!=null)
            {
                if (VRTK.VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<VRTK.VRTK_UIPointer>()!=null)
                {
                    bakVRTKInputActivationMode = VRTK.VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<VRTK.VRTK_UIPointer>().activationMode;
                    VRTK.VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<VRTK.VRTK_UIPointer>().activationMode = VRTK.VRTK_UIPointer.ActivationMethods.AlwaysOn;
                }
            }
        }

        /// <summary>
        /// Recover VRTK Input Activation Mode
        /// </summary>
        private void RecoverVRTKInputActivationMode()
        {
            // VRTK Used
            if (VRTK.VRTK_SDKManager.instance.loadedSetup != null)
            {
                if (VRTK.VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<VRTK.VRTK_UIPointer>() != null)
                {                   
                    VRTK.VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<VRTK.VRTK_UIPointer>().activationMode = bakVRTKInputActivationMode;
                }
            }
        }

        /// <summary>
        /// Init Volume UI
        /// </summary>
        private void InitVolumeUI()
        {
            UpdateVolumeUI(true, DeviceUtils.GetVolume());
        }

        /// <summary>
        /// Init Brightness UI
        /// </summary>
        private void InitBrightnessUI()
        {
            UpdateBrightnessUI(true, DeviceUtils.GetBrightness());
        }

        /// <summary>
        /// Update Hmd Info 
        /// </summary>
        private void UpdateHmdInfo()
        {
            if (MirageAR_SDK.Instance.HmdPeripheral.ControllerInput.connectionState == Ximmerse.InputSystem.DeviceConnectionState.Connected)
            {
                m_HmdInfo_Connected.SetActive(true);
                m_HmdInfo_DisConnected.SetActive(false);
                int batteryLevel = Mathf.Clamp(MirageAR_SDK.Instance.HmdPeripheral.GetBatteryLevel(), 0, 100);
                m_HmdInfo_Text.gameObject.SetActive(true);
                m_HmdInfo_Text.color = batteryLevel <= 20 ? m_LowLevelColor : m_NormalLevelColor;
                m_HmdInfo_Text.text = batteryLevel.ToString() + "%";
            }
            else
            {
                m_HmdInfo_Connected.SetActive(false);
                m_HmdInfo_DisConnected.SetActive(true);
                m_HmdInfo_Text.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Update Comtroller Info
        /// </summary>
        private void UpdateComtrollerInfo()
        {
            if (MirageAR_SDK.Instance.ControllerPeripheral.ControllerInput.connectionState == Ximmerse.InputSystem.DeviceConnectionState.Connected)
            {
                m_ControllerInfo_Connected.SetActive(true);
                m_ControllerInfo_DisConnected.SetActive(false);
                int batteryLevel = Mathf.Clamp(MirageAR_SDK.Instance.ControllerPeripheral.GetBatteryLevel(), 0, 100);
                m_ControllerInfo_Text.gameObject.SetActive(true);
                m_ControllerInfo_Text.color = batteryLevel <= 20 ? m_LowLevelColor : m_NormalLevelColor;
                m_ControllerInfo_Text.text = batteryLevel.ToString() + "%";
            }
            else
            {
                m_ControllerInfo_Connected.SetActive(false);
                m_ControllerInfo_DisConnected.SetActive(true);
                m_ControllerInfo_Text.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 根据音量值,跟新音量UI
        /// </summary>
        private void UpdateVolumeUI(bool immediately, float value)
        {
            if (immediately)
            {
                m_VolumeDashboard_CircleFront.GetComponent<Image>().fillAmount = Mathf.Clamp01(value);
            }
            else
            {
                m_VolumeDashboard_CircleFront.GetComponent<Image>().DOFillAmount(Mathf.Clamp01(value), 0.6f);
            }            
        }

        /// <summary>
        /// Update Brightness UI
        /// </summary>
        private void UpdateBrightnessUI(bool immediately,float value)
        {
            if (immediately)
            {
                m_BrightnessDashboard_CircleFront.GetComponent<Image>().fillAmount = Mathf.Clamp01(value);
            }
            else
            {
                m_BrightnessDashboard_CircleFront.GetComponent<Image>().DOFillAmount(Mathf.Clamp01(value), 0.6f);
            }
        }

        /// <summary>
        /// 返回Target Z轴修改后的Vector值
        /// </summary>
        private Vector3 OutVector(GameObject obj, float depth)
        {
            Vector3 origin = obj.GetComponent<RectTransform>().anchoredPosition3D;
            return new Vector3(origin.x, origin.y, depth);
        }

        /// <summary>
        /// Get Dashboard Input Value
        /// </summary>
        private float GetDashboardInputValue(Vector2 clickPos,Vector2 origin)
        {
            Vector2 relative = clickPos- origin;
            
            float angle = Vector2.Angle(relative, Vector2.up);
            // unsinged angle
            if (relative.x < 0)
            {
                angle = 360- angle;
            }
            return Mathf.Clamp(angle/360,0,1);
        }

        #endregion //Private Method

        #region Event Bind

        private void BtnCancel_Onclick(BaseEventData obj)
        {
            this.gameObject.SetActive(false);
        }

        private void BtnQuit_Onclick(BaseEventData obj)
        {
            // Quit App
            Application.runInBackground = false;
            Application.Quit();

#if UNITY_ANDROID && !UNITY_EDITOR

            DeviceUtils.OpenApplication(MirageAR_SDK.mLauncherPackageName);

#elif UNITY_IOS && !UNITY_EDITOR

            DeviceUtils.OpenApplication(MirageAR_SDK.mLauncherPackageName.Split('.')[2]+"://");

#endif
        }

        private void VolumeDashboard_CircleFront_OnClickWithData(BaseEventData obj)
        {
            PointerEventData data = obj as PointerEventData;

            // Get Click Position
            Vector2 clickPos;            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform,
            data.position, uiCanvas.transform.GetComponent<GraphicRaycaster>().eventCamera, out clickPos);

            // Get UI Position
            Vector2 uiPos=RectTransformUtility.WorldToScreenPoint(uiCanvas.transform.GetComponent<GraphicRaycaster>().eventCamera,m_VolumeDashboard_CircleFront.GetComponent<RectTransform>().position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform,
           uiPos, uiCanvas.transform.GetComponent<GraphicRaycaster>().eventCamera, out uiPos);         

            float fillAmountValue = GetDashboardInputValue(clickPos, uiPos);

            m_VolumeDashboard_CircleFront.GetComponent<Image>().DOFillAmount(fillAmountValue, m_DashboardAnimDuration);
            
            // Set Volume
            DeviceUtils.SetVolume(fillAmountValue);

        }

        private void VolumeDashboard_CircleFront_OnPointerEnter(BaseEventData obj)
        {
            Sequence moveSequence = DOTween.Sequence();
            moveSequence.Append(m_VolumeDashboard.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_VolumeDashboard,-50), m_DashboardAnimDuration))
                .Join(m_VolumeDashboard_CircleFront.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_VolumeDashboard_CircleFront,-50), m_DashboardAnimDuration))
                .Join(m_VolumeDashboard_Title.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_VolumeDashboard_Title,-100),m_DashboardAnimDuration).SetDelay(m_DashboardAnimDuration*0.8f));
        }

        private void VolumeDashboard_CircleFront_OnPointerExit(BaseEventData obj)
        {
            Sequence moveSequence = DOTween.Sequence();
            moveSequence.Append(m_VolumeDashboard.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_VolumeDashboard, 0), m_DashboardAnimDuration))
                .Join(m_VolumeDashboard_CircleFront.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_VolumeDashboard_CircleFront, 0), m_DashboardAnimDuration))
                .Join(m_VolumeDashboard_Title.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_VolumeDashboard_Title, 0), m_DashboardAnimDuration));
        }

        private void BrightnessDashboard_CircleFront_OnClickWithData(BaseEventData obj)
        {
            PointerEventData data = obj as PointerEventData;

            // Get Click Position
            Vector2 clickPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform,
            data.position, uiCanvas.transform.GetComponent<GraphicRaycaster>().eventCamera, out clickPos);

            // Get UI Position
            Vector2 uiPos = RectTransformUtility.WorldToScreenPoint(uiCanvas.transform.GetComponent<GraphicRaycaster>().eventCamera, m_BrightnessDashboard_CircleFront.GetComponent<RectTransform>().position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform,
           uiPos, uiCanvas.transform.GetComponent<GraphicRaycaster>().eventCamera, out uiPos);

            float fillAmountValue = GetDashboardInputValue(clickPos, uiPos);

            m_BrightnessDashboard_CircleFront.GetComponent<Image>().DOFillAmount(fillAmountValue, m_DashboardAnimDuration);

           // Set Brightness
            DeviceUtils.SetBrightness(fillAmountValue);

        }

        private void BrightnessDashboard_CircleFront_OnPointerEnter(BaseEventData obj)
        {
            Sequence moveSequence = DOTween.Sequence();
            moveSequence.Append(m_BrightnessDashboard.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_BrightnessDashboard, -50), m_DashboardAnimDuration))
                .Join(m_BrightnessDashboard_CircleFront.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_BrightnessDashboard_CircleFront, -50), m_DashboardAnimDuration))
                .Join(m_BrightnessDashboard_Title.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_BrightnessDashboard_Title, -100), m_DashboardAnimDuration).SetDelay(m_DashboardAnimDuration * 0.8f));
        }

        private void BrightnessDashboard_CircleFront_OnPointerExit(BaseEventData obj)
        {
            Sequence moveSequence = DOTween.Sequence();
            moveSequence.Append(m_BrightnessDashboard.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_BrightnessDashboard, 0), m_DashboardAnimDuration))
                .Join(m_BrightnessDashboard_CircleFront.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_BrightnessDashboard_CircleFront, 0), m_DashboardAnimDuration))
                .Join(m_BrightnessDashboard_Title.GetComponent<RectTransform>().DOAnchorPos3D(OutVector(m_BrightnessDashboard_Title, 0), m_DashboardAnimDuration).SetDelay(m_DashboardAnimDuration * 0.8f));
        }

#endregion //Event Bind
    }
}

