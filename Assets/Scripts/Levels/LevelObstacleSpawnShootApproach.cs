using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(LevelObstacleSpawnShoot))]
    public class LevelObstacleSpawnShootApproach : EnemyShootApproach {

        private LevelObstacleSpawnMaster spawnMaster;
        protected override void OnEnable() { }
        private void Start() { }
        [ContextMenu("LoadPrefab")]
        private void LoadPrefab()
        {
            spawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            ObstacleDetectApproach oda = spawnMaster.GetPrefab.GetComponent<ObstacleDetectApproach>();
            if (oda != null)
            {
                radiusPlayerProximity = oda.radiusPlayerProximity;
                randomPlayerDistanceNear = oda.randomPlayerDistanceNear;
                dificultType = oda.dificultType;
                EnemyShootApproach enemyOda = (EnemyShootApproach)oda;
                timeToCheck = enemyOda.timeToCheck;
                //enDifList = oda.enDifList;
            }
        }
    }
}

