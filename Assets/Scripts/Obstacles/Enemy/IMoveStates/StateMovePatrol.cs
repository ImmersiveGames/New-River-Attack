using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateMovePatrol : IMove
    {
        private Transform m_Target;
        private float m_StartApproachRadius;
        private float m_PlayerApproachRadius;

        private readonly ObstacleMaster m_ObstacleMaster;
        private EnemiesSetDifficulty m_EnemiesSetDifficulty;
        private PlayerDetectApproach m_PlayerDetectApproach;
        private readonly EnemiesMovement m_EnemiesMovement;

        public StateMovePatrol(EnemiesMovement enemiesMovement, ObstacleMaster obstacleMaster)
        {
            m_EnemiesMovement = enemiesMovement;
            m_ObstacleMaster = obstacleMaster;
        }
        public void EnterState()
        {
            //Debug.Log($"{m_EnemiesMaster.gameObject.name} Estado: Patrol - Enter");
            m_Target = null;
            m_PlayerApproachRadius = m_StartApproachRadius = m_EnemiesMovement.playerApproachRadius;

            if (!m_ObstacleMaster.enemy && !m_ObstacleMaster.enemy.enemiesSetDifficultyListSo) return;
            m_EnemiesSetDifficulty = m_ObstacleMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_ObstacleMaster.actualDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadiusToMove;
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            //Debug.Log($"{transform.gameObject.name} Estado: Patrol - Update: "+ m_PlayerApproachRadius);
            var position = transform.position;
            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            m_Target = m_PlayerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);
            if(m_Target)
                m_EnemiesMovement.ChangeState(new StateMove(m_EnemiesMovement, m_ObstacleMaster));
        }
        public void ExitState()
        {
            m_Target = null;
            //Debug.Log("Estado: Patrol - Exit");
        }
    }
}
