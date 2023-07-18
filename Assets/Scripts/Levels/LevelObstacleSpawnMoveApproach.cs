using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(LevelObstacleSpawnMaster))]
    public class LevelObstacleSpawnMoveApproach : ObstacleMoveByApproach
    {
        private LevelObstacleSpawnMaster m_SpawnMaster;
        protected override void SetInitialReferences() { }
        [ContextMenu("LoadPrefab")]
        private void LoadPrefab()
        {
            m_SpawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            var obstacleMoveByApproach = m_SpawnMaster.getPrefab.GetComponent<ObstacleMoveByApproach>();
            if (obstacleMoveByApproach != null)
            {
                radiusPlayerProximity = obstacleMoveByApproach.radiusPlayerProximity;
                timeToCheck = obstacleMoveByApproach.timeToCheck;
                difficultType = obstacleMoveByApproach.difficultType;
                randomPlayerDistanceNear = obstacleMoveByApproach.randomPlayerDistanceNear;
                enemyDifficultyList = obstacleMoveByApproach.enemyDifficultyList;
            }
        }
    }
}
