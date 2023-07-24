using UnityEngine;

namespace RiverAttack
{
    public class EnemiesMoveByApproach : ObstacleMoveByApproach
    {
        protected override void ApproachPlayer()
        {
            playerDistance = GetPlayerDistance();
            if (randomPlayerDistanceNear.y > 0)
                playerDistance = rangePatrol;
            // ReSharper disable once Unity.PreferNonAllocApi
            var colliders = Physics.OverlapSphere(transform.position, playerDistance, GameManager.instance.layerPlayer);
            if (colliders == null) return;
            Debug.Log("Player Radius Prox: " + radiusPlayerProximity);
            foreach (var col in colliders)
            {
                Debug.Log("Colidiu: "+ col);
            }
            if (colliders.Length > 0 && radiusPlayerProximity > 0)
                    obstacleMovement.canMove = true;
        }
    }
}
