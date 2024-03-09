#if UNITY_EDITOR
using UnityEditor;
#endif

using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.Localization;

namespace ImmersiveGames.SaveManagers
{
    [CreateAssetMenu(fileName = "GameOptionsSave", menuName = "ImmersiveGames/GameOptionsSave", order = 1)]
    public class GameOptionsSave : SingletonScriptable<GameOptionsSave>
    {
        [Header("Options Localization")]
        public Locale startLocale;
        
        [Header("Options Sound And Music")]
        public float musicVolume;
        public float sfxVolume;

        private new static string GetResourcePath() => "SavesSO/GameOptionsSave";

#if UNITY_EDITOR
        [MenuItem("ImmersiveGames/Load GameOptionsSave")]
        private static void LoadGameOptionsSave()
        {
            // Editor-only method to load GameOptionsSave
            var gameOptionsSave = Resources.Load<GameOptionsSave>(GetResourcePath());

            if (gameOptionsSave != null)
            {
                DebugManager.Log("GameOptionsSave loaded in the Editor.");
            }
            else
            {
                DebugManager.LogError("Failed to load GameOptionsSave in the Editor.");
            }
        }
#endif
    }
}