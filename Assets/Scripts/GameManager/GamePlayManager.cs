using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class GamePlayManager : Singleton<GamePlayManager>
    {
        [SerializeField]
        bool gameBeat;
        [SerializeField]
        bool gamePaused;
        [SerializeField]
        bool completePath;
        [SerializeField]
        bool godMode;
        [SerializeField]
        Levels actualLevel;

        public bool getGodMode { get { return godMode; } }
        public bool isGamePause { get { return gamePaused; } }
        public bool isGameBeat { get { return gameBeat; } }

        [Header("Level Settings"), SerializeField]
        Levels classicLevel;
        [SerializeField]
        PlayerSettings[] numPlayers;
        [SerializeField]
        GameObject prefabPlayer;
        public List<GameObject> listPlayer { get;
            private set; }
        int m_ActualPath;
        //Dictionary<string, object> m_GameplayDefault = new Dictionary<string, object>();

        GameManager m_GameManager;
        GameSettings m_GameSettings;
        GamePlayAudio m_GamePlayAudio;
        public GamePlayAudio.LevelType actualBGM { get; set; }

    #region Delegates
        public delegate void GamePlayManagerEventHandler();
        public event GamePlayManagerEventHandler EventPausePlayGame;
        public event GamePlayManagerEventHandler EventUnPausePlayGame;
        public event GamePlayManagerEventHandler EventCompletePath;
        public event GamePlayManagerEventHandler EventCompleteGame;
        public event GamePlayManagerEventHandler EventGameOver;
        public event GamePlayManagerEventHandler EventShouldContinue;
        public event GamePlayManagerEventHandler EventStartPath;
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
        public delegate void CollectableEventHandle(CollectibleScriptable collectibles);
        public event CollectableEventHandle EventCollectItem;
        public delegate void ShakeCamEventHandle(float power, float inTime);
        public event ShakeCamEventHandle EventShakeCam;
    #endregion
#region UnityMethods
        void OnEnable()
        {
            SetInitialReferences();
            completePath = false;
            var levelRoot = new GameObject();
            m_ActualPath = 0;
            if (actualLevel != null)
            {
                levelRoot.name = actualLevel.levelName;
                m_GameManager.actualLevel.CreateLevel(levelRoot.transform);
                actualBGM = m_GameManager.actualLevel.startLevelBGM;
                m_GamePlayAudio.PlayBGM(actualBGM); 
            }
            //SpawnPlayers(numPlayers.Length);
            EventCheckPlayerPosition += CheckPoolLevel;
            //m_GameplayDefault.Add("config_gamePlay_godmod", false);
        }
        
        void OnDisable()
        {
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
            }*/
        }
        public void GamePlayPause(bool setPause)
        {
            gamePaused = setPause;
        }
        
        #region Multiplayer
        public GameObject GetPlayerById(int id)
        {
            return listPlayer[id];
        }
        private void MultiPlayerSpawns(int players)
        {
            listPlayer = new List<GameObject>();
            for (int i = 0; i < players; i++)
            {
                listPlayer.Add(Instantiate(prefabPlayer));
                listPlayer[i].SetActive(true);
                listPlayer[i].name = "Player" + i;
                listPlayer[i].GetComponent<PlayerMaster>().Init(this.numPlayers[i], i);
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
        #region LevelBuilder
        
  #endregion
        

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
            }*/
        }

        public bool haveAnyPlayMasterBeReady
        {
            get
            {
                return listPlayer.Any(t => t.GetComponent<PlayerMaster>().ShouldPlayerBeReady());
            }
        }

        public bool shouldStartPath
        {
            get
            {
                return (gamePaused && !m_GameManager.isGameStopped && !m_GameManager.isGameOver && !gameBeat && !completePath);
            }
        }

        public bool shouldBePlayingGame
        {
            get
            {
                return m_GameManager.isGameStopped || gamePaused || (!m_GameManager.isGameOver && !completePath);
            }
        }

        public bool shouldBeInGameOver
        {
            get
            {
                return (m_GameManager.isGameOver && !m_GameManager.isGameStopped && !gamePaused);
            }
        }

        public bool shouldBeFinishPath
        {
            get
            {
                return (completePath && !gamePaused && !m_GameManager.isGameOver && !m_GameManager.isGameStopped);
            }
        }

        public bool shouldFinishGame
        {
            get
            {
                return (gameBeat && completePath && !m_GameManager.isGameOver && !m_GameManager.isGameStopped);
            }
        }

    #region Calls
        public void CallEventPausePlayGame()
        {
            gamePaused = true;
            EventPausePlayGame?.Invoke();
        }

        public void CallEventUnPausePlayGame()
        {
            gamePaused = false;
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
            //m_GameManager.isGameOver = true;
            //GamePlayAudio.instance.StopBGM();
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

        public void CallEventResetEnemies()
        {
            EventResetEnemies?.Invoke();
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
    }
}
