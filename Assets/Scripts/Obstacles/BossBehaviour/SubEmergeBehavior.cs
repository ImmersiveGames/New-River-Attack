using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class SubEmergeBehavior: IBossBehavior
    {
        bool m_Finished = false;
        BossMaster m_BossMaster;
        
        internal SubEmergeBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
        }
        public void Enter()
        {
            Debug.Log("Entrando no comportamento SubEmerge");
            // Lógica de entrada para o comportamento SubEmerge
            m_Finished = false;
        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento SubEmerge");
            // Lógica de atualização para o comportamento SubEmerge

            // Exemplo: Após um certo tempo ou condição, marcamos o comportamento como concluído
            if (Input.GetKeyDown(KeyCode.P)) {
                m_Finished = true;
            }
        }
        public void Exit()
        {
            Debug.Log("Saindo do comportamento SubEmerge");
            // Lógica de saída para o comportamento SubEmerge
        }
        public bool IsFinished()
        {
            return m_Finished;
        }
    }
}
