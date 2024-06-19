using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossCollider : ObstacleCollider
    {
        internal override void OnTriggerEnter(Collider other)
        {
            if (other == null || !ObstacleMaster.ObjectIsReady) return;
            
            ComponentToKill(other.GetComponentInParent<PlayerMaster>(), EnumCollisionType.Collider);
            base.OnTriggerEnter(other);
        }
    }
}