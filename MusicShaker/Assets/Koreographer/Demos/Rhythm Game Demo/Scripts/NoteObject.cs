//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2016 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;

namespace SonicBloom.Koreo.Demos
{
	[AddComponentMenu("Koreographer/Demos/Rhythm Game/Note Object")]
	public class NoteObject : MonoBehaviour
	{
		#region Fields

		[Tooltip("The visual to use for this Note Object.")]
		public SpriteRenderer visuals;

		// If active, the KoreographyEvent that this Note Object wraps.  Contains the relevant timing information.
		KoreographyEvent trackedEvent;

		// If active, the Lane Controller that this Note Object is contained by.
		LaneController laneController;

		// If active, the Rhythm Game Controller that controls the game this Note Object is found within.
		RhythmGameController gameController;

		#endregion
		#region Static Methods
		
		// Unclamped Lerp.  Same as Vector3.Lerp without the [0.0-1.0] clamping.
		static Vector3 Lerp(Vector3 from, Vector3 to, float t)
		{
			return new Vector3 (from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
		}

		#endregion
		#region Methods

		// Prepares the Note Object for use.
		public void Initialize(KoreographyEvent evt, Color color, LaneController laneCont, RhythmGameController gameCont)
		{
			trackedEvent = evt;
			visuals.color = color;
			laneController = laneCont;
			gameController = gameCont;

			UpdatePosition();
		}

		// Resets the Note Object to its default state.
		void Reset()
		{
			trackedEvent = null;
			laneController = null;
			gameController = null;
		}

		void Update()
		{
			UpdateHeight();

			UpdatePosition();

			if (transform.position.y <= laneController.DespawnY)
			{
				gameController.ReturnNoteObjectToPool(this);
				Reset();
			}
		}

		// Updates the height of the Note Object.  This is relative to the speed at which the notes fall and 
		//  the specified Hit Window range.
		void UpdateHeight()
		{
			float baseUnitHeight = visuals.sprite.rect.height / visuals.sprite.pixelsPerUnit;
			float targetUnitHeight = gameController.WindowSizeInUnits * 2f;	// Double it for before/after.

			Vector3 scale = transform.localScale;
			scale.y = targetUnitHeight / baseUnitHeight;	
			transform.localScale = scale;
		}

		// Updates the position of the Note Object along the Lane based on current audio position.
		void UpdatePosition()
		{
			// Get the number of samples we traverse given the current speed in Units-Per-Second.
			float samplesPerUnit = gameController.SampleRate / gameController.noteSpeed;

			// Our position is offset by the distance from the target in world coordinates.  This depends on
			//  the distance from "perfect time" in samples (the time of the Koreography Event!).
			Vector3 pos = laneController.TargetPosition;
			pos.y -= (gameController.DelayedSampleTime - trackedEvent.StartSample) / samplesPerUnit;
			transform.position = pos;
		}

		// Checks to see if the Note Object is currently hittable or not based on current audio sample
		//  position and the configured hit window width in samples (this window used during checks for both
		//  before/after the specific sample time of the Note Object).
		public bool IsNoteHittable()
		{
			int noteTime = trackedEvent.StartSample;
			int curTime = gameController.DelayedSampleTime;
			int hitWindow = gameController.HitWindowSampleWidth;

			return (Mathf.Abs(noteTime - curTime) <= hitWindow);
		}

		// Checks to see if the note is no longer hittable based on the configured hit window width in
		//  samples.
		public bool IsNoteMissed()
		{
			bool bMissed = true;

			if (enabled)
			{
				int noteTime = trackedEvent.StartSample;
				int curTime = gameController.DelayedSampleTime;
				int hitWindow = gameController.HitWindowSampleWidth;

				bMissed = (curTime - noteTime > hitWindow);
			}
			
			return bMissed;
		}

		// Returns this Note Object to the pool which is controlled by the Rhythm Game Controller.  This
		//  helps reduce runtime allocations.
		void ReturnToPool()
		{
			gameController.ReturnNoteObjectToPool(this);
			Reset();
		}

		// Performs actions when the Note Object is hit.
		public void OnHit()
		{
			ReturnToPool();
		}

		// Performs actions when the Note Object is cleared.
		public void OnClear()
		{
			ReturnToPool();
		}

		#endregion
	}
}
