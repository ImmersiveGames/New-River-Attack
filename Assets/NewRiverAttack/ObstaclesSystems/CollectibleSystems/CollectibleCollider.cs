using System;
using NewRiverAttack.ObstaclesSystems.Abstracts;

namespace NewRiverAttack.ObstaclesSystems.CollectibleSystems
{
    public class CollectibleCollider: ObstacleCollider
    {
        private void OnBecameInvisible()
        {
            if(ObstacleMaster.objectDefault.obstacleTypes == ObstacleTypes.Collectable)
                Destroy(gameObject, .1f);
        }
    }
}