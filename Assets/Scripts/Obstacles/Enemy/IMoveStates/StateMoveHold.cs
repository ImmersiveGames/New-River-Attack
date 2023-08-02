using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateMoveHold : IMove
    {
        readonly EnemiesMovement m_EnemiesMovement;
        Vector3 vectorDirection;
        public StateMoveHold(EnemiesMovement enemiesMovement)
        {
            m_EnemiesMovement = enemiesMovement;
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            Debug.Log("Estado: HOLD - Entrando ");
            vectorDirection = m_EnemiesMovement.SetDirection(EnemiesMovement.Directions.None);
            enemiesMaster.CallEventEnemiesMasterMovement(vectorDirection);
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            Debug.Log("Estado: HOLD - UPDATE");
        }
        public void ExitState()
        {
            Debug.Log("Estado: HOLD - Exit");
        }
    }
}

