using UnityEngine;
using UnityEngine.Audio;
using Utils;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "RiverAttack/Enemy", order = 1)]
    [System.Serializable]
    public class EnemiesScriptable : ScriptableObject
    {
        [Header("Default Settings")]
        public new string name;
        public new string namePT_BR;
        public int enemyScore;
        public Sprite spriteIcon;
        public AudioMixerGroup enemyAudioMixerGroup;
        [Header("Drop Items")]
        public ListDropItems listDropItems;
        [Header("Difficulty Set")]
        public EnemiesSetDifficultyListSo enemiesSetDifficultyListSo;
        [Header("Game Behaviors")]
        public bool canRespawn;
        public bool canDestruct;
        public bool isCheckInPoint;
    }
}
