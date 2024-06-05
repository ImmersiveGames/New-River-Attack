using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateShootHold : IShoot
    {
        public void EnterState(ObstacleMasterOld enemyMasterOld)
        {
           // Debug.Log("Estado: ShootHold - Entrando");
            // Coloque aqui as ações a serem executadas ao entrar no estado "Hold"
        }
        public void UpdateState()
        {
            // Coloque aqui o código para Hold
           // Debug.Log("Shoot Hold!");
        }
        public void ExitState()
        {
           // Debug.Log("Estado: Shoot Hold - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado "hold"
        }
    }
}
