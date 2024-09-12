using ImmersiveGames.DebugManagers;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public class SingletonScriptable<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        // ReSharper disable once StaticMemberInGenericType
        private static string _resourcePath = "SavesSO/GameOptionsSave";  // Defina o caminho padrão aqui

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = Resources.Load<T>(_resourcePath);
                if (_instance == null)
                {
                    DebugManager.LogError<T>($"SingletonScriptable<{typeof(T)}> not found in Resources at path {_resourcePath}.");
                }
                return _instance;
            }
        }

        protected static void SetResourcePath(string newPath)
        {
            _resourcePath = newPath;
        }

        protected string GetResourcePath() => _resourcePath;
    }
}

