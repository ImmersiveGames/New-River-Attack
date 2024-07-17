using UnityEngine;

namespace ImmersiveGames.Utils
{
    public class HardWereVibration
    {
        private long _timeVibration;
        public static void Vibration(long timeVibration)
        {
            if (Application.platform == RuntimePlatform.Android && SystemInfo.supportsVibration)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                ToolsAndroid.Vibrate(timeVibration);
                Handheld.Vibrate();
#endif
            }
        }
    }
}