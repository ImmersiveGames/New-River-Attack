using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateShootPatrol : IShoot
    {
        public void EnterState()
        {
            Debug.Log("Estado: Patrol - Entrando");
            // Coloque aqui as ações a serem executadas ao entrar no estado Patrol
        }
        public void UpdateState(IHasPool myPool, EnemiesMaster enemyMaster)
        {
            // Coloque aqui o código para Patrol
            Debug.Log("Patrol!");
        }
        public void ExitState()
        {
            Debug.Log("Estado: Patrol - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado Patrol
        }
        public void Fire(IHasPool myPool, EnemiesMaster enemyMaster)
        {
            throw new System.NotImplementedException();
        }
    }
}
