using UnityEngine;

namespace RiverAttack
{
    public class EnemyMoveByApproach : ObstacleMoveByApproach
    {
        protected override void ApproachPlayer()
        {       
            playerDistance = GetPlayerDistance();
            if (randomPlayerDistanceNear.maxValue > 0)
                playerDistance = rangePatrol;
            var colliders = Physics.OverlapSphere(transform.position, playerDistance, GameSettings.instance.layerPlayer);
            if (colliders.Length > 0 && radiusPlayerProximity > 0)
                obstacleMovement.canMove = true;
        }
    }
}
