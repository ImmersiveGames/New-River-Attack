using UnityEngine;

namespace Utils
{
    public static class ToolsAndroid
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private static readonly AndroidJavaObject Vibrator =
            new AndroidJavaClass("com.unity3d.player.UnityPlayer")        // Get the Unity Player.
                .GetStatic<AndroidJavaObject>("currentActivity")          // Get the Current Activity from the Unity Player.
                .Call<AndroidJavaObject>("getSystemService", "vibrator"); // Then get the Vibration Service from the Current Activity.

        public static void Vibrate(long milliseconds)
        {
            Vibrator.Call("vibrate", milliseconds);
        }

        public static void Vibrate(long[] pattern, int repeat)
        {
            Vibrator.Call("vibrate", pattern, repeat);
        }
#endif
    }
}
