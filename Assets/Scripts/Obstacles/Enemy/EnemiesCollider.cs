using UnityEngine;

namespace RiverAttack
{
    public class EnemiesCollider : ObstacleColliders
    {

        /*
        #region UNITY METHODS
        protected override void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponentInParent<PlayerMaster>() && !collision.GetComponent<Bullets>() || !obstacleMaster.enemy.canDestruct)
                return;
            if (collision.GetComponent<BulletEnemy>()) return;
            HitThis(collision);
        }
        #endregion
        
        protected override void HitThis(Collider collision)
        {
            obstacleMaster.isDestroyed = true;
            var playerMaster = WhoHit(collision);
            ColliderOff();
            // Quem desativa o rander é o animation de explosão
            obstacleMaster.CallEventDestroyEnemy(playerMaster);
            ShouldSavePoint();
            playerMaster.CallEventPlayerMasterCollider();
            //ShouldCompleteMission();
        }*/
        
    }
}
