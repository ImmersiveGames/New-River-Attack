using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateMovePatrol : IMove
    {
        public Transform target;
        readonly float m_StartApproachRadius;
        float m_PlayerApproachRadius;

        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemiesSetDifficulty;
        PlayerDetectApproach m_PlayerDetectApproach;
        
        public StateMovePatrol(ObstacleDetectApproach enemiesMovement, Transform target)
        {
            this.target = target;
            m_PlayerApproachRadius = m_StartApproachRadius = enemiesMovement.playerApproachRadius;
        }
        public void EnterState(EnemiesMaster enemiesMaster)
        {
            target = null;
            m_EnemiesMaster = enemiesMaster;
            if (!m_EnemiesMaster.enemy && !m_EnemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.getDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadiusToMove;
            //Debug.Log("Estado: Patrol - Enter");
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            //Debug.Log("Estado: Patrol - Update");
            var position = m_EnemiesMaster.transform.position;
            
            //TODO: Quando o inimigo morre, precisa resetar o target também.
            
            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            target =  m_PlayerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);
        }
        public void ExitState()
        {
            target = null;
            //Debug.Log("Estado: Patrol - Exit");
        }
    }
}
