using UnityEngine;

namespace RiverAttack
{
    public class PlayerLives : MonoBehaviour
    {
        PlayerMaster m_PlayerMaster;
        GamePlayManager m_GamePlayManager;
        PlayerSettings m_PlayerSettings;
        GamePlaySettings m_GamePlaySettings;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerMasterHit += LoseLive;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterHit -= LoseLive;
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;

            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlaySettings = m_GamePlayManager.gamePlaySettings;
        }
        void LoseLive()
        {
            m_PlayerSettings.lives -= 1;
            LogGamePlay(1);
            m_GamePlayManager.OnEventUpdateLives(m_PlayerSettings.lives);
        }
        void LogGamePlay(int live)
        {
            m_GamePlaySettings.livesSpent += live;
        }

    }
}
