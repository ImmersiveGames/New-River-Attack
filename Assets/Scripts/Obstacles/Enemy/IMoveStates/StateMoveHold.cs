using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateMoveHold : IMove
    {
        /*readonly EnemiesMovement m_EnemiesMovement;
        Vector3 m_VectorDirection;
        public StateMoveHold(EnemiesMovement enemiesMovement)
        {
            m_EnemiesMovement = enemiesMovement;
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            Debug.Log("Estado: HOLD - Entrando ");
            m_VectorDirection = m_EnemiesMovement.SetDirection(EnemiesMovement.Directions.None);
            enemiesMaster.CallEventEnemiesMasterMovement(m_VectorDirection);
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            Debug.Log("Estado: HOLD - UPDATE");
        }
        public void ExitState()
        {
            Debug.Log("Estado: HOLD - Exit");
        }*/
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            throw new System.NotImplementedException();
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            throw new System.NotImplementedException();
        }
        public void ExitState()
        {
            throw new System.NotImplementedException();
        }
    }
}

