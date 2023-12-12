using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EmergeBehavior : IBossBehavior
    {
        bool m_Finished;
        readonly BossMaster m_BossMaster;
        float m_CountTime = 0f;
        const float TIME_LIMIT = 10f;

        internal EmergeBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
        }
        public void Enter()
        {
            Debug.Log($"Entrando no comportamento Emergir {m_BossMaster.targetPlayer}");
            // Lógica de entrada para o comportamento Emergir
            m_Finished = false;
            m_BossMaster.invulnerability = true;
            m_BossMaster.MoveBoss(m_BossMaster.actualPosition);
        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento Emergir");
            m_CountTime += Time.deltaTime;

            if (!(m_CountTime >= TIME_LIMIT))
                return;
            m_CountTime = 0f;
            m_Finished = true;
            Debug.Log("O contador atingiu " + TIME_LIMIT + " segundos.");
        }
        public void Exit()
        {
            Debug.Log("Saindo do comportamento Emergir");
            m_BossMaster.invulnerability = false;
            // Lógica de saída para o comportamento Emergir
        }
        public bool IsFinished()
        {
            return m_Finished;
        }
    }
}
