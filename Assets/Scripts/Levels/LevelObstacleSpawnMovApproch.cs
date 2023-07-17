using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(LevelObstacleSpawnMaster))]
    public class LevelObstacleSpawnMovApproch : ObstacleMoveByApproach
    {
        private LevelObstacleSpawnMaster spawnMaster;
        private void OnEnable(){}
        protected override void SetInitialReferences(){}
        [ContextMenu("LoadPrefab")]
        private void LoadPrefab()
        {
            spawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            ObstacleMoveByApproach omba = spawnMaster.GetPrefab.GetComponent<ObstacleMoveByApproach>();
            if (omba != null)
            {
                radiusPlayerProximity = omba.RadiusPlayerProximity;
                timeToCheck = omba.TimeToCheck;
                dificultType = omba.dificultType;
                randomPlayerDistanceNear = omba.RandomPlayerDistanceNear;
                //enDifList = omba.enDifList;
            }
        }
        private void Update() { }
        private void OnDisable() { }
    }
}

