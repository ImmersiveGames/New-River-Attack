using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateShootHold : IShoot
    {
        public void EnterState()
        {
            Debug.Log("Estado: Hold - Entrando");
            // Coloque aqui as ações a serem executadas ao entrar no estado "Hold"
        }
        public void UpdateState(IHasPool myPool, EnemiesMaster enemyMaster)
        {
            // Coloque aqui o código para Hold
            Debug.Log("Hold!");
        }
        public void ExitState()
        {
            Debug.Log("Estado: Hold - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado "hold"
        }
        public void Fire(IHasPool myPool, EnemiesMaster enemyMaster)
        {
            throw new System.NotImplementedException();
        }
    }
}