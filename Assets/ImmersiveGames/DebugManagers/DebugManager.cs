using UnityEngine;

namespace ImmersiveGames.DebugManagers
{
    public static class DebugManager
    {
        public enum DebugLevel
        {
            None,
            Logs,
            LogsAndWarnings,
            All
        }

        public static DebugLevel debugLevel = DebugLevel.All;

        public static void Log(string message)
        {
            if (debugLevel is DebugLevel.Logs or DebugLevel.LogsAndWarnings or DebugLevel.All)
            {
                Debug.Log(message);
            }
        }

        public static void LogWarning(string message)
        {
            if (debugLevel is DebugLevel.LogsAndWarnings or DebugLevel.All && debugLevel != DebugLevel.None)
            {
                Debug.LogWarning(message);
            }
        }

        public static void LogError(string message)
        {
            if (debugLevel == DebugLevel.All && debugLevel != DebugLevel.None)
            {
                Debug.LogError(message);
            }
        }
    }

}