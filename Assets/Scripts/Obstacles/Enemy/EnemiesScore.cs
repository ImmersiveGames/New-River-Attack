using UnityEngine;

namespace RiverAttack
{
    public class EnemiesScore : MonoBehaviour
    {
        protected GamePlayManager gamePlayManager;
        private ObstacleMasterOld _mObstacleMasterOld;
        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            _mObstacleMasterOld.EventObstacleScore += SetScore;
        }
        protected virtual void OnDisable()
        {
            _mObstacleMasterOld.EventObstacleScore -= SetScore;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            _mObstacleMasterOld = GetComponent<ObstacleMasterOld>();
            gamePlayManager = GamePlayManager.instance;
        }

        private void SetScore(PlayerSettings playerSettings)
        {
            float score = _mObstacleMasterOld.enemy.enemyScore;
            if (score == 0) return;
            var enemyMaster = _mObstacleMasterOld as EnemiesMasterOld;
            if (enemyMaster != null && EnemiesMasterOld.myDifficulty.multiplyScore > 0)
            {
                var myDifficulty = EnemiesMasterOld.myDifficulty;
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
