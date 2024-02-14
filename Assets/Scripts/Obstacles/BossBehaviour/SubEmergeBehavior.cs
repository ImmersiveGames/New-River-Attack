using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class SubEmergeBehavior: IBossBehavior
    {
        private bool m_Finished;
        private readonly BossMaster m_BossMaster;
        private float m_CountTime;
        private const float TIME_LIMIT = 8f;
        
        internal SubEmergeBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
        }
        public void Enter()
        {
            Debug.Log("Entrando no comportamento SubEmerge");
            // Lógica de entrada para o comportamento SubEmerge
            m_Finished = false;
           // m_BossMaster.BossInvulnerability(true);
            m_BossMaster.OnEventBossSubmerge();
        }
        public void Update()
        {
            if(!m_BossMaster.shouldBeBossBattle) return;
            Debug.Log("Atualizando comportamento SubEmerge");
            m_CountTime += Time.deltaTime;

            if (!(m_CountTime >= TIME_LIMIT))
                return;
            m_CountTime = 0f;
            m_BossMaster.BossInvulnerability(false);
            FinishBehavior();
        }
        public void Exit()
        {
            Debug.Log("Saindo do comportamento SubEmerge");
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
