﻿using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EmergeBehavior : IBossBehavior
    {
        bool m_Finished = false;
        public void Enter()
        {
            Debug.Log("Entrando no comportamento Emergir");
            // Lógica de entrada para o comportamento Emergir
            m_Finished = false;
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
