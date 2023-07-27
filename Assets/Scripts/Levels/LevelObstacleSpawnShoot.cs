﻿using UnityEngine;

namespace RiverAttack
{
    public class LevelObstacleSpawnShoot : MonoBehaviour
    {

        [SerializeField]
        public float bulletSpeedy;
        [SerializeField]
        public float cadenceShoot;
        [SerializeField]
        public int startPool;
        [SerializeField]
        public bool playerTarget;

        [ContextMenu("LoadPrefab")]
        void LoadPrefab()
        {
            var spawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            var obstacleShoot = spawnMaster.getPrefab.GetComponent<EnemiesShoot>();
            if (obstacleShoot == null)
                return;
            bulletSpeedy = obstacleShoot.bulletSpeedy;
            cadenceShoot = obstacleShoot.cadenceShoot;
            var enemiesShoot = obstacleShoot;
            startPool = enemiesShoot.poolStart;
            playerTarget = enemiesShoot.hasTarget;
        }
    }
}
