using UnityEngine;

namespace RiverAttack
{
    public class PlayerLives : MonoBehaviour
    {
        private PlayerMasterOld _mPlayerMasterOld;
        private GamePlayManager m_GamePlayManager;
        private PlayerSettings m_PlayerSettings;
        private GamePlayingLog m_GamePlayingLog;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            _mPlayerMasterOld.EventPlayerMasterHit += LoseLive;
            m_GamePlayManager.EventUpdateScore += CheckNewLives;
        }

        private void OnDisable()
        {
            _mPlayerMasterOld.EventPlayerMasterHit -= LoseLive;
            m_GamePlayManager.EventUpdateScore -= CheckNewLives;
        }
  #endregion

  private void SetInitialReferences()
        {
            _mPlayerMasterOld = GetComponent<PlayerMasterOld>();
            m_PlayerSettings = _mPlayerMasterOld.getPlayerSettings;

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
            if (score < _mPlayerMasterOld.nextScoreForLive) return;
            if (m_PlayerSettings.lives > GameSettings.instance.maxLives) return;
            _mPlayerMasterOld.nextScoreForLive += scoreToLive;
            m_PlayerSettings.lives++;
            m_GamePlayManager.OnEventUpdateLives(m_PlayerSettings.lives);
        }

        private void LogGamePlay(int live)
        {
            m_GamePlayingLog.livesSpent += live;
        }
    }
}
