using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(LevelObstacleSpawnMaster))]
    public class LevelObstacleSpawnMovement : ObstacleMovement
    {
        [ContextMenu("LoadPrefab")]
        void LoadPrefab()
        {
            var spawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            var om = spawnMaster.getPrefab.GetComponent<ObstacleMovement>();
            if (om == null) return;
            directions = om.moveDirection;
            freeDirection = om.moveFree;
            movementSpeed = om.movementSpeed;
            animationCurve = om.curveMovement;
            canMove = om.canMove;
        }
    }
}
