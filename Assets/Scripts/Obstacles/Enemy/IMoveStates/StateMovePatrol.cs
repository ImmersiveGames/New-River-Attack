using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateMovePatrol : IMove
    {
        public readonly Transform target;
        readonly EnemiesMovement m_EnemiesMovement;
        public StateMovePatrol(EnemiesMovement enemiesMovement, Transform target)
        {
            this.target = target;
            m_EnemiesMovement = enemiesMovement;
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            Debug.Log("Estado: Patrol - Enter");
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            Debug.Log("Estado: Patrol - Update");
        }
        public void ExitState()
        {
            Debug.Log("Estado: Patrol - Exit");
        }
    }
}
