using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class StateMoveHold : IMove
    {
        private readonly EnemiesMovement m_EnemiesMovement;
        private readonly ObstacleMasterOld _mObstacleMasterOld;
        private readonly GamePlayManager m_GamePlayManager;
        private Vector3 m_VectorDirection;
        public StateMoveHold(EnemiesMovement enemiesMovement, ObstacleMasterOld obstacleMasterOld)
        {
            m_EnemiesMovement = enemiesMovement;
            _mObstacleMasterOld = obstacleMasterOld;
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
                m_EnemiesMovement.ChangeState(new StateMove(m_EnemiesMovement, _mObstacleMasterOld));
            }
            if (m_EnemiesMovement.shouldBeMoving && m_EnemiesMovement.shouldBeApproach)
            {
                //.Log($"{transform.name} Patrulha!!!!");
                m_EnemiesMovement.ChangeState(new StateMovePatrol(m_EnemiesMovement, _mObstacleMasterOld));
            }
            
        }
        public void ExitState()
        {
           //Debug.Log("Estado: HOLD - Exit");
        }
    }
}
