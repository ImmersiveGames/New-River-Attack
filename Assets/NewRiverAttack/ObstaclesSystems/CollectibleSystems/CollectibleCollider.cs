using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.CollectibleSystems
{
    public class CollectibleCollider: ObstacleCollider
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            ObstacleMaster.EventObstacleHit += ColliderHit;
            GamePlayManager.EventGameReload += ColliderReload;
            GamePlayManager.EventGameRestart += ColliderReload;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ObstacleMaster.EventObstacleHit -= ColliderHit;
            GamePlayManager.EventGameReload -= ColliderReload;
            GamePlayManager.EventGameRestart -= ColliderReload;
        }

        private void OnBecameInvisible()
        {
            if (ObstacleMaster.objectDefault.obstacleTypes != ObstacleTypes.Collectable) return;
            ColliderOn(true);
            Destroy(gameObject, .1f);

        }
        
        private void ColliderReload()
        {
            ColliderOn(true);
        }
        
        private void ColliderHit(PlayerMaster playerMaster)
        {
            ColliderOn(false);
        }

        private void ColliderOn(bool active)
        {
            var colliders = GetComponentsInChildren<Collider>();
            if(colliders.Length <=0) return;
            foreach (var t in colliders)
            {
                t.enabled = active;
            }
        }
    }
}