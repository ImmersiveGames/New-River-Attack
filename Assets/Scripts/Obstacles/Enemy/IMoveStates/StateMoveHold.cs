using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateMoveHold : IMove
    {
        readonly EnemiesMovement m_EnemiesMovement;
        readonly EnemiesMaster m_EnemiesMaster;
        readonly GamePlayManager m_GamePlayManager;
        Vector3 m_VectorDirection;
        public StateMoveHold(EnemiesMovement enemiesMovement, EnemiesMaster enemiesMaster)
        {
            m_EnemiesMovement = enemiesMovement;
            m_EnemiesMaster = enemiesMaster;
            m_GamePlayManager = GamePlayManager.instance;
        }
        public void EnterState()
        {
           // Debug.Log($"{enemiesMaster.gameObject.name} Estado: HOLD - Entrando ");
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
           // Debug.Log($"{transform.gameObject.name} Estado: HOLD - UPDATE");
            if (m_EnemiesMovement.shouldBeMoving && !m_EnemiesMovement.shouldBeApproach)
            {
                //Debug.Log($"{transform.name} MOVE!!!!");
                m_EnemiesMovement.ChangeState(new StateMove(m_EnemiesMovement, m_EnemiesMaster));
            }
            if (m_EnemiesMovement.shouldBeMoving && m_EnemiesMovement.shouldBeApproach)
            {
                //.Log($"{transform.name} Patrulha!!!!");
                m_EnemiesMovement.ChangeState(new StateMovePatrol(m_EnemiesMovement, m_EnemiesMaster));
            }
            
        }
        public void ExitState()
        {
           //Debug.Log("Estado: HOLD - Exit");
        }
    }
}
