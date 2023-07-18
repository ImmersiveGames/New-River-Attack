using UnityEngine;
using Utils;

namespace RiverAttack
{

    [RequireComponent(typeof(CollectiblesMaster))]
    public class CollectiblesCollider : EnemiesCollider
    {
        protected CollectiblesMaster collectiblesMaster;
        private CollectibleScriptable m_Collectible;

        protected override void OnEnable()
        {
            base.OnEnable();
            collectiblesMaster.CollectibleEvent += DisableGraphic;
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            collectiblesMaster = GetComponent<CollectiblesMaster>();
            m_Collectible = (CollectibleScriptable)collectiblesMaster.enemy;
        }

        private void DisableGraphic(PlayerMaster playerMaster)
        {
            Tools.ToggleChildren(this.transform, false);
        }

        protected override void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>())
            {
                CollectThis(collision);
            }
            if (collision.GetComponentInParent<Bullets>() && collectiblesMaster.enemy.canDestruct)
            {
                HitThis(collision);
            }
        }
        public override void CollectThis(Collider collision)
        {
            PlayerMaster playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (playerMaster.CouldCollectItem(m_Collectible.maxCollectible, m_Collectible))
            {
                ColliderOff();
                playerMaster.AddCollectiblesList(m_Collectible);
                PlayerPowerUp playerPowerup = collision.GetComponentInParent<PlayerPowerUp>();
                if (playerPowerup != null && collectiblesMaster.collectibles.getPowerUp != null)
                    playerPowerup.ActivatePowerup(collectiblesMaster.collectibles.getPowerUp);
                enemiesMaster.isDestroyed = true;
                collectiblesMaster.CallCollectibleEvent(playerMaster);
                gamePlay.CallEventCollectable(m_Collectible);
            }
        }

        public override void HitThis(Collider collision)
        {
            enemiesMaster.isDestroyed = true;
            PlayerMaster playerMaster = WhoHit(collision);
            //if(playerMaster == null)
            //    playerMaster = WhoHit(collision);
            ColliderOff();
            playerMaster.AddHitList(m_Collectible);
            // Quem desativa o rander é o animation de explosão
            enemiesMaster.CallEventDestroyEnemy(playerMaster);
            ShouldSavePoint();
            //ShouldCompleteMission();
        }

        private void OnDisable()
        {
            collectiblesMaster.CollectibleEvent -= DisableGraphic;
        }
    }
}
