﻿using UnityEngine;

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
        
        /*
        [SerializeField] int playerStartLives = 3;
        [SerializeField] int playerLivesMax = 9;
        [SerializeField] float timeoutReSpawn = 1.8f;
        [SerializeField] bool reSpawnInSavePoint = false;
    #region Variable Private Inspector
        PlayerMaster m_PlayerMaster;
        PlayerSettings m_PlayerSettings;
        GamePlayManager m_GamePlayMaster;

        [SerializeField]
        int scoreForExtraLife;
        int m_Score;
        [SerializeField] int playerLives;
    #endregion

    #region UNITY METHODS
        void Start()
        {
            m_PlayerSettings.maxLives = playerLivesMax;
            m_PlayerSettings.startLives = playerStartLives;
            m_PlayerSettings.lives = playerStartLives;
            playerLives = playerStartLives;

            m_Score = m_PlayerSettings.score + scoreForExtraLife;
        }
        
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerMasterOnDestroy += PlayerLivesKill;
            //m_PlayerMaster.EventPlayerMasterCollider += GainExtraLive;
            //m_GamePlayMaster.EventRestartPlayer += RevivePlayer;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterOnDestroy -= PlayerLivesKill;
            //m_PlayerMaster.EventPlayerMasterCollider -= GainExtraLive;
            //m_GamePlayMaster.EventRestartPlayer -= RevivePlayer;
        }
  #endregion

        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.GetPlayersSettings();
            m_GamePlayMaster = GamePlayManager.instance;
        }
        

        void PlayerLivesKill()
        {
            ChangeLife(-1);
            if (m_PlayerSettings.lives <= 0 && !GameManager.instance.GetGameOver())
            {
                GameManager.instance.ChangeStatesGamePlay(GameManager.States.GameOver);
                m_PlayerMaster.SetActualPlayerStateMovement(PlayerMaster.MovementStatus.Paused);
                m_GamePlayMaster.CallEventGameOver();
            }
            else
            {
                m_GamePlayMaster.CallEventEnemyDestroyPlayer();
                StopAllCoroutines();
                StartCoroutine(ReSpawn());
                Debug.Log("Chamou corrotina respawn");
            }
        }

        void SetRespawnPosition(float positionZ)
        {
            var transform1 = transform;
            transform1.position = reSpawnInSavePoint ? new Vector3(0f, transform1.position.y, positionZ) : new Vector3(0f, transform1.position.y, m_PlayerMaster.GetLastSavePosition());
        }

        IEnumerator ReSpawn()
        {
            m_GamePlayMaster.SetGamePlayPause(true);
            m_PlayerMaster.SetActualPlayerStateMovement(PlayerMaster.MovementStatus.Paused);
            yield return new WaitForSeconds(timeoutReSpawn);
            /*if (!m_GamePlayMaster.shouldBePlayingGame)
                yield break;#1#
            SetRespawnPosition(transform.localPosition.z);

            m_PlayerMaster.CallEventPlayerMasterReSpawn();
            m_GamePlayMaster.CallEventResetEnemies();

            m_PlayerMaster.SetActualPlayerStateMovement(PlayerMaster.MovementStatus.None);
            m_PlayerMaster.SetPlayerReady();
            m_GamePlayMaster.CallEventUnPausePlayGame();
        }
        void GainExtraLive()
        {
            if (m_PlayerSettings.score < m_Score) return;
            int rest = (int)m_PlayerSettings.score - m_Score;
            int life = 1;
            if (rest >= scoreForExtraLife) life += rest / scoreForExtraLife;
            ChangeLife(life);
            m_Score = (m_PlayerSettings.score - rest) + scoreForExtraLife * life;
            m_PlayerMaster.CallEventPlayerAddLive();
        }
        void ChangeLife(int life)
        {
            if (m_PlayerSettings.maxLives != 0 && m_PlayerSettings.lives + life >= m_PlayerSettings.maxLives)
                m_PlayerSettings.lives = m_PlayerSettings.maxLives;
            else if (m_PlayerSettings.lives + life <= 0)
                playerLives = m_PlayerSettings.lives = 0;
            else
                playerLives = m_PlayerSettings.lives += life;
            GamePlaySettings.instance.livesSpent += life * 1;
        }*/
        
    }
}
