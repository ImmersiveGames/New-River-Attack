using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EmergeBehavior : IBossBehavior
    {
        bool m_Finished;
        readonly BossMaster m_BossMaster;
        float m_CountTime;
        const float TIME_LIMIT = 7.5f;

        internal EmergeBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
        }
        public void Enter()
        {
            Debug.Log($"Entrando no comportamento Emergir {m_BossMaster.targetPlayer}");
            m_BossMaster.BossInvulnerability(true);
            // Lógica de entrada para o comportamento Emergir
            m_Finished = false;
            //m_BossMaster.BossInvulnerability(true);
            m_BossMaster.MoveBoss(m_BossMaster.actualPosition);
        }
        public void Update()
        {
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
