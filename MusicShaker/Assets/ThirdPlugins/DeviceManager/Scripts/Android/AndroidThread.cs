namespace DeviceManager {
	using System.Collections.Generic;
	using System.Collections;
	using System;
	using UnityEngine;

	public class AndroidThread {
		public static void RunOnNewThread(Run run) {
			AndroidJavaObject thread = new AndroidJavaObject("java.lang.Thread", new AndroidRunnable(run));
			thread.Call("start");
		}

		public static void Sleep(long millis) {
			new AndroidJavaClass("java.lang.Thread").CallStatic("sleep", millis);
		}

	}
}