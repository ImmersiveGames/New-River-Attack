using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateShootPatrol : IShoot
    {
        private float m_StartApproachRadius;
        private float m_PlayerApproachRadius;
        private EnemiesMasterOld _mEnemiesMasterOld;
        private ObstacleMasterOld _mObstacleMasterOld;
        private EnemiesSetDifficulty m_EnemiesSetDifficulty;
        public Transform target;
        private PlayerDetectApproach m_PlayerDetectApproach;
        private readonly ObstacleDetectApproach m_ObstacleDetectApproach;

        public StateShootPatrol(ObstacleDetectApproach enemiesShoot, Transform target)
        {
            this.target = target;
            m_ObstacleDetectApproach = enemiesShoot;
        }

        public void EnterState(ObstacleMasterOld obstacleMasterOld)
        {
            target = null;
            _mObstacleMasterOld = obstacleMasterOld;
            //Debug.Log("Estado: Patrol - Entrando: " + enemyMaster.enemy);
            m_PlayerApproachRadius = m_StartApproachRadius = m_ObstacleDetectApproach.playerApproachRadius;
            if (_mObstacleMasterOld is not EnemiesMasterOld master)
                return;
            _mEnemiesMasterOld = master;
            if (_mEnemiesMasterOld != null && !_mEnemiesMasterOld.enemy && !_mEnemiesMasterOld.enemy.enemiesSetDifficultyListSo) return;
            if (_mEnemiesMasterOld != null)
                m_EnemiesSetDifficulty = _mEnemiesMasterOld.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(_mEnemiesMasterOld.actualDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadiusToShoot;

        }
        public void UpdateState()
        {
            var position = _mObstacleMasterOld.transform.position;

            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            target = m_PlayerDetectApproach.TargetApproach<PlayerMasterOld>(GameManager.instance.layerPlayer);
        }
        public void ExitState()
        {
            target = null;
            //Debug.Log("Estado: Patrol - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado Patrol
        }
    }
}
