namespace DeviceManager {
	using System;
	using UnityEngine;

	public class AndroidDialogBuilder {
		private AndroidJavaObject activity;

		private AndroidJavaObject builder;

		//public delegate void OnPositiveClickListener(DialogInterface dialog);
		//public delegate void OnNegativeClickListener(DialogInterface dialog);
		private DialogOnClickListener onPositiveClick;
		private DialogOnClickListener onNegativeClick;
		//private OnPositiveClickListener onConfirmClick;
		//private OnNegativeClickListener onCancelClick;

		public AndroidDialogBuilder() {
			activity = AndroidUtil.GetActivity();
			builder = new AndroidJavaObject("android.app.AlertDialog$Builder", activity, 3);
		}

		public AndroidDialogBuilder SetTitle(string title) {
			if (title != null) {
				builder.Call<AndroidJavaObject>("setTitle", title);
			}
			return this;
		}

		public AndroidDialogBuilder SetMessage(string message) {
			if (message != null) {
				builder.Call<AndroidJavaObject>("setMessage", message);
			}
			return this;
		}

		public AndroidDialogBuilder SetPositiveButton(string name, Action onClick) {
			if (name == null || onClick == null) return this;
			onPositiveClick = new DialogOnClickListener();
			onPositiveClick.onClickDelegate = (AndroidJavaObject dialog, int which) => {
				if (onClick != null) {
					onClick();
				}
			};
			builder.Call<AndroidJavaObject>("setPositiveButton", name, onPositiveClick);
			return this;
		}

		public AndroidDialogBuilder SetNegativeButton(string name, Action onClick) {
			if (name == null || onClick == null) return this;
			onNegativeClick = new DialogOnClickListener();
			onNegativeClick.onClickDelegate = (AndroidJavaObject dialog, int which) => {
				if (onClick != null) {
					onClick();
				}
			};
			builder = builder.Call<AndroidJavaObject>("setNegativeButton", name, onNegativeClick);
			return this;
		}
		public AndroidDialogBuilder Create() {
			builder = builder.Call<AndroidJavaObject>("create");
			return this;
		}

		public void Show() {
			builder.Call("show");
		}
	}

	public class DialogInterface {
		private AndroidJavaObject dialog;
		public DialogInterface(AndroidJavaObject dialog) {
			this.dialog = dialog;
		}
		public void Cancel() {
			if (dialog != null) {
				dialog.Call("cancel");
			}
		}
	}
}