using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateMovePatrol : IMove
    {
        /*public Transform target;
        float m_StartApproachRadius;
        float m_PlayerApproachRadius;

        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemiesSetDifficulty;
        PlayerDetectApproach m_PlayerDetectApproach;
        readonly ObstacleDetectApproach m_ObstacleDetectApproach;
        
        public StateMovePatrol(ObstacleDetectApproach enemiesMovement, Transform target)
        {
            this.target = target;
            m_ObstacleDetectApproach = enemiesMovement;
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            //Debug.Log("Estado: Patrol - Enter: " + m_ObstacleDetectApproach.playerApproachRadius);
            target = null;
            m_EnemiesMaster = enemiesMaster;
            m_PlayerApproachRadius = m_StartApproachRadius = m_ObstacleDetectApproach.playerApproachRadius;
            
            if (!m_EnemiesMaster.enemy && !m_EnemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.actualDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadiusToMove;
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            var position = transform.position;
            //Debug.Log("Estado: Patrol - Update: "+ m_PlayerApproachRadius);
            //TODO: Quando o inimigo morre, precisa resetar o target também.
            
            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            target =  m_PlayerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);
        }
        public void ExitState()
        {
            target = null;
            //Debug.Log("Estado: Patrol - Exit");
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
