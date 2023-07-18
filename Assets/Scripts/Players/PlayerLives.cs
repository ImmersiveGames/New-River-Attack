using System;
using System.Collections;
using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerLives : MonoBehaviour
    {
        [SerializeField]
        private float timeoutReSpawn = 1.8f;
    #region Variable Private Inspector
        private PlayerMaster m_PlayerMaster;
        private PlayerStats m_PlayerStats;
        private GamePlayManager m_GamePlayMaster;

        [SerializeField]
        private int scoreForExtraLife;
        private int m_Score;
    #endregion

    #region UNITYMETHODS
        void Start()
        {
            m_Score = (int)m_PlayerStats.score + scoreForExtraLife;
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

        private void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerStats = m_PlayerMaster.GetPlayersSettings();
            m_GamePlayMaster = GamePlayManager.instance;
        }
        private void AddLives(int newLives)
        {
            if (m_PlayerStats.maxLives > 0 && (m_PlayerStats.lives + newLives) > m_PlayerStats.maxLives)
                m_PlayerStats.lives += m_PlayerStats.maxLives;
            else
                m_PlayerStats.lives += newLives;
            m_PlayerMaster.CallEventPlayerAddLive();
        }

        private void KillPlayer()
        {
            m_PlayerStats.ChangeLife(-1);
            LogLives(1);
            if (m_PlayerStats.lives <= 0 && !GameManager.instance.GetGameOver())
            {
                m_GamePlayMaster.CallEventGameOver();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ReSpawn());
            }
        }

        public void RevivePlayer(int numLives)
        {
            m_PlayerStats.ChangeLife(numLives);
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
            if (m_PlayerStats.score < m_Score) return;
            int rest = (int)m_PlayerStats.score - m_Score;
            int life = 1;
            if (rest >= scoreForExtraLife) life += rest / scoreForExtraLife;
            AddLives(life);
            m_Score = ((int)m_PlayerStats.score - rest) + scoreForExtraLife * life;
        }

        static void LogLives(int lives)
        {
            GamePlaySettings.instance.livesSpents += Mathf.Abs(lives);
        }


    }
}
