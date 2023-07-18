using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public string fbScore;
        public int enemyScore;
        public Sprite spriteIcon;
        public AudioMixerGroup enemyAudioMixerGroup;
        public DifficultyList enemiesDifficulty;
        public bool canFlip;
        public bool canRespawn;
        public bool canDestruct;
        public bool isCheckInPoint;

        public string getName
        {
            get
            {
                return this.name;
                // Traduzir o nome
            }
        }
    }
}

