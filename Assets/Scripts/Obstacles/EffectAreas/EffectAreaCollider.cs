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
            if (!collision.GetComponent<BulletPlayer>() || !enemiesMaster.enemy.canDestruct)
                return;
            HitThis(collision);
        }

        void OnTriggerExit(Collider collision)
        {
            if (!collision.GetComponentInParent<PlayerMaster>()) return;
            m_EffectAreaMaster.CallEventExitAreaEffect();
        }
        void OnTriggerStay(Collider collision)
        {
            if (!collision.GetComponentInParent<PlayerMaster>()) return;
            CollectThis(collision);
        }
        #endregion

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EffectAreaMaster = GetComponent<EffectAreaMaster>();
            m_EffectArea = (EffectAreaScriptable)m_EffectAreaMaster.enemy;
        }
        public override void HitThis(Collider collision)
        {
            m_EffectAreaMaster.isDestroyed = true;
            var playerMaster = WhoHit(collision);
            m_EffectAreaMaster.CallEventDestroyEnemy(playerMaster);
            ShouldSavePoint();
        }
        public override void CollectThis(Collider collision)
        {
            var player = collision.GetComponentInParent<PlayerMaster>().GetPlayersSettings();
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

