using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class MissileAttackBehavior: IBossBehavior
    {
        bool m_Finished = false;
        public void Enter()
        {
            Debug.Log("Entrando no comportamento MissileAttack");
            // Lógica de entrada para o comportamento MissileAttack
            m_Finished = false;
        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento MissileAttack");
            // Lógica de atualização para o comportamento MissileAttack

            // Exemplo: Após um certo tempo ou condição, marcamos o comportamento como concluído
            if (Input.GetKeyDown(KeyCode.P)) {
                m_Finished = true;
            }
        }
        public void Exit()
        {
            Debug.Log("Saindo do comportamento MissileAttack");
            // Lógica de saída para o comportamento MissileAttack
        }
        public bool IsFinished()
        {
            return m_Finished;
        }
    }
}
