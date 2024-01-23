using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "RiverAttack/GameSettings", order = 0)]
    public class GameSettings : SingletonScriptableObject<GameSettings>
    {
        [Header("PLayer Start Settings")]
        public int maxBombs;
        public int startBombs;
        public int maxFuel;
        public int startFuel;
        public int maxLives;
        public int startLives;
        
        [Header("Options Preferences")]
        public float musicVolume;
        public float sfxVolume;
        
        public Locale startLocale;
        [Header("Game play Settings")]
        public int multiplyScoreForLives;
        
        [Header("Graphics Settings")]
        public int indexQuality;
        public Vector2Int actualResolution;
        public FullScreenMode actualFullScreenMode;
        public int indexFrameRate; 
    }
}
