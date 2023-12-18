using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class SubEmergeBehavior: IBossBehavior
    {
        bool m_Finished;
        readonly BossMaster m_BossMaster;
        float m_CountTime;
        const float TIME_LIMIT = 8f;
        
        internal SubEmergeBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
        }
        public void Enter()
        {
            Debug.Log("Entrando no comportamento SubEmerge");
            // Lógica de entrada para o comportamento SubEmerge
            m_Finished = false;
            m_BossMaster.BossInvulnerability(true);
            m_BossMaster.Submerge();
        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento SubEmerge");
            m_CountTime += Time.deltaTime;

            if (!(m_CountTime >= TIME_LIMIT))
                return;
            m_CountTime = 0f;
            m_BossMaster.BossInvulnerability(false);
            FinishBehavior();
            Debug.Log("O contador atingiu " + TIME_LIMIT + " segundos.");
        }
        public void Exit()
        {
            Debug.Log("Saindo do comportamento SubEmerge");
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
