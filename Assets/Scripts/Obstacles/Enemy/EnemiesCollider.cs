using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class EnemiesCollider : ObstacleColliders
    {
        Collider[] m_MyCollider;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            if (enemiesMaster.enemy.canRespawn)
                gamePlay.EventResetEnemies += ColliderOn;
        }
        protected override void OnTriggerEnter(Collider collision)
        {
            if ((collision.GetComponentInParent<PlayerMaster>() || collision.GetComponentInParent<Bullets>()) && enemiesMaster.enemy.canDestruct)
            {
                HitThis(collision);
            }
        }
        private void OnDisable()
        {
            if (enemiesMaster.enemy.canRespawn)
                gamePlay.EventResetEnemies -= ColliderOn;
        }
        private void OnDestroy()
        {
            if (enemiesMaster.enemy.canRespawn)
                gamePlay.EventResetEnemies -= ColliderOn;
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_MyCollider = GetComponentsInChildren<Collider>();
        }

        public override void HitThis(Collider collision)
        {
            enemiesMaster.isDestroyed = true;
            var playerMaster = WhoHit(collision);
            ColliderOff();
            // Quem desativa o rander é o animation de explosão
            enemiesMaster.CallEventDestroyEnemy(playerMaster);
            ShouldSavePoint();
            playerMaster.CallEventPlayerHit();
            //ShouldCompleteMission();
        }
        protected void ColliderOff()
        {
            int length = m_MyCollider.Length;
            for (int i = 0; i < length; i++)
            {
                m_MyCollider[i].enabled = false;
            }
        }
        void ColliderOn()
        {
            if (m_MyCollider == null)  return;
            int length = m_MyCollider.Length;
            for (int i = 0; i < length; i++)
            {
                m_MyCollider[i].enabled = true;
            }

        }
    }
}
