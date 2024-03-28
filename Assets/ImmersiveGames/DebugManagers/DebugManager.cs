using UnityEngine;
using System;
using System.Collections.Generic;

namespace ImmersiveGames.DebugManagers
{
    public static class DebugManager
    {
        public enum DebugLevels
        {
            None,
            Logs,
            LogsAndWarnings,
            All
        }

        private static DebugLevels _globalDebugLevel = DebugLevels.All;
        private static readonly Dictionary<Type, DebugLevels> ScriptDebugLevels = new Dictionary<Type, DebugLevels>();

        public static void SetGlobalDebugLevel(DebugLevels debugLevel)
        {
            _globalDebugLevel = debugLevel;
        }

        public static void SetScriptDebugLevel<T>(DebugLevels debugLevel)
        {
            var scriptType = typeof(T);
            ScriptDebugLevels[scriptType] = debugLevel;
        }

        public static void Log<T>(string message)
        {
            var scriptType = typeof(T);
            if (ScriptDebugLevels.ContainsKey(scriptType) && ScriptDebugLevels[scriptType] >= _globalDebugLevel)
            {
                Debug.Log($"[{scriptType.Name}] {message}");
            }
        }

        public static void LogWarning<T>(string message)
        {
            var scriptType = typeof(T);
            if (ScriptDebugLevels.ContainsKey(scriptType) && ScriptDebugLevels[scriptType] >= _globalDebugLevel)
            {
                Debug.LogWarning($"[{scriptType.Name}] {message}");
            }
        }

        public static void LogError<T>(string message)
        {
            var scriptType = typeof(T);
            if (ScriptDebugLevels.ContainsKey(scriptType) && ScriptDebugLevels[scriptType] >= _globalDebugLevel)
            {
                Debug.LogError($"[{scriptType.Name}] {message}");
            }
        }
    }
}