﻿using UnityEngine;

namespace RiverAttack
{
    
    [RequireComponent(typeof(LevelObstacleSpawnMaster))]
    public class LevelObstacleSpawnMovement : ObstacleMovement
    {
        private void OnEnable() { }
        private void SetInitialReferences() { }
        private void Update() { }
        private void OnDisable() { }

        [ContextMenu("LoadPrefab")]
        private void LoadPrefab()
        {
            LevelObstacleSpawnMaster spawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            ObstacleMovement om = spawnMaster.getPrefab.GetComponent<ObstacleMovement>();
            if (om != null)
            {
                moveDirection = om.MoveDirection;
                freeDirection = om.FreeDirection;
                movementSpeed = om.MovementSpeed;
                curveMoviment = om.CurveMoviment;
                canMove = om.canMove;
            }
        }
    }
}
