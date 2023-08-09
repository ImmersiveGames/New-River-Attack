using UnityEngine;

namespace RiverAttack
{
    public class EffectAreaCollider : EnemiesCollider
    {
        EffectAreaMaster m_EffectAreaMaster;
        EffectAreaScriptable m_EffectArea;
        [SerializeField]
        float timeToAccess;

        float m_Timer;
        
        #region UNITYMETHODS
        protected override void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponent<Bullets>() || !obstacleMaster.enemy.canDestruct) return;
            if (collision.GetComponent<BulletEnemy>()) return;
            HitThis(collision);
        }

        void OnTriggerExit(Collider collision)
        {
            var playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (!playerMaster) return;
            playerMaster.inEffectArea = false;
            m_EffectAreaMaster.CallEventExitAreaEffect();
        }
        void OnTriggerStay(Collider collision)
        {
            var playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (!playerMaster) return;
            if (!playerMaster.inEffectArea) playerMaster.inEffectArea = true;
            CollectThis(playerMaster);
        }
        #endregion

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EffectAreaMaster = GetComponent<EffectAreaMaster>();
            m_EffectArea = (EffectAreaScriptable)m_EffectAreaMaster.enemy;
        }
        protected override void HitThis(Collider collision)
        {
            m_EffectAreaMaster.isDestroyed = true;
            var playerMaster = WhoHit(collision);
            m_EffectAreaMaster.CallEventDestroyEnemy(playerMaster);
            ShouldSavePoint();
        }
        void CollectThis(PlayerMaster collision)
        {
            var player = collision.GetPlayersSettings();
            if (m_Timer <= 0)
            {
                m_EffectArea.EffectAreaStart(player);
                m_EffectAreaMaster.CallEventAreaEffect();
                m_Timer = timeToAccess;
            }
            m_Timer -= Time.deltaTime;
        }
    }
}

