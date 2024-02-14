using UnityEngine;

namespace RiverAttack
{
    public class PlayerLives : MonoBehaviour
    {
        private PlayerMaster m_PlayerMaster;
        private GamePlayManager m_GamePlayManager;
        private PlayerSettings m_PlayerSettings;
        private GamePlayingLog m_GamePlayingLog;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerMasterHit += LoseLive;
            m_GamePlayManager.EventUpdateScore += CheckNewLives;
        }

        private void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterHit -= LoseLive;
            m_GamePlayManager.EventUpdateScore -= CheckNewLives;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;

            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlayingLog = m_GamePlayManager.gamePlayingLog;
        }

        private void LoseLive()
        {
            m_PlayerSettings.lives -= 1;
            LogGamePlay(1);
            m_GamePlayManager.OnEventUpdateLives(m_PlayerSettings.lives);
        }

        private void CheckNewLives(int score)
        {
            int scoreToLive = GameSettings.instance.multiplyScoreForLives;
            if (score < m_PlayerMaster.nextScoreForLive) return;
            if (m_PlayerSettings.lives > GameSettings.instance.maxLives) return;
            m_PlayerMaster.nextScoreForLive += scoreToLive;
            m_PlayerSettings.lives++;
            m_GamePlayManager.OnEventUpdateLives(m_PlayerSettings.lives);
        }

        private void LogGamePlay(int live)
        {
            m_GamePlayingLog.livesSpent += live;
        }
    }
}
