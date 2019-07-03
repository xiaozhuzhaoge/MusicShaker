namespace DeviceManager
{
    using UnityEngine;
    public class AndroidAudioManager : AndroidJavaProxy
    {
        /* The default audio stream */
        public const int STREAM_DEFAULT = -1;
        /* The audio stream for phone calls */
        public const int STREAM_VOICE_CALL = 0;
        /* The audio stream for system sounds */
        public const int STREAM_SYSTEM = 1;
        /* The audio stream for the phone ring and message alerts */
        public const int STREAM_RING = 2;
        /* The audio stream for music playback */
        public const int STREAM_MUSIC = 3;
        /* The audio stream for alarms */
        public const int STREAM_ALARM = 4;
        /* The audio stream for notifications */
        public const int STREAM_NOTIFICATION = 5;
        /* @hide The audio stream for phone calls when connected on bluetooth */
        public const int STREAM_BLUETOOTH_SCO = 6;
        /* @hide The audio stream for enforced system sounds in certain countries (e.g camera in Japan) */
        public const int STREAM_SYSTEM_ENFORCED = 7;
        /* @hide The audio stream for DTMF tones */
        public const int STREAM_DTMF = 8;
        /* @hide The audio stream for text to speech (TTS) */
        public const int STREAM_TTS = 9;
        private static AndroidJavaObject s_AudioManager;

        public AndroidAudioManager() : base("android.media.AudioManager")
        {
        }

        // MAX STREAM_MUSIC Volume is 15
        public static int GetStreamMaxVolume(int streamType)
        {
            return GetAudioManager().Call<int>("getStreamMaxVolume", streamType);
        }

        public static void SetStreamVolume(int streamType, int index, int flags)
        {
            GetAudioManager().Call("setStreamVolume", streamType, index, flags);
        }

        public static int GetStreamVolume(int streamType)
        {
            return GetAudioManager().Call<int>("getStreamVolume", streamType);
        }

        private static AndroidJavaObject GetAudioManager()
        {
            if (s_AudioManager == null)
            {
                s_AudioManager = AndroidUtil.GetActivity().Call<AndroidJavaObject>("getSystemService", "audio");
            }
            return s_AudioManager;
        }
    }
}
