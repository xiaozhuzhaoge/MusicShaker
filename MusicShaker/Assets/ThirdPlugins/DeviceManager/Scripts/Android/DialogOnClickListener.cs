namespace DeviceManager {

	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;
	public class DialogOnClickListener : AndroidJavaProxy {
		public delegate void OnClickDelegate(AndroidJavaObject dialog, int which);

		public OnClickDelegate onClickDelegate;

		public DialogOnClickListener() : base("android.content.DialogInterface$OnClickListener") {

		}

		public void onClick(AndroidJavaObject dialog, int which) {
			if (onClickDelegate != null) {
				onClickDelegate(dialog, which);
			}
		}
	}
}