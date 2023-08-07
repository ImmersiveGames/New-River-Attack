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
            if (obstacleMaster.enemy.canRespawn)
                gamePlayManager.EventResetEnemies += ColliderOn;
        }
        protected override void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponentInParent<PlayerMaster>() && !collision.GetComponent<Bullets>() || !obstacleMaster.enemy.canDestruct)
                return;
            if (collision.GetComponent<BulletEnemy>()) return;
            HitThis(collision);
        }
        private void OnDisable()
        {
            if (obstacleMaster.enemy.canRespawn)
                gamePlayManager.EventResetEnemies -= ColliderOn;
        }
        private void OnDestroy()
        {
            if (obstacleMaster.enemy.canRespawn)
                gamePlayManager.EventResetEnemies -= ColliderOn;
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_MyCollider = GetComponentsInChildren<Collider>();
        }

        public override void HitThis(Collider collision)
        {
            obstacleMaster.isDestroyed = true;
            var playerMaster = WhoHit(collision);
            ColliderOff();
            // Quem desativa o rander é o animation de explosão
            obstacleMaster.CallEventDestroyEnemy(playerMaster);
            ShouldSavePoint();
            playerMaster.CallEventPlayerMasterCollider();
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
