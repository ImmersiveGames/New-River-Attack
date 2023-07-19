using System.Collections;
using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerLives : MonoBehaviour
    {
        [SerializeField]
        float timeoutReSpawn = 1.8f;
    #region Variable Private Inspector
        PlayerMaster m_PlayerMaster;
        PlayerSettings m_PlayerSettings;
        GamePlayManager m_GamePlayMaster;

        [SerializeField]
        int scoreForExtraLife;
        int m_Score;
    #endregion

    #region UNITY METHODS
        void Start()
        {
            m_Score = (int)m_PlayerSettings.score + scoreForExtraLife;
        }
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerDestroy += KillPlayer;
            m_PlayerMaster.EventPlayerHit += GainExtraLive;
            m_GamePlayMaster.EventRestartPlayer += RevivePlayer;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerDestroy -= KillPlayer;
            m_PlayerMaster.EventPlayerHit -= GainExtraLive;
            m_GamePlayMaster.EventRestartPlayer -= RevivePlayer;
        }
  #endregion

        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.GetPlayersSettings();
            m_GamePlayMaster = GamePlayManager.instance;
        }
        void AddLives(int newLives)
        {
            if (m_PlayerSettings.maxLives > 0 && (m_PlayerSettings.lives + newLives) > m_PlayerSettings.maxLives)
                m_PlayerSettings.lives += m_PlayerSettings.maxLives;
            else
                m_PlayerSettings.lives += newLives;
            m_PlayerMaster.CallEventPlayerAddLive();
        }

        void KillPlayer()
        {
            m_PlayerSettings.ChangeLife(-1);
            LogLives(1);
            if (m_PlayerSettings.lives <= 0 && !GameManager.instance.GetGameOver())
            {
                m_GamePlayMaster.CallEventGameOver();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ReSpawn());
            }
        }

        void RevivePlayer(int numLives)
        {
            m_PlayerSettings.ChangeLife(numLives);
            //StopAllCoroutines();
            StartCoroutine(ReSpawn());
        }

        IEnumerator ReSpawn()
        {
            yield return new WaitForSeconds(timeoutReSpawn);
            if (!m_GamePlayMaster.shouldBePlayingGame)
                yield break;
            m_PlayerMaster.CallEventPlayerReload();
            m_GamePlayMaster.CallEventResetPlayers();
            m_GamePlayMaster.CallEventResetEnemies();
            m_GamePlayMaster.GamePlayPause(false);
            //gamePlayMaster.CallEventUnPausePlayGame();
        }
        void GainExtraLive()
        {
            if (m_PlayerSettings.score < m_Score) return;
            int rest = (int)m_PlayerSettings.score - m_Score;
            int life = 1;
            if (rest >= scoreForExtraLife) life += rest / scoreForExtraLife;
            AddLives(life);
            m_Score = ((int)m_PlayerSettings.score - rest) + scoreForExtraLife * life;
        }
        static void LogLives(int lives)
        {
            GamePlaySettings.instance.livesSpent += Mathf.Abs(lives);
        }
    }
}
