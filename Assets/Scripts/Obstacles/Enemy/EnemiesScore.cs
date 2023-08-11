using UnityEngine;

namespace RiverAttack
{
    public class EnemiesScore : MonoBehaviour
    {
        /*GamePlayManager m_GamePlayManager;
        protected ObstacleMaster obstacleMaster;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            obstacleMaster.EventPlayerDestroyObject += SetScore;
        }
        protected virtual void OnDisable()
        {
            obstacleMaster.EventPlayerDestroyObject -= SetScore;
        }
  #endregion

        protected virtual void SetInitialReferences()
        {
            obstacleMaster = GetComponent<ObstacleMaster>();
            m_GamePlayManager = GamePlayManager.instance;

        }
        void SetScore(PlayerMaster playerMaster)
        {
            float score = obstacleMaster.enemy.enemyScore;
            if (EnemiesMaster.myDifficulty.multiplyScore > 0)
            {
                var myDifficulty = EnemiesMaster.myDifficulty;
                if (myDifficulty.multiplyScore > 0)
                    score *= myDifficulty.multiplyScore;
            }
            if (playerMaster == null) return;
            playerMaster.GetPlayersSettings().score += (int)(score);
            m_GamePlayManager.CallEventUIScore(playerMaster.GetPlayersSettings().score);
        }
        void Log(int score)
        {
            if (score > GamePlaySettings.instance.totalScore)
                GamePlaySettings.instance.totalScore = score;
        }*/
        
    }
}
