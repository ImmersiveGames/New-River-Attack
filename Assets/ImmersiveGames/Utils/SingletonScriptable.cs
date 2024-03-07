using UnityEngine;

namespace ImmersiveGames.Utils
{
    public abstract class SingletonScriptable<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        private static string _resourcePath = "SavesSO/GameOptionsSave";  // Defina o caminho padrão aqui

        public static T instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = Resources.Load<T>(_resourcePath);
                if (_instance == null)
                {
                    Debug.LogError($"SingletonScriptable<{typeof(T)}> not found in Resources at path {_resourcePath}.");
                }
                return _instance;
            }
        }

        public static void SetResourcePath(string newPath)
        {
            _resourcePath = newPath;
        }

        protected string GetResourcePath() => _resourcePath;
    }
}

