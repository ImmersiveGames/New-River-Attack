using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class LevelObstacleSpawnShoot : MonoBehaviour
    {

        [SerializeField]
        public float bulletSpeedy;
        [SerializeField]
        public float cadencyShoot;
        [SerializeField]
        public int startPool;
        [SerializeField]
        public bool playerTarget;

        [ContextMenu("LoadPrefab")]
        private void LoadPrefab()
        {
            var spawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            var obstacleShoot = spawnMaster.getPrefab.GetComponent<ObstacleShoot>();
            if (obstacleShoot == null)
                return;
            bulletSpeedy = obstacleShoot.bulletSpeedy;
            cadencyShoot = obstacleShoot.cadencyShoot;
            var enemiesShoot = (EnemiesShoot)obstacleShoot;
            startPool = enemiesShoot.poolStart;
            playerTarget = enemiesShoot.playerTarget;
        }
    }
}
