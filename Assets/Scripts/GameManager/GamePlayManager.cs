using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace RiverAttack
{
    public class GamePlayManager : Singleton<GamePlayManager>
    {
        [SerializeField] internal bool completePath;
        [SerializeField]
        bool godMode;
        [SerializeField]
        GameSettings gameSettings;
        internal bool getGodMode { get { return godMode; } }
        [SerializeField]
        internal GamePlaySettings gamePlaySettings;

        protected internal bool playerDead;

        GameManager m_GameManager;
        PlayersInputActions m_InputSystem;
        #region Delegates
        public delegate void GeneralEventHandler();
        internal event GeneralEventHandler EventActivateEnemiesMaster;
        internal event GeneralEventHandler EventDeactivateEnemiesMaster;
        internal event GeneralEventHandler EventReSpawnEnemiesMaster;
        internal event GeneralEventHandler EventEnemiesMasterKillPlayer;
        internal delegate void UiUpdateEventHandler(int value);
        internal event UiUpdateEventHandler EventUpdateScore;
        internal event UiUpdateEventHandler EventUpdateDistance;
        internal event UiUpdateEventHandler EventUpdateRefugees;
        internal event UiUpdateEventHandler EventUpdateBombs;
        internal event UiUpdateEventHandler EventUpdateLives;
        
        public delegate void CollectableEventHandle(CollectibleScriptable collectibles);
        public event CollectableEventHandle EventCollectItem;
        
        public delegate void ShakeCamEventHandle(float power, float inTime);
        public event ShakeCamEventHandle EventShakeCam;
        
        #endregion

        #region UNITYMETHODS
        void OnEnable()
        {
            m_GameManager = GameManager.instance;
        }
  #endregion
        public GameSettings getGameSettings
        {
            get { return gameSettings; }
        }
        
        public bool shouldBePlayingGame { get { return (m_GameManager.currentGameState is GameStatePlayGame  && !completePath); } }

        public PlayerSettings GetNoPlayerPlayerSettings(int playerIndex = 0)
        {
            return m_GameManager.playerSettingsList.Count > 0 ? m_GameManager.playerSettingsList[playerIndex] : null;
        }
        public static void AddResultList(List<LogResults> list, PlayerSettings playerSettings, EnemiesScriptable enemy, int qnt, CollisionType collisionType)
        {
            var itemResults = list.Find(x => x.player == playerSettings && x.enemy == enemy && x.collisionType == collisionType);
            if (itemResults != null)
            {
                if (enemy is CollectibleScriptable collectibles)
                {
                    if (itemResults.quantity + qnt < collectibles.maxCollectible)
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
        public int HighScorePlayers()
        {
            if (m_GameManager.haveAnyPlayerInitialized == false) return 0;
            int score = 0;
            foreach (var pl in m_GameManager.initializedPlayerMasters.Where(pl => score < pl.GetComponent<PlayerMaster>().getPlayerSettings.score))
            {
                score += (int)pl.GetComponent<PlayerMaster>().getPlayerSettings.score;
            }
            return score;
        }

        #region Calls
        protected internal void OnEventActivateEnemiesMaster()
        {
            EventActivateEnemiesMaster?.Invoke();
        }
        protected internal void OnEventDeactivateEnemiesMaster()
        {
            EventDeactivateEnemiesMaster?.Invoke();
        }
        protected internal void OnEventReSpawnEnemiesMaster()
        {
            EventReSpawnEnemiesMaster?.Invoke();
        }
        protected internal void OnEventEnemiesMasterKillPlayer()
        {
            EventEnemiesMasterKillPlayer?.Invoke();
        }
        protected internal void OnEventUpdateScore(int value)
        {
            EventUpdateScore?.Invoke(value);
        }
        protected internal void OnEventUpdateDistance(int value)
        {
            EventUpdateDistance?.Invoke(value);
        }
        protected internal void OnEventUpdateRefugees(int value)
        {
            EventUpdateRefugees?.Invoke(value);
        }
        protected internal void OnEventUpdateBombs(int value)
        {
            EventUpdateBombs?.Invoke(value);
        }
        protected internal void OnEventUpdateLives(int value)
        {
            EventUpdateLives?.Invoke(value);
        }
        protected internal void OnEventCollectItem(CollectibleScriptable collectibles)
        {
            EventCollectItem?.Invoke(collectibles);
        }
        protected internal void OnEventShakeCam(float power, float intime)
        {
            EventShakeCam?.Invoke(power, intime);
        }
  #endregion
        /*
        [SerializeField]
        bool gameBeat;
        [SerializeField]
        bool gamePlayPaused;
        [SerializeField]
        bool completePath;
        [SerializeField]
        bool godMode;
        public bool getGodMode { get { return godMode; } }
        public bool isGamePlayPause
        {
            get { return gamePlayPaused; }
            set
            {
                gamePlayPaused = value;
            }
        }
        public bool isGameBeat { get { return gameBeat; } }

        [Header("Level Settings"), SerializeField]
        public List<Levels> levelsFinish = new List<Levels>();
        [SerializeField]
        public Levels actualLevel;
        [SerializeField]
        Levels classicLevel;
        [SerializeField]
        GameObject prefabPlayer;
        public List<Transform> listPlayer { get;
            private set; }

        public List<PlayerMaster> playersMasterList;
        int m_ActualPath;

        PlayersInputActions m_InputSystem;

        GameManager m_GameManager;
        GameSettings m_GameSettings;
        GamePlayAudio m_GamePlayAudio;
        public GamePlayAudio.LevelType actualBGM { get; set; }

    #region Delegates
        public delegate void GamePlayManagerEventHandler();
        public event GamePlayManagerEventHandler EventStartPlayGame;
        public event GamePlayManagerEventHandler EventPausePlayGame;
        public event GamePlayManagerEventHandler EventUnPausePlayGame;
        public event GamePlayManagerEventHandler EventCompletePath;
        public event GamePlayManagerEventHandler EventCompleteGame;
        public event GamePlayManagerEventHandler EventGameOver;
        public event GamePlayManagerEventHandler EventShouldContinue;
        public event GamePlayManagerEventHandler EventStartPath;
        public event GamePlayManagerEventHandler EventEnemyDestroyPlayer;
        public event GamePlayManagerEventHandler EventResetEnemies;
        public event GamePlayManagerEventHandler EventResetPlayers;
        public event GamePlayManagerEventHandler EventUICollectable;
        
        public delegate void PlayerPositionEventHandler(Vector3 position);
        public event PlayerPositionEventHandler EventCheckPoint;
        public event PlayerPositionEventHandler EventCheckPlayerPosition;
        public delegate void PowerUpEventHandler(bool active);
        public event PowerUpEventHandler EventRapidFire;
        public delegate void GameManagerRestartPlayer(int lives);
        public event GameManagerRestartPlayer EventRestartPlayer;
        public delegate void GameManagerUIEventHandler(int value);
        public event GameManagerUIEventHandler EventUIScore;
        
        public delegate void CollectableEventHandle(CollectibleScriptable collectibles);
        public event CollectableEventHandle EventCollectItem;
        public delegate void ShakeCamEventHandle(float power, float inTime);
        public event ShakeCamEventHandle EventShakeCam;
    #endregion
    
#region UnityMethods
        void Awake()
        {
            playersMasterList = new List<PlayerMaster>();
            gamePlayPaused = true;
            gameBeat = false;
            completePath = false;
        }
        void OnEnable()
        {
            SetInitialReferences();
            var levelRoot = new GameObject();
            m_ActualPath = 0;
            if (actualLevel != null)
            {
                levelRoot.name = actualLevel.levelName;
                actualLevel.CreateLevel(levelRoot.transform);
                actualBGM = actualLevel.startLevelBGM;
                m_GamePlayAudio.levelType = actualBGM;
                m_GamePlayAudio.PlayBGM(actualBGM); 
            }
            //SpawnPlayers(numPlayers.Length);
            EventStartPlayGame += StartPlayingGame;
            EventCheckPlayerPosition += CheckPoolLevel;
            //m_GameplayDefault.Add("config_gamePlay_godmod", false);
        }
        
        void OnDisable()
        {
            EventStartPlayGame -= StartPlayingGame;
            EventCheckPlayerPosition -= CheckPoolLevel;
            m_GamePlayAudio.StopBGM();
            StopAllCoroutines();
        }
#endregion
        void SetInitialReferences()
        {
            m_GameManager = GameManager.instance;
            m_GameSettings = GameSettings.instance;
            m_GamePlayAudio = GamePlayAudio.instance;
            /*if (m_GameSettings.gameMode.modeId == m_GameSettings.GetGameModes(0).modeId)
            {
                m_GameManager.actualLevel = classicLevel;
            }#1#
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.Player.Enable();
            m_InputSystem.Player.Pause.performed += ctx => ControllerButtonPauseGame();
        }

        void StartPlayingGame()
        {
            gamePlayPaused = false;
        }
        public bool shouldBePlayingGame
        {
            get
            {
                return (!m_GameManager.isGameOver && !completePath && !m_GameManager.isGameStopped && !gamePlayPaused);
            }
        }
 
        public void SetGamePlayPause(bool setPause)
        {
            gamePlayPaused = setPause;
        }

        public void AddPlayerToGamePlayManager(PlayerMaster playerMaster)
        {
            playersMasterList.Add(playerMaster);
        }

        public PlayerMaster GetPlayerMasterByIndex(int indexPlayer)
        {
            return playersMasterList.Count <= 0 ? null : playersMasterList[indexPlayer];
        }

        public int HighScorePlayers()
        {
            if (listPlayer is not { Count: > 0 }) return 0;
            int score = 0;
            foreach (var pl in listPlayer.Where(pl => score < pl.GetComponent<PlayerMaster>().GetPlayersSettings().score))
            {
                score = (int)pl.GetComponent<PlayerMaster>().GetPlayersSettings().score;
            }
            return score;
        }

        public void SaveAllPlayers()
        {
            Debug.Log("AQUI SALVA O JOGO");
            /*for (int i = 0; i < numPlayers.Length; i++)
            {
                GameManagerSaves.Instance.SavePlayer(numPlayers[i]);
            }#1#
        }

        public void ButtonPauseGame()
        {
            gamePlayPaused = !gamePlayPaused;
            if(gamePlayPaused)
                CallEventPausePlayGame();
            else
                CallEventUnPausePlayGame();
        }

        private void ControllerButtonPauseGame() 
        {          
            if (m_GameManager.GetActualGameState() != GameManager.States.GamePlay) return;

            if (gamePlayPaused) return;

            var pauseBtn = m_GameManager.pauseButton.gameObject.GetComponent<Button>();
            pauseBtn.onClick.Invoke();           
        }

        public bool haveAnyPlayMasterBeReady
        {
            get
            {
                bool anyReady = false;
                foreach (var playerMaster in playersMasterList)
                {
                    anyReady = playerMaster.ShouldPlayerBeReady();
                }
                return anyReady;
            }
        }
        
        public bool shouldBeInGameOver
        {
            get
            {
                return (m_GameManager.isGameOver && !m_GameManager.isGameStopped && !gamePlayPaused);
            }
        }

        public bool shouldBeFinishPath
        {
            get
            {
                return (completePath && !gamePlayPaused && !m_GameManager.isGameOver && !m_GameManager.isGameStopped);
            }
        }

        public bool shouldFinishGame
        {
            get
            {
                return (gameBeat && completePath && !m_GameManager.isGameOver && !m_GameManager.isGameStopped);
            }
        }
        #region Multiplayer
        public Transform GetPlayerById(int id)
        {
            return listPlayer[id];
        }
        private void MultiPlayerSpawns(int players)
        {
            listPlayer = new List<Transform>();
            for (int i = 0; i < players; i++)
            {
                var player = Instantiate(prefabPlayer);
                listPlayer.Add(player.transform);
                listPlayer[i].gameObject.SetActive(true);
                listPlayer[i].name = "Player" + i;
                //listPlayer[i].GetComponent<PlayerMaster>().Init(m_GameManager.playerObjectAvailableList[i], i);
            }
        }
        public PlayerMaster GetPlayerMasterByMultiPlayerId(int id)
        {
            return listPlayer[id].GetComponent<PlayerMaster>();
        }
        public PlayerSettings GetPlayerSettingsByMultiPlayerId(int id)
        {
            return listPlayer[id].GetComponent<PlayerMaster>().GetPlayersSettings();
        }
        public void SetReadyMultiPlayer(bool ready)
        {
            foreach (var t in listPlayer)
            {
                t.GetComponent<PlayerMaster>().SetPlayerReady();
            }
        }
        #endregion
        #region LevelBuilder
        public Levels GetActualLevel()
        {
            return actualLevel;
        }

        public int GetActualPath()
        {
            return m_ActualPath;
        }
        void CheckPoolLevel(Vector3 pos)
        {
            if (completePath || !((actualLevel.levelMilestones[m_ActualPath] - pos).z <= 0)) return;
            actualLevel.CallUpdatePoolLevel(m_ActualPath);
            m_ActualPath++;
        }
        public bool shouldStartPath
        {
            get
            {
                return (gamePlayPaused && !m_GameManager.isGameStopped && !m_GameManager.isGameOver && !gameBeat && !completePath);
            }
        }
    #endregion

        #region Calls
        public void CallEventStartPlayGame()
        {
            EventStartPlayGame?.Invoke();
        }
        public void CallEventResetEnemies()
        {
            EventResetEnemies?.Invoke();
        }
        public void CallEventEnemyDestroyPlayer()
        {
            EventEnemyDestroyPlayer?.Invoke();
        }
        public void CallEventUIScore(int score)
        {
            EventUIScore?.Invoke(score);
        }
        
        //Old Call
        public void CallEventPausePlayGame()
        {
            gamePlayPaused = true;
            EventPausePlayGame?.Invoke();
        }

        public void CallEventUnPausePlayGame()
        {
            gamePlayPaused = false;
            EventUnPausePlayGame?.Invoke();
        }

        public void CallEventCompletePath()
        {
            completePath = true;
            EventCompletePath?.Invoke();
        }

        public void CallEventCompleteGame()
        {
            gameBeat = true;
            EventCompleteGame?.Invoke();
        }
        public void CallEventGameOver()
        {
            m_GameManager.isGameOver = true;
            GamePlayAudio.instance.StopBGM();
            EventGameOver?.Invoke();
        }

        public void CallEventShouldContinue()
        {
            EventShouldContinue?.Invoke();
        }

        public void CallEventStartPath()
        {
            EventStartPath?.Invoke();
        }

        

        public void CallEventResetPlayers()
        {
            EventResetPlayers?.Invoke();
        }

        

        public void CallEventCheckPoint(Vector3 position)
        {
            EventCheckPoint?.Invoke(position);
        }

        public void CallEventCheckPlayerPosition(Vector3 position)
        {
            EventCheckPlayerPosition?.Invoke(position);
        }

        public void CallEventRapidFire(bool active)
        {
            EventRapidFire?.Invoke(active);
        }
        public void CallEventCollectable(CollectibleScriptable collectibles)
        {
            EventCollectItem?.Invoke(collectibles);
            EventUICollectable?.Invoke();
        }

        public void CallEventRestartPlayer(int lives)
        {
            m_GameManager.SetupGame();
            EventRestartPlayer?.Invoke(lives);
        }
        public void CallEventShakeCam(float power, float inTime)
        {
            EventShakeCam?.Invoke(power, inTime);
        }
        #endregion
        */

        
    }
}
