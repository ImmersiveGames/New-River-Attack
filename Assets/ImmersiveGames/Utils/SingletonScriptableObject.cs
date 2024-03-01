using System.Linq;
using UnityEngine;

namespace ImmersiveGames.Utils
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance = null;

        public static T instance
        {
            get
            {
                if (_instance) return _instance;
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

                if (_instance) return _instance;
                // Se não encontrado, tenta carregar do Resources
                var assets = Resources.LoadAll<T>("");
                if (assets.Length > 0)
                {
                    _instance = assets[0];
                }
                else
                {
                    Debug.LogError($"SingletonScriptableObject<{typeof(T)}> not found in Resources.");
                }

                return _instance;
            }
        }

#if UNITY_EDITOR
        // Avisa no Editor se o SingletonScriptableObject não for encontrado
        static SingletonScriptableObject()
        {
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        }

        private static void OnBeforeAssemblyReload()
        {
            if (!instance)
            {
                Debug.LogWarning($"SingletonScriptableObject<{typeof(T)}> not found in the project.");
            }
        }
#endif
    }
}