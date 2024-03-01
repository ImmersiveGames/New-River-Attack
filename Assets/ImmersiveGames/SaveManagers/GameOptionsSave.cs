using UnityEngine;
using UnityEngine.Localization;
using ImmersiveGames.Utils;

namespace ImmersiveGames.SaveManagers
{
    [CreateAssetMenu(fileName = "GameOptionsSave", menuName = "ImmersiveGames/GameOptionsSave", order = 1)]
    public class GameOptionsSave : SingletonScriptableObject<GameOptionsSave>
    {
        [Header("Options Localization")]
        public Locale startLocale;
        
        [Header("Options Sound And Music")]
        public float musicVolume;
        public float sfxVolume;
    }
}