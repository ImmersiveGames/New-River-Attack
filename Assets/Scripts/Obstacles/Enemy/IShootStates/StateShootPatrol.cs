using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateShootPatrol : IShoot
    {
        float m_StartApproachRadius;
        float m_PlayerApproachRadius;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemiesSetDifficulty;
        public Transform target;
        PlayerDetectApproach m_PlayerDetectApproach;
        readonly ObstacleDetectApproach m_ObstacleDetectApproach;
        
        public StateShootPatrol(ObstacleDetectApproach enemiesShoot, Transform target)
        {
            this.target = target;
            m_ObstacleDetectApproach = enemiesShoot;
        }

        public void EnterState(EnemiesMaster enemyMaster)
        {
            target = null;
            //Debug.Log("Estado: Patrol - Entrando: " + enemyMaster.enemy);
            m_EnemiesMaster = enemyMaster;
            m_PlayerApproachRadius = m_StartApproachRadius = m_ObstacleDetectApproach.playerApproachRadius;
            if (!m_EnemiesMaster.enemy && !m_EnemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.actualDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadiusToShoot;
        }
        public void UpdateState()
        {
            var position = m_EnemiesMaster.transform.position;
            
            //TODO: Quando o inimigo morre, precisa resetar o target também.
            
            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            target =  m_PlayerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);
        }
        public void ExitState()
        {
            target = null;
            //Debug.Log("Estado: Patrol - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado Patrol
        }
    }
}
