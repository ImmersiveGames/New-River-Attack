using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems
{
    [System.Serializable]
    public struct EnemyDropData
    {
        public GameObject prefabPowerUp;
        [Range(1,10)]
        public int dropChance;
    }
}