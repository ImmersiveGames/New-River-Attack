using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesCollider : ObstacleCollider
    {
        internal override void OnTriggerEnter(Collider other)
        {
            if (other == null || !ObstacleMaster.ObjectIsReady) return;
            base.OnTriggerEnter(other);
            ComponentToKill(other.GetComponentInParent<PlayerMaster>(), EnumCollisionType.Collider);
        }
    }
}