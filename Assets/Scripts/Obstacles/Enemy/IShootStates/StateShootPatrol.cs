using System.Drawing;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateShootPatrol : IShoot
    {
        float m_TimeToCheck;
        float m_StarTimeToCheck;
        float m_StartApproachRadius;
        float m_PlayerApproachRadius;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemiesSetDifficulty;
        public Transform target;
        protected internal void SetPatrol(float approachRadius, float timeToCheck)
        {
            m_PlayerApproachRadius = m_StartApproachRadius = approachRadius;
            m_StarTimeToCheck = m_TimeToCheck = timeToCheck;
        }

        public void EnterState(EnemiesMaster enemyMaster)
        {
            target = null;
            Debug.Log("Estado: Patrol - Entrando");
            m_EnemiesMaster = enemyMaster;
            if (!m_EnemiesMaster.enemy || m_EnemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.getDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadius;
        }
        public void UpdateState()
        {
            //Debug.Log("PLAYER: "+ target.position.z);
            // Coloque aqui o código para Patrol
            //Debug.Log("GET TARGET: "+ target + "Disance: " + m_PlayerApproachRadius);
            var results = new Collider[2];
            var position = m_EnemiesMaster.transform.position;
            //Debug.Log("Enemy position: " + position);
            int size = Physics.OverlapSphereNonAlloc(position, m_PlayerApproachRadius, results, GameManager.instance.layerPlayer);
            if (size < 1) return;
            for (int i = 0; i < size; i++)
            {
                var player = results[i].GetComponentInParent<PlayerMaster>();
                target = player ? player.transform : null;
            }
            /*var playerDetectApproach = new PlayerDetectApproach(m_EnemiesMaster.transform.position, m_PlayerApproachRadius);
            playerDetectApproach.UpdatePatrolDistance(m_PlayerApproachRadius);
            target = playerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);*/
        }
        public void ExitState()
        {
            target = null;
            Debug.Log("Estado: Patrol - Saindo");
            // Coloque aqui as ações a serem executadas ao sair do estado Patrol
        }
        public void Fire()
        {
            Debug.Log("Patrol!");
        }
    }
}
