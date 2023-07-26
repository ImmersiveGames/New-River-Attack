using UnityEngine;
using UnityEngine.Audio;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "RiverAttack/Enemy", order = 1)]
    [System.Serializable]
    public class EnemiesScriptable : ScriptableObject
    {
        [Header("Default Settings")]
        public new string name;
        //public LocalizationString translateName;
        public float velocity;
        public float radiusToApproach;
        public string fbScore;
        public int enemyScore;
        public Sprite spriteIcon;
        public AudioMixerGroup enemyAudioMixerGroup;
        public EnemiesSetDifficultyListSo enemiesSetDifficultyListSo;
        public bool canRespawn;
        public bool canDestruct;
        public bool isCheckInPoint;
    }
}
