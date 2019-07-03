//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2016 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace SonicBloom.Koreo.Demos
{
	[AddComponentMenu("Koreographer/Demos/Rhythm Game/Demo Delay Display UI")]
	public class DemoDelayDisplayUI : MonoBehaviour
	{
		[Tooltip("The Text Component that will display the Event Delay number.")]
		public Text readoutText;

		[Tooltip("The Slider that controls the Event Delay number.")]
		public Slider slider;

		void Start()
		{
			// Ensure the slider and the readout are properly in sync with Koreographer on Start!
			float delayTime = Koreographer.Instance.EventDelayInSeconds;

			slider.value = delayTime;
			SetNewDelay(slider.value);
		}

		/// <summary>
		/// A simple function to respond to Event Triggers with the UI system.  This takes the
		/// new float value from the slider and formats it for the Text Component.  It also sets
		/// the value to the <c>Koreographer</c> singleton instance.
		/// </summary>
		/// <param name="newDelay">The new delay value to use.</param>
		public void SetNewDelay(float newDelay)
		{
			Koreographer.Instance.EventDelayInSeconds = newDelay;

			readoutText.text = newDelay.ToString("0.####") + "s";
		}
	}
}
