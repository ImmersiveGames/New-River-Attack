using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(CollectiblesMaster))]
    public class CollectiblesCollider : EnemiesCollider
    {
        CollectiblesMaster m_CollectiblesMaster;
        private CollectibleScriptable m_Collectible;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            m_CollectiblesMaster.CollectibleEvent += DisableGraphic;
        }
        protected override void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>())
            {
                CollectThis(collision);
            }
            if (collision.GetComponentInParent<Bullets>() && m_CollectiblesMaster.enemy.canDestruct)
            {
                HitThis(collision);
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
            Tools.ToggleChildren(this.transform, false);
        }
        
        public override void CollectThis(Collider collision)
        {
            var playerMaster = collision.GetComponentInParent<PlayerMaster>();
            var playerPowerUp = collision.GetComponentInParent<PlayerPowerUp>();
            if (!playerMaster.CouldCollectItem(m_Collectible.maxCollectible, m_Collectible)) return;
            ColliderOff();
            playerMaster.AddCollectiblesList(m_Collectible);
            if (playerPowerUp != null && m_CollectiblesMaster.collectibles.getPowerUp != null)
                playerPowerUp.ActivatePowerUp(m_CollectiblesMaster.collectibles.getPowerUp);
            enemiesMaster.isDestroyed = true;
            m_CollectiblesMaster.CallCollectibleEvent(playerMaster);
            gamePlay.CallEventCollectable(m_Collectible);
        }

        public override void HitThis(Collider collision)
        {
            enemiesMaster.isDestroyed = true;
            var playerMaster = WhoHit(collision);
            if(playerMaster == null)
                playerMaster = WhoHit(collision);
            ColliderOff();
            playerMaster.AddHitList(m_Collectible);
            // Quem desativa o rander é o animation de explosão
            enemiesMaster.CallEventDestroyEnemy(playerMaster);
            ShouldSavePoint();
            //ShouldCompleteMission();
        }
    }
}
