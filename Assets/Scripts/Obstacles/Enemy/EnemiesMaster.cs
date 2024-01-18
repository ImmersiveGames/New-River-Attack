using UnityEngine;

namespace RiverAttack
{
    public class EnemiesMaster : ObstacleMaster
    {
        EnemiesSetDifficultyListSo m_EnemiesSetDifficultList;
        
        public static EnemiesSetDifficulty myDifficulty { get; private set; }

        #region Delegates
        internal event MovementEventHandler EventObjectMasterFlipEnemies;
        #endregion

        #region UNITYMETHODS
        internal override void Awake()
        {
            base.Awake();
            m_EnemiesSetDifficultList = enemy.enemiesSetDifficultyListSo;
            if (m_EnemiesSetDifficultList != null)
                myDifficulty = m_EnemiesSetDifficultList.GetDifficultByEnemyDifficult(actualDifficultName);
        }
        internal override void OnEnable()
        {
            base.OnEnable();
            ChangeDifficulty();
            EventObstacleMasterHit += ChangeDifficulty;
        }
        internal override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.GetComponent<BulletPlayer>()) return;
            ComponentToKill(other.GetComponentInParent<PlayerMaster>(), CollisionType.Collider);
            GamePlayManager.instance.OnEventOtherEnemiesKillPlayer();
        }
        void OnDisable()
        {
            EventObstacleMasterHit -= ChangeDifficulty;
        }
          #endregion

        void ChangeDifficulty()
        {
            if (m_EnemiesSetDifficultList != null)
            {
                myDifficulty = enemy.enemiesSetDifficultyListSo.GetDifficultByScore(PlayerManager.instance.HighScorePlayers());
            }
        }

        #region Calls
        internal void OnEventObjectMasterFlipEnemies(bool active)
        {
            EventObjectMasterFlipEnemies?.Invoke(active);
        }
  #endregion

    }
}
