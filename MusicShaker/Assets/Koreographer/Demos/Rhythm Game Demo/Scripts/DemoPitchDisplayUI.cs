//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2016 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace SonicBloom.Koreo.Demos
{
	[AddComponentMenu("Koreographer/Demos/Rhythm Game/Demo Pitch Display UI")]
	public class DemoPitchDisplayUI : MonoBehaviour
	{
		[Tooltip("The AudioSource component to track/update for pitch settings.")]
		public AudioSource audioCom;

		[Tooltip("The Text Component that will display the Pitch number.")]
		public Text readoutText;

		[Tooltip("The Slider that controls the Pitch number.")]
		public Slider slider;

		void Start()
		{
			// Ensure the slider and the readout are properly in sync with the AudioSource on Start!
			float pitch = audioCom.pitch;

			slider.value = pitch;
			SetNewPitch(slider.value);
		}

		/// <summary>
		/// A simple function to respond to Event Triggers with the UI system.  This takes the
		/// new pitch value from the slider and formats it for the Text Component.  It also sets
		/// the value to the specified <c>AudioSource</c> instance.
		/// </summary>
		/// <param name="newPitch">The new pitch value to use.</param>
		public void SetNewPitch(float newPitch)
		{
			audioCom.pitch = newPitch;

			readoutText.text = newPitch.ToString("0.####") + "x";
		}
	}
}
