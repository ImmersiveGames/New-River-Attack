using UnityEngine;

namespace RiverAttack
{
    public class EnemiesMaster : ObstacleMaster
    {
        /*EnemiesSetDifficultyListSo m_EnemiesSetDifficultList;

        public EnemiesSetDifficulty.EnemyDifficult actualDifficultName;
        public static EnemiesSetDifficulty myDifficulty { get; private set; }

        #region UNITYMETHODS
        protected override void Awake()
        {
            base.Awake();
            m_EnemiesSetDifficultList = enemy.enemiesSetDifficultyListSo;
            if (m_EnemiesSetDifficultList != null)
                myDifficulty = m_EnemiesSetDifficultList.GetDifficultByEnemyDifficult(actualDifficultName);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            ChangeDifficulty();
            EventDestroyObject += ChangeDifficulty;
        }

        void Start()
        {
            ChangeDifficulty();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDestroyObject -= ChangeDifficulty;
        }
  #endregion
        void ChangeDifficulty()
        {
            if (m_EnemiesSetDifficultList != null)
            {
                myDifficulty = enemy.enemiesSetDifficultyListSo.GetDifficultByScore(gamePlayManager.HighScorePlayers());
            }
        }*/

    }
}
