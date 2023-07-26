using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public sealed class EnemiesDifficulty : MonoBehaviour
    {
        EnemiesMaster m_EnemyMaster;
        EnemiesSetDifficultyListSo m_EnemiesSetDifficultList;
        public EnemiesSetDifficulty myDifficulty { get; private set; }

        #region UNITY METHODS
        void Awake()
        {
            if (m_EnemiesSetDifficultList != null)
                myDifficulty = m_EnemiesSetDifficultList.enemiesSetDifficulties[0];
        }

        void OnEnable()
        {
            SetInitialReferences();
            //ChangeDifficulty();
        }

        void Start()
        {
            ChangeDifficulty();
            m_EnemyMaster.EventDestroyEnemy += ChangeDifficulty;
        }
        void OnDisable()
        {
            m_EnemyMaster.EventDestroyEnemy -= ChangeDifficulty;
        }
  #endregion

        void SetInitialReferences()
        {
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            m_EnemiesSetDifficultList = m_EnemyMaster.enemy.enemiesSetDifficultyListSo;
            if (m_EnemiesSetDifficultList != null)
                myDifficulty = m_EnemiesSetDifficultList.GetDifficultByEnemyDifficult(m_EnemyMaster.getDifficultName);
        }
        void ChangeDifficulty()
        {
            if (m_EnemiesSetDifficultList != null)
            {
                myDifficulty = m_EnemyMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByScore((int)(GamePlayManager.instance.HighScorePlayers()));
            }
        }

        
        public EnemiesSetDifficultyListSo GetDifficultList()
        {
            return m_EnemiesSetDifficultList;
        }
    }
}
