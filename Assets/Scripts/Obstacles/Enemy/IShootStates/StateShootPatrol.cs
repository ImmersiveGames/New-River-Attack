using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateShootPatrol : IShoot
    {
        private float m_StartApproachRadius;
        private float m_PlayerApproachRadius;
        private EnemiesMaster m_EnemiesMaster;
        private ObstacleMaster m_ObstacleMaster;
        private EnemiesSetDifficulty m_EnemiesSetDifficulty;
        public Transform target;
        private PlayerDetectApproach m_PlayerDetectApproach;
        private readonly ObstacleDetectApproach m_ObstacleDetectApproach;

        public StateShootPatrol(ObstacleDetectApproach enemiesShoot, Transform target)
        {
            this.target = target;
            m_ObstacleDetectApproach = enemiesShoot;
        }

        public void EnterState(ObstacleMaster obstacleMaster)
        {
            target = null;
            m_ObstacleMaster = obstacleMaster;
            //Debug.Log("Estado: Patrol - Entrando: " + enemyMaster.enemy);
            m_PlayerApproachRadius = m_StartApproachRadius = m_ObstacleDetectApproach.playerApproachRadius;
            if (m_ObstacleMaster is not EnemiesMaster master)
                return;
            m_EnemiesMaster = master;
            if (m_EnemiesMaster != null && !m_EnemiesMaster.enemy && !m_EnemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            if (m_EnemiesMaster != null)
                m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.actualDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadiusToShoot;

        }
        public void UpdateState()
        {
            var position = m_ObstacleMaster.transform.position;

            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            target = m_PlayerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);
        }
        public void ExitState()
        {
            target = null;
            //Debug.Log("Estado: Patrol - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado Patrol
        }
    }
}
