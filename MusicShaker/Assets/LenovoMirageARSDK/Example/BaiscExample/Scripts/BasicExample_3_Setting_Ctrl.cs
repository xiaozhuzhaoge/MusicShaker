using DeviceManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ximmerse.Vision;

namespace LenovoMirageARSDK.Example
{
    /// <summary>
    /// The Example of LenovoMirageARSDK
    /// Example of How to Controller The Auido and Brightness
    /// </summary>
    public class BasicExample_3_Setting_Ctrl : MonoBehaviour
    {
        #region Private Properties

        /// <summary>
        /// Get Brightness Action
        /// </summary>
        private Action<float> GetBrightnessAction;

        /// <summary>
        /// Brightness Value
        /// </summary>
        private float brightnessValue;

        /// <summary>
        /// Audio Step Value,Audio Value Between (0,15)
        /// </summary>
        private float audioStep = 1.0f/15;

        /// <summary>
        /// Brightness Step Value,Brightness Value Between (0,1)
        /// </summary>
        private float brightnessStep = 1.0f/10;

        #endregion

        #region Public Properties     

        /// <summary>
        /// Audio Add Button
        /// </summary>
        public Button AudioAddButton;

        /// <summary>
        /// Audio Minus Button
        /// </summary>
        public Button AudioMinusButton;

        /// <summary>
        /// Brightness Add Button
        /// </summary>
        public Button BrightnessAddButton;

        /// <summary>
        /// Brightness Minus Button
        /// </summary>
        public Button BrightnessMinusButton;

        /// <summary>
        /// Audio Text Value
        /// </summary>
        public Text AudioTextValue;

        /// <summary>
        /// Brightness Text Value
        /// </summary>
        public Text BrightnessTextValue;

        #endregion

        #region Attribute

        public float BrightnessValue
        {
            get
            {
                return brightnessValue;
            }

            set
            {
                brightnessValue = value;
                CheckBrightnessEnable();
            }
        }

        #endregion Attribute

        #region Unity Method

        private void Awake()
        {
            GetBrightnessAction = delegate (float screenBrightness)
            {
                Debug.Log("GetActivityBrightness " + screenBrightness);
                BrightnessValue = screenBrightness;
            };

            CheckAudioButtonEnable();
            BrightnessValue = DeviceUtils.GetBrightness();
        }

        #endregion

        #region Public Methods

        public void AudioAdd()
        {
            float audioValue = DeviceUtils.GetVolume();
            audioValue = Mathf.Clamp(audioValue + audioStep, 0, 15);
            DeviceUtils.SetVolume(audioValue);

            CheckAudioButtonEnable();
        }

        public void AudioMinus()
        {
            float audioValue = DeviceUtils.GetVolume();
            audioValue = Mathf.Clamp(audioValue - audioStep, 0, 15);
            DeviceUtils.SetVolume(audioValue);

            CheckAudioButtonEnable();
        }

        public void BightnessAdd()
        {
            BrightnessValue = Mathf.Clamp01(BrightnessValue + brightnessStep);
            DeviceUtils.SetBrightness(BrightnessValue);
            DeviceUtils.GetBrightnessThreadSafty(GetBrightnessAction);

        }

        public void BightnessMius()
        {
            BrightnessValue = Mathf.Clamp01(BrightnessValue - brightnessStep);
            DeviceUtils.SetBrightness(BrightnessValue);
            DeviceUtils.GetBrightnessThreadSafty(GetBrightnessAction);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Audio Value between (0-15)
        /// </summary>
        private void CheckAudioButtonEnable()
        {
            AudioTextValue.text = "Audio=" + DeviceUtils.GetVolume();

            if (DeviceUtils.GetVolume() <= 0)
            {
                AudioMinusButton.interactable = false;
            }
            else
            {
                AudioMinusButton.interactable = true;
            }

            if (DeviceUtils.GetVolume() >= 1)
            {
                AudioAddButton.interactable = false;
            }
            else
            {
                AudioAddButton.interactable = true;
            }
        }

        /// <summary>
        /// Brightness Value Between (0,1)
        /// </summary>
        private void CheckBrightnessEnable()
        {
            BrightnessTextValue.text = "Brightness=" + BrightnessValue.ToString();

            if (BrightnessValue <= 0)
            {
                BrightnessMinusButton.interactable = false;
            }
            else
            {
                BrightnessMinusButton.interactable = true;
            }

            if (BrightnessValue >= 1)
            {
                BrightnessAddButton.interactable = false;
            }
            else
            {
                BrightnessAddButton.interactable = true;
            }
        }

        #endregion

    }

}
