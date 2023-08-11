using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class CollectiblesCollider : ObstacleColliders
    {
        /*CollectiblesMaster m_CollectiblesMaster;
        CollectibleScriptable m_Collectible;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            m_CollectiblesMaster.CollectibleEvent += DisableGraphic;
        }
        protected override void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponent<BulletPlayer>() != null && m_CollectiblesMaster.enemy.canDestruct)
            {
                HitThis(collision);
                return;
            }
            var playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (playerMaster)
            {
                CollectThis(playerMaster);
            }
        }
        void OnDisable()
        {
            m_CollectiblesMaster.CollectibleEvent -= DisableGraphic;
        }
        
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_CollectiblesMaster = GetComponent<CollectiblesMaster>();
            m_Collectible = (CollectibleScriptable)m_CollectiblesMaster.enemy;
        }

        void DisableGraphic(PlayerMaster playerMaster)
        {
            Tools.ToggleChildren(transform, false);
        }
        
        void CollectThis(Component collision)
        {
            var playerMaster = collision.GetComponentInParent<PlayerMaster>();
            var playerPowerUp = collision.GetComponentInParent<PlayerPowerUp>();
            if (!playerMaster.CouldCollectItem(m_Collectible.maxCollectible, m_Collectible)) return;
            ColliderOff();
            playerMaster.AddCollectiblesList(m_Collectible);
            if (playerPowerUp != null && m_CollectiblesMaster.collectibleScriptable.getPowerUp != null)
                playerPowerUp.ActivatePowerUp(m_CollectiblesMaster.collectibleScriptable.getPowerUp);
            obstacleMaster.isDestroyed = true;
            m_CollectiblesMaster.CallCollectibleEvent(playerMaster);
            gamePlayManager.CallEventCollectable(m_Collectible);
        }

        protected override void HitThis(Collider collision)
        {
            obstacleMaster.isDestroyed = true;
            var playerMaster = WhoHit(collision);
            if(playerMaster == null)
                playerMaster = WhoHit(collision);
            ColliderOff();
            playerMaster.AddHitList(m_Collectible);
            // Quem desativa o rander é o animation de explosão
            obstacleMaster.CallEventDestroyEnemy(playerMaster);
            ShouldSavePoint();
            //ShouldCompleteMission();
        }*/
    }
}
