//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2016 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace SonicBloom.Koreo.Demos
{
	[AddComponentMenu("Koreographer/Demos/Rhythm Game/Lane Controller")]
	public class LaneController : MonoBehaviour
	{
		#region Fields

		[Tooltip("The Color of Note Objects and Buttons in this Lane.")]
		public Color color = Color.blue;

		[Tooltip("A reference to the visuals for the \"target\" location.")]
		public SpriteRenderer targetVisuals;

		[Tooltip("The Keyboard Button used by this lane.")]
		public KeyCode keyboardButton;

		[Tooltip("A list of Payload strings that Koreography Events will contain for this Lane.")]
		public List<string> matchedPayloads = new List<string>();

		// The list that will contain all events in this lane.  These are added by the Rhythm Game Controller.
		List<KoreographyEvent> laneEvents = new List<KoreographyEvent>();

		// A Queue that contains all of the Note Objects currently active (on-screen) within this lane.  Input and
		//  lifetime validity checks are tracked with operations on this Queue.
		Queue<NoteObject> trackedNotes = new Queue<NoteObject>();

		// A reference to the Rythm Game Controller.  Provides access to the NoteObject pool and other parameters.
		RhythmGameController gameController;

		// Lifetime boundaries.  This game goes from the top of the screen to the bottom.
		float spawnY = 0f;
		float despawnY = 0f;

		// Index of the next event to check for spawn timing in this lane.
		int pendingEventIdx = 0;

		// Feedback Scales used for resizing the buttons on press.
		Vector3 defaultScale;
		float scaleNormal = 1f;
		float scalePress = 1.4f;
		float scaleHold = 1.2f;

		#endregion
		#region Properties

		// The position at which new Note Objects should spawn.
		public Vector3 SpawnPosition
		{
			get
			{
				return new Vector3(transform.position.x, spawnY);
			}
		}

		// The position at which the timing target exists.
		public Vector3 TargetPosition
		{
			get
			{
				return new Vector3(transform.position.x, transform.position.y);
			}
		}

		// The position at which Note Objects should despawn and return to the pool.
		public float DespawnY
		{
			get
			{
				return despawnY;
			}
		}

		#endregion
		#region Methods

		public void Initialize(RhythmGameController controller)
		{
			gameController = controller;
		}

		// This method controls cleanup, resetting the internals to a fresh state.
		public void Restart()
		{
			pendingEventIdx = 0;

			// Clear out the tracked notes.
			int numToClear = trackedNotes.Count;
			for (int i = 0; i < numToClear; ++i)
			{
				trackedNotes.Dequeue().OnClear();
			}
		}

		void Start()
		{
			// Get the vertical bounds of the camera.  Offset by a bit to allow for offscreen spawning/removal.
			float cameraOffsetZ = -Camera.main.transform.position.z;
			spawnY = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, cameraOffsetZ)).y + 1f;
			despawnY = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, cameraOffsetZ)).y - 1f;

			// Update our visual color.
			targetVisuals.color = color;

			// Capture the default scale set in the Inspector.
			defaultScale = targetVisuals.transform.localScale;
		}

		void Update()
		{
			// Clear out invalid entries.
			while (trackedNotes.Count > 0 && trackedNotes.Peek().IsNoteMissed())
			{
				trackedNotes.Dequeue();
			}

			// Check for new spawns.
			CheckSpawnNext();

			// Check for input.  Note that touch controls are handled by the Event System, which is all
			//  configured within the Inspector on the buttons themselves, using the same functions as
			//  what is found here.  Touch input does not have a built-in concept of "Held", so it is not
			//  currently supported.
			if (Input.GetKeyDown(keyboardButton))
			{
				CheckNoteHit();
				SetScalePress();
			}
			else if (Input.GetKey(keyboardButton))
			{
				SetScaleHold();
			}
			else if (Input.GetKeyUp(keyboardButton))
			{
				SetScaleDefault();
			}
		}

		// Adjusts the scale with a multiplier against the default scale.
		void AdjustScale(float multiplier)
		{
			targetVisuals.transform.localScale = defaultScale * multiplier;
		}

		// Uses the Target position and the current Note Object speed to determine the audio sample
		//  "position" of the spawn location.  This value is relative to the audio sample position at
		//  the Target position (the "now" time).
		int GetSpawnSampleOffset()
		{
			// Given the current speed, what is the sample offset of our current.
			float spawnDistToTarget = spawnY - transform.position.y;
			
			// At the current speed, what is the time to the location?
			double spawnSecsToTarget = (double)spawnDistToTarget / (double)gameController.noteSpeed;
			
			// Figure out the samples to the target.
			return (int)(spawnSecsToTarget * gameController.SampleRate);
		}

		// Checks if a Note Object is hit.  If one is, it will perform the Hit and remove the object
		//  from the trackedNotes Queue.
		public void CheckNoteHit()
		{
			// Always check only the first event as we clear out missed entries before.
			if (trackedNotes.Count > 0 && trackedNotes.Peek().IsNoteHittable())
			{
				NoteObject hitNote = trackedNotes.Dequeue();

				hitNote.OnHit();
			}
		}

		// Checks if the next Note Object should be spawned.  If so, it will spawn the Note Object and
		//  add it to the trackedNotes Queue.
		void CheckSpawnNext()
		{
			int samplesToTarget = GetSpawnSampleOffset();
			
			int currentTime = gameController.DelayedSampleTime;
			
			// Spawn for all events within range.
			while (pendingEventIdx < laneEvents.Count &&
			       laneEvents[pendingEventIdx].StartSample < currentTime + samplesToTarget)
			{
				KoreographyEvent evt = laneEvents[pendingEventIdx];
				
				NoteObject newObj = gameController.GetFreshNoteObject();
				newObj.Initialize(evt, color, this, gameController);

				trackedNotes.Enqueue(newObj);
				
				pendingEventIdx++;
			}
		}

		// Adds a KoreographyEvent to the Lane.  The KoreographyEvent contains the timing information
		//  that defines when a Note Object should appear on screen.
		public void AddEventToLane(KoreographyEvent evt)
		{
			laneEvents.Add(evt);
		}

		// Checks to see if the string value passed in matches any of the configured values specified
		//  in the matchedPayloads List.
		public bool DoesMatchPayload(string payload)
		{
			bool bMatched = false;

			for (int i = 0; i < matchedPayloads.Count; ++i)
			{
				if (payload == matchedPayloads[i])
				{
					bMatched = true;
					break;
				}
			}

			return bMatched;
		}

		// Sets the Target scale to the original default scale.
		public void SetScaleDefault()
		{
			AdjustScale(scaleNormal);
		}

		// Sets the Target scale to the specified "initially pressed" scale.
		public void SetScalePress()
		{
			AdjustScale(scalePress);
		}

		// Sets the Target scale to the specified "continuously held" scale.
		public void SetScaleHold()
		{
			AdjustScale(scaleHold);
		}
		
		#endregion
	}
}
