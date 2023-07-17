using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class LevelObstacleSpawnShoot : MonoBehaviour {

        [SerializeField]
        public float bulletSpeedy;
        [SerializeField]
        public float cadencyShoot;
        [SerializeField]
        public int startpool;
        [SerializeField]
        public bool playerTarget;

        [ContextMenu("LoadPrefab")]
        private void LoadPrefab()
        {
            LevelObstacleSpawnMaster spawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            ObstacleShoot oshoot = spawnMaster.GetPrefab.GetComponent<ObstacleShoot>();
            if (oshoot != null)
            {
                this.bulletSpeedy = oshoot.bulletSpeedy;
                this.cadencyShoot = oshoot.cadencyShoot;
                EnemyShoot enemyShoot = (EnemyShoot)oshoot;
                this.startpool = enemyShoot.startpool;
                this.playerTarget = enemyShoot.playerTarget;
            }
        }
    }
}

