using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateMovePatrol : IMove
    {
        private Transform m_Target;
        private float m_StartApproachRadius;
        private float m_PlayerApproachRadius;

        private readonly ObstacleMasterOld _mObstacleMasterOld;
        private EnemiesSetDifficulty m_EnemiesSetDifficulty;
        private PlayerDetectApproach m_PlayerDetectApproach;
        private readonly EnemiesMovement m_EnemiesMovement;

        public StateMovePatrol(EnemiesMovement enemiesMovement, ObstacleMasterOld obstacleMasterOld)
        {
            m_EnemiesMovement = enemiesMovement;
            _mObstacleMasterOld = obstacleMasterOld;
        }
        public void EnterState()
        {
            //Debug.Log($"{m_EnemiesMaster.gameObject.name} Estado: Patrol - Enter");
            m_Target = null;
            m_PlayerApproachRadius = m_StartApproachRadius = m_EnemiesMovement.playerApproachRadius;

            if (!_mObstacleMasterOld.enemy && !_mObstacleMasterOld.enemy.enemiesSetDifficultyListSo) return;
            m_EnemiesSetDifficulty = _mObstacleMasterOld.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(_mObstacleMasterOld.actualDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadiusToMove;
        }
        public void UpdateState(Transform transform, Vector3 direction)
        {
            //Debug.Log($"{transform.gameObject.name} Estado: Patrol - Update: "+ m_PlayerApproachRadius);
            var position = transform.position;
            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            m_Target = m_PlayerDetectApproach.TargetApproach<PlayerMasterOld>(GameManager.instance.layerPlayer);
            if(m_Target)
                m_EnemiesMovement.ChangeState(new StateMove(m_EnemiesMovement, _mObstacleMasterOld));
        }
        public void ExitState()
        {
            m_Target = null;
            //Debug.Log("Estado: Patrol - Exit");
        }
    }
}
