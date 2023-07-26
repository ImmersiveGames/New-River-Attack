using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(LevelObstacleSpawnShoot))]
    public class LevelObstacleSpawnShootApproach : EnemiesShootApproach
    {
        LevelObstacleSpawnMaster m_SpawnMaster;
        
        [ContextMenu("LoadPrefab")]
        private void LoadPrefab()
        {
            m_SpawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            var oda = m_SpawnMaster.getPrefab.GetComponent<ObstacleDetectApproach>();
            if (oda == null) return;
            radiusPlayerProximity = oda.radiusPlayerProximity;
            randomPlayerDistanceNear = oda.randomPlayerDistanceNear;
            difficultType = oda.difficultType;
            var enemiesOda = (EnemiesShootApproach)oda;
            timeToCheck = enemiesOda.timeToCheck;
            enemiesesEnemiesSetDifficultyListSo = oda.enemiesesEnemiesSetDifficultyListSo;
        }
    }
}
