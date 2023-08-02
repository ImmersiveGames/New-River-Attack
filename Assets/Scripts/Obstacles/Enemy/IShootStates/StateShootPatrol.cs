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
        protected internal void SetPatrol(float approachRadius, float timeToCheck)
        {
            m_PlayerApproachRadius = m_StartApproachRadius = approachRadius;
        }

        public void EnterState(EnemiesMaster enemyMaster)
        {
            target = null;
            Debug.Log("Estado: Patrol - Entrando: " + enemyMaster.enemy);
            m_EnemiesMaster = enemyMaster;
            if (!m_EnemiesMaster.enemy && !m_EnemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            m_EnemiesSetDifficulty = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.getDifficultName);
            m_PlayerApproachRadius = m_StartApproachRadius * m_EnemiesSetDifficulty.multiplyPlayerDistanceRadius;
        }
        public void UpdateState()
        {
            var position = m_EnemiesMaster.transform.position;
            
            //TODO: Quando o inimigo morre, precisa resetar o target também.
            
            /*var results = new Collider[2];
            int size = Physics.OverlapSphereNonAlloc(position, m_PlayerApproachRadius, results, GameManager.instance.layerPlayer);
            if (size < 1) return;
            for (int i = 0; i < size; i++)
            {
                Debug.Log("GIRO: "+ i + "Result"+results[i]);
                var player = results[i].GetComponentInParent<PlayerMaster>();
                target = player ? player.transform : null;
            }*/
            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            target =  m_PlayerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);
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
