using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EmergeBehavior : IBossBehavior
    {
        bool m_Finished = false;
        BossMaster m_BossMaster;

        internal EmergeBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
        }
        public void Enter()
        {
            Debug.Log($"Entrando no comportamento Emergir {m_BossMaster.targetPlayer}");
            // Lógica de entrada para o comportamento Emergir
            m_Finished = false;
            m_BossMaster.Invoke("m_BossMaster.MoveBoss(BattleBossSubState.Top)",2f);
            
        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento Emergir");
            // Lógica de atualização para o comportamento Emergir

            // Exemplo: Após um certo tempo ou condição, marcamos o comportamento como concluído
            if (Input.GetKeyDown(KeyCode.P)) {
                m_Finished = true;
            }
        }
        public void Exit()
        {
            Debug.Log("Saindo do comportamento Emergir");
            // Lógica de saída para o comportamento Emergir
        }
        public bool IsFinished()
        {
            return m_Finished;
        }
    }
}
