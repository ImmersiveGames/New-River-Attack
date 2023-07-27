using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public class EnemiesScore : MonoBehaviour
    {
        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemyMaster;
        EnemiesDifficulty m_EnemyDifficulties;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            m_EnemyMaster.EventPlayerDestroyEnemy += SetScore;
        }
        protected virtual void OnDisable()
        {
            m_EnemyMaster.EventPlayerDestroyEnemy -= SetScore;
        }
  #endregion
        
        protected virtual void SetInitialReferences()
        {
            m_EnemyMaster = GetComponent<EnemiesMaster>();
            m_GamePlayManager = GamePlayManager.instance;
            if (GetComponent<EnemiesDifficulty>())
            {
                m_EnemyDifficulties = GetComponent<EnemiesDifficulty>();
            }
        }
        void SetScore(PlayerMaster playerMaster)
        {
            float score = m_EnemyMaster.enemy.enemyScore;
            if (m_EnemyDifficulties != null)
            {
                var myDifficulty = m_EnemyDifficulties.myDifficulty;
                if (myDifficulty.multiplyScore > 0)
                    score *= myDifficulty.multiplyScore;
            }
            if (playerMaster == null) return;
            playerMaster.GetPlayersSettings().score += (int)(score);
            m_GamePlayManager.CallEventUIScore();
        }
        void Log(int score)
        {
            if (score > GamePlaySettings.instance.totalScore)
                GamePlaySettings.instance.totalScore = score;
        }
    }
}
