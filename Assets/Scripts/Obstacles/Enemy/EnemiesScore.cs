using UnityEngine;

namespace RiverAttack
{
    public class EnemiesScore : MonoBehaviour
    {
        protected GamePlayManager gamePlayManager;
        private ObstacleMaster m_ObstacleMaster;
        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            m_ObstacleMaster.EventObstacleScore += SetScore;
        }
        protected virtual void OnDisable()
        {
            m_ObstacleMaster.EventObstacleScore -= SetScore;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            m_ObstacleMaster = GetComponent<ObstacleMaster>();
            gamePlayManager = GamePlayManager.instance;
        }

        private void SetScore(PlayerSettings playerSettings)
        {
            float score = m_ObstacleMaster.enemy.enemyScore;
            if (score == 0) return;
            var enemyMaster = m_ObstacleMaster as EnemiesMaster;
            if (enemyMaster != null && EnemiesMaster.myDifficulty.multiplyScore > 0)
            {
                var myDifficulty = EnemiesMaster.myDifficulty;
                if (myDifficulty.multiplyScore > 0)
                    score *= myDifficulty.multiplyScore;
            }
            if (playerSettings == null) return;
            playerSettings.score += (int)(score);
            gamePlayManager.OnEventUpdateScore(playerSettings.score);
            LogGamePlay(playerSettings.score);
        }
        protected static void LogGamePlay(int score)
        {
            if (score > GamePlayingLog.instance.totalScore)
                GamePlayingLog.instance.totalScore = score;
        }

    }
}
