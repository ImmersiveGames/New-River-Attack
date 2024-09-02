using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class SubEmergeBehavior: IBossBehavior
    {
        private bool m_Finished;
        private readonly BossMasterOld _mBossMasterOld;
        private float m_CountTime;
        private const float TIME_LIMIT = 8f;
        
        internal SubEmergeBehavior(BossMasterOld bossMasterOld)
        {
            _mBossMasterOld = bossMasterOld;
        }
        public void Enter()
        {
            //Debug.Log("Entrando no comportamento SubEmerge");
            // Lógica de entrada para o comportamento SubEmerge
            m_Finished = false;
           // m_BossMaster.BossInvulnerability(true);
            _mBossMasterOld.OnEventBossSubmerge();
        }
        public void Update()
        {
            if(!_mBossMasterOld.shouldBeBossBattle) return;
            //Debug.Log("Atualizando comportamento SubEmerge");
            m_CountTime += Time.deltaTime;

            if (!(m_CountTime >= TIME_LIMIT))
                return;
            m_CountTime = 0f;
            _mBossMasterOld.BossInvulnerability(false);
            FinishBehavior();
        }
        public void Exit()
        {
            //Debug.Log("Saindo do comportamento SubEmerge");
            //m_BossMaster.BossInvulnerability(false);
            // Lógica de saída para o comportamento SubEmerge
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
