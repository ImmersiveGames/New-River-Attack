using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EmergeBehavior : IBossBehavior
    {
        private bool m_Finished;
        private readonly BossMasterOld _mBossMasterOld;
        private float m_CountTime;
        private const float TIME_LIMIT = 7.5f;

        internal EmergeBehavior(BossMasterOld bossMasterOld)
        {
            _mBossMasterOld = bossMasterOld;
        }
        public void Enter()
        {
            Debug.Log($"Entrando no comportamento Emergir {_mBossMasterOld.targetPlayer}");
            _mBossMasterOld.BossInvulnerability(true);
            // Lógica de entrada para o comportamento Emergir
            m_Finished = false;
            //m_BossMaster.BossInvulnerability(true);
            _mBossMasterOld.MoveBoss(_mBossMasterOld.actualPosition);
        }
        public void Update()
        {
            if(!_mBossMasterOld.shouldBeBossBattle) return;
            //Debug.Log("Atualizando comportamento Emergir");
            m_CountTime += Time.deltaTime;

            if (!(m_CountTime >= TIME_LIMIT))
                return;
            m_CountTime = 0f;
            FinishBehavior();
        }
        public void Exit()
        {
            //m_BossMaster.BossInvulnerability(false);
            Debug.Log("Saindo do comportamento Emergir");
            //m_BossMaster.invulnerability = false;
            // Lógica de saída para o comportamento Emergir
        }
        public void FinishBehavior()
        {
            m_Finished = true;
        }
        public bool IsFinished()
        {
            return m_Finished;
        }
    }
}
