using UnityEngine;

namespace RiverAttack
{
    public class PlayerLives : MonoBehaviour
    {
        PlayerMaster m_PlayerMaster;
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;
        GamePlayingLog m_GamePlayingLog;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerMasterHit += LoseLive;
            m_GamePlayManager.EventUpdateScore += CheckNewLives;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterHit -= LoseLive;
            m_GamePlayManager.EventUpdateScore -= CheckNewLives;
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;

            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlayingLog = m_GamePlayManager.gamePlayingLog;
        }
        void LoseLive()
        {
            m_PlayerSettings.lives -= 1;
            LogGamePlay(1);
            m_GamePlayManager.OnEventUpdateLives(m_PlayerSettings.lives);
        }
        void CheckNewLives(int score)
        {
            int scoreToLive = GameSettings.instance.multiplyScoreForLives;
            if (score < m_PlayerMaster.nextScoreForLive) return;
            if (m_PlayerSettings.lives > GameSettings.instance.maxLives) return;
            m_PlayerMaster.nextScoreForLive += scoreToLive;
            m_PlayerSettings.lives++;
            m_GamePlayManager.OnEventUpdateLives(m_PlayerSettings.lives);
        }
        void LogGamePlay(int live)
        {
            m_GamePlayingLog.livesSpent += live;
        }
    }
}
