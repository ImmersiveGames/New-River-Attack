using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateMovePatrol : IMove
    {
        Transform m_Target;
        float m_StartApproachRadius;
        float m_PlayerApproachRadius;

        readonly EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemiesSetDifficulty;
        PlayerDetectApproach m_PlayerDetectApproach;
        readonly EnemiesMovement m_EnemiesMovement;

        public StateMovePatrol(EnemiesMovement enemiesMovement, EnemiesMaster enemiesMaster)
        {
            m_EnemiesMovement = enemiesMovement;
            m_EnemiesMaster = enemiesMaster;
        }
        public void EnterState()
        {
            //Debug.Log($"{m_EnemiesMaster.gameObject.name} Estado: Patrol - Enter");
            m_Target = null;
            m_PlayerApproachRadius = m_StartApproachRadius = m_EnemiesMovement.playerApproachRadius;

            if (!m_EnemiesMaster.enemy && !m_EnemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.actualDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadiusToMove;
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            //Debug.Log($"{transform.gameObject.name} Estado: Patrol - Update: "+ m_PlayerApproachRadius);
            var position = transform.position;
            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            m_Target = m_PlayerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);
            if(m_Target)
                m_EnemiesMovement.ChangeState(new StateMove(m_EnemiesMovement, m_EnemiesMaster));
        }
        public void ExitState()
        {
            m_Target = null;
            //Debug.Log("Estado: Patrol - Exit");
        }
    }
}
