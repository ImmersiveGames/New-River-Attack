using UnityEngine;

namespace RiverAttack
{
    public sealed class EnemiesMaster : ObstacleMaster
    {
        EnemiesSetDifficultyListSo m_EnemiesSetDifficultList;

        public EnemiesSetDifficulty.EnemyDifficult actualDifficultName;
        public static EnemiesSetDifficulty myDifficulty { get; private set; }

        #region Delegates
        internal event GeneralEventHandler EventObjectMasterFlipEnemies;
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
        internal override void OnDisable()
        {
            base.OnDisable();
            EventObstacleMasterHit -= ChangeDifficulty;
        }
          #endregion

        void ChangeDifficulty()
        {
            if (m_EnemiesSetDifficultList != null)
            {
                myDifficulty = enemy.enemiesSetDifficultyListSo.GetDifficultByScore(gamePlayManager.HighScorePlayers());
            }
        }

        #region Calls
        internal void OnEventObjectMasterFlipEnemies()
        {
            EventObjectMasterFlipEnemies?.Invoke();
        }
  #endregion

    }
}
