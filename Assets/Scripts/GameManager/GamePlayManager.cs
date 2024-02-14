using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    public sealed class GamePlayManager : Singleton<GamePlayManager>
    {
        [Header("Panels Settings")]
        [SerializeField]
        internal PanelMenuGame panelMenuGame;

        [Header("Level Settings")]
        [SerializeField] internal bool completePath;
        [SerializeField] internal bool readyToFinish;
        [Header("Debug Settings")]
        [SerializeField]
        private bool godMode;
        [SerializeField] public bool godModeSpeed;
        
        [SerializeField] internal GamePlayingLog gamePlayingLog;
        internal bool getGodMode => godMode;

        [Header("Power Up References")]
        public CollectibleScriptable refillBomb;

        internal Levels actualLevels;
        internal bool playerDead;
        internal bool bossFight;
        internal bool bossFightPause;
        internal BossMaster bossMaster;

        internal const float LimitX = 28.0f;
        internal const float LimitZTop = 40.0f;
        internal const float LimitZBottom = 15.0f;

        internal static readonly Vector2 ScreenLimitMin = new Vector2(-LimitX, LimitZBottom);
        internal static readonly Vector2 ScreenLimitMax = new Vector2(LimitX, LimitZTop);

        private GameManager m_GameManager;
        private PlayerManager m_PlayerManager;
        internal PlayersInputActions inputSystem;

        private int m_Score;

        #region Delegates
        public delegate void GeneralEventHandler();
        internal event GeneralEventHandler EventActivateEnemiesMaster;
        internal event GeneralEventHandler EventDeactivateEnemiesMaster;
        internal event GeneralEventHandler EventReSpawnEnemiesMaster;
        internal event GeneralEventHandler EventEnemiesMasterKillPlayer;
        internal event GeneralEventHandler EventOtherEnemiesKillPlayer;
        internal event GeneralEventHandler EventEnemiesMasterForceRespawn;
        internal event GeneralEventHandler EventPlayerPushButtonShoot;
        internal event GeneralEventHandler EventPlayerPushButtonBomb;
        internal delegate void UiUpdateEventHandler(int value);
        internal event GeneralEventHandler EventStartRapidFire;
        internal event GeneralEventHandler EventEndRapidFire;
        internal event UiUpdateEventHandler EventUpdateScore;
        internal event UiUpdateEventHandler EventUpdateDistance;
        internal event UiUpdateEventHandler EventUpdateRefugees;
        internal event UiUpdateEventHandler EventUpdateBombs;
        internal event UiUpdateEventHandler EventUpdateLives;

        internal delegate void BuildPatchHandler(float position);
        internal event BuildPatchHandler EventBuildPathUpdate;

        internal delegate void UpdatePowerUpHandler(PowerUp powerUp, float timer);
        internal event UpdatePowerUpHandler EventUpdatePowerUpDuration;

        #endregion

        #region UNITYMETHODS

        private void Awake()
        {
            if (FindObjectsOfType(typeof(GamePlayManager)).Length > 1)
            {
                gameObject.SetActive(false);
                Destroy(this);
            }
            m_GameManager = GameManager.instance;
            m_PlayerManager = PlayerManager.instance;
            actualLevels = m_GameManager.GetLevel();
            if (actualLevels.bossFight)
            {
                bossFight = actualLevels.bossFight;
            }
            inputSystem = new PlayersInputActions();
        }

        private void Start()
        {
            inputSystem.Player.Pause.performed += PauseGamePlay;
            inputSystem.UI_Controlls.Pause.performed += UnPauseGamePlay;
        }

        protected override void OnDestroy()
        {
            DestroyImmediate(m_PlayerManager);
            StopAllCoroutines();
            //base.OnDestroy();
        }
  #endregion
        public bool shouldBePlayingGame => m_GameManager.currentGameState is GameStatePlayGame or GameStatePlayGameBoss && !completePath;

        public static GameSettings getGameSettings => GameManager.instance.gameSettings;

        public void OnStartGame()
        {
            StartCoroutine(StartGamePlay(.5f));
        }
        public void OnRestartGame()
        {
            StartCoroutine(StartGamePlay());
        }

        private IEnumerator StartGamePlay(float timeWait = 2f)
        {
            yield return new WaitForSeconds(timeWait);
            //m_PlayerManager.ActivePlayers(true);
            m_PlayerManager.UnPausedMovementPlayers();
            OnEventActivateEnemiesMaster();
        }
        public void GameOverMenu()
        {
            panelMenuGame.SetMenuGameOver();
        }

        private void PauseGamePlay(InputAction.CallbackContext callbackContext)
        {
            if (GameManager.instance.onLoadScene || 
                m_PlayerManager.initializedPlayerMasters[0].isPlayerDead) return;
            if (GameManager.instance.currentGameState is not (GameStatePlayGame or GameStatePlayGameBoss))
                return;
            GameManager.instance.ChangeState(new GameStatePause());
        }
        private static void UnPauseGamePlay(InputAction.CallbackContext callbackContext)
        {
            UnPauseGamePlay();
        }
        internal static void UnPauseGamePlay()
        {
            var gameManager = GameManager.instance;
            if (gameManager.onLoadScene) return;
            if (gameManager.currentGameState is not GameStatePause)
                return;
            if (gameManager.lastGameState is GameStatePlayGame)
            {
                gameManager.ChangeState(new GameStatePlayGame());
            }
            else if (gameManager.lastGameState is GameStatePlayGameBoss)
            {
                gameManager.ChangeState(new GameStatePlayGameBoss());
            }
            
        }
        internal void PauseBossBattle(bool pause)
        {
            if (pause)
            {
                panelMenuGame.PauseMenu(true);
                Time.timeScale = 0;
                bossFightPause = true;
            }
            else
            {
                panelMenuGame.PauseMenu(false);
                Time.timeScale = 1;
                bossFightPause = false;
            }
        }

        public static void AddResultList(List<LogResults> list, PlayerSettings playerSettings, EnemiesScriptable enemy, int qnt, CollisionType collisionType)
        {
            var itemResults = list.Find(x => x.player == playerSettings && x.enemy == enemy && x.collisionType == collisionType);
            if (itemResults != null)
            {
                if (enemy is CollectibleScriptable collectibles)
                {
                    if (collectibles.maxCollectible == 0 || itemResults.quantity + qnt < collectibles.maxCollectible)
                        itemResults.quantity += qnt;
                    else
                        itemResults.quantity = collectibles.maxCollectible;
                }
                else
                    itemResults.quantity += qnt;
            }
            else
            {
                var newItemResults = new LogResults(playerSettings, enemy, qnt, collisionType);
                list.Add(newItemResults);
            }
        }
        

        #region Calls
        internal void OnEventActivateEnemiesMaster()
        {
            EventActivateEnemiesMaster?.Invoke();
        }
        internal void OnEventDeactivateEnemiesMaster()
        {
            EventDeactivateEnemiesMaster?.Invoke();
        }
        internal void OnEventReSpawnEnemiesMaster()
        {
            EventReSpawnEnemiesMaster?.Invoke();
        }
        internal void OnEventEnemiesMasterKillPlayer()
        {
            EventEnemiesMasterKillPlayer?.Invoke();
        }
        internal void OnEventOtherEnemiesKillPlayer()
        {
            EventOtherEnemiesKillPlayer?.Invoke();
        }
        internal void OnEventPlayerPushButtonShoot()
        {
            EventPlayerPushButtonShoot?.Invoke();
        }
        internal void OnEventPlayerPushButtonBomb()
        {
            EventPlayerPushButtonBomb?.Invoke();
        }
        internal void OnEventUpdateScore(int value)
        {
            //GameSteamManager.UpdateScore(6200, false);
            EventUpdateScore?.Invoke(value);
        }
        internal void OnEventUpdateDistance(int value)
        {
            EventUpdateDistance?.Invoke(value);
        }
        internal void OnEventUpdateRefugees(int value)
        {
            EventUpdateRefugees?.Invoke(value);
        }
        internal void OnEventUpdateBombs(int value)
        {
            EventUpdateBombs?.Invoke(value);
        }
        internal void OnEventUpdateLives(int value)
        {
            EventUpdateLives?.Invoke(value);
        }

        internal void OnEventBuildPathUpdate(float position)
        {
            EventBuildPathUpdate?.Invoke(position);
        }
        internal void OnEventRapidFire()
        {
            EventStartRapidFire?.Invoke();
        }
        internal void OnEventEndRapidFire()
        {
            EventEndRapidFire?.Invoke();
        }
        internal void OnEventEnemiesMasterForceRespawn()
        {
            EventEnemiesMasterForceRespawn?.Invoke();
        }

        internal void OnEventUpdatePowerUpDuration(PowerUp powerUp, float f)
        {
            EventUpdatePowerUpDuration?.Invoke(powerUp,f);
        }
  #endregion
        
    }
}
