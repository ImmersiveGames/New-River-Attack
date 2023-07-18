using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(LevelObstacleSpawnShoot))]
    public class LevelObstacleSpawnShootApproach : EnemiesShootApproach
    {

        private LevelObstacleSpawnMaster spawnMaster;
        protected override void OnEnable() { }
        private void Start() { }
        [ContextMenu("LoadPrefab")]
        private void LoadPrefab()
        {
            spawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            ObstacleDetectApproach oda = spawnMaster.getPrefab.GetComponent<ObstacleDetectApproach>();
            if (oda != null)
            {
                radiusPlayerProximity = oda.radiusPlayerProximity;
                randomPlayerDistanceNear = oda.randomPlayerDistanceNear;
                difficultType = oda.difficultType;
                EnemiesShootApproach enemiesOda = (EnemiesShootApproach)oda;
                timeToCheck = enemiesOda.timeToCheck;
                //enemyDifficultyList = oda.enemyDifficultyList;
            }
        }
    }
}
