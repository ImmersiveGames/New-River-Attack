using System.Collections;
using System.Collections.Generic;
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


        public bool godmode { get { return godMode; } }

        public bool isGamePause { get { return gamePaused; } }
        public bool isGameBeat { get { return gameBeat; } }

        [Header("Level Settings"), SerializeField]
        private Levels classicLevel;
        [SerializeField]
        private PlayerStats[] numPlayers;
        [SerializeField]
        private GameObject prefabPlayer;
        public List<GameObject> listPlayer { get; private set; }
        private int m_ActualPath;
        private Dictionary<string, object> m_GameplayDefault = new Dictionary<string, object>();

        private GameManager m_GameManager;
        private GameSettings m_GameSettings;
        private GamePlayAudio m_GamePlayAudio;
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
        /*
         private void OnEnable()
    {
        SetInitialReferences();
        completePath = false;
        GameObject levelroot = new GameObject();
        m_ActualPath = 0;
        levelroot.name = actualLevel.levelName;
        m_GameManager.actualLevel.CreateLevel(levelroot.transform);
        actualBGM = m_GameManager.actualLevel.startLevelBGM;
        m_GamePlayAudio.PlayBGM(actualBGM);
        SpawnPlayers(numPlayers.Length);
        EventCheckPlayerPosition += CheckPoolLevel;
        m_GameplayDefault.Add("config_gamePlay_godmod", false);
        FirebaseRemoteConfig.SetDefaults(m_GameplayDefault);
    }

    private void Start()
    {
        FirebaseRemoteConfig.ActivateFetched();

        godMode = (godMode) ? godMode : FirebaseRemoteConfig.GetValue("config_gamePlay_godmod").BooleanValue;
    }
         * */
  #endregion


        private void SpawnPlayers(int numPlayers)
        {
            listPlayer = new List<GameObject>();
            for (int i = 0; i < numPlayers; i++)
            {
                listPlayer.Add(Instantiate(prefabPlayer));
                listPlayer[i].SetActive(true);
                listPlayer[i].name = "Player" + i;
                listPlayer[i].GetComponent<PlayerMaster>().Init(this.numPlayers[i], i);
            }
        }

        private void SetInitialReferences()
        {
            m_GameManager = GameManager.instance;
            m_GameSettings = GameSettings.instance;
            m_GamePlayAudio = GamePlayAudio.instance;
            /*if (m_GameSettings.gameMode.modeId == m_GameSettings.GetGameModes(0).modeId)
            {
                m_GameManager.actualLevel = classicLevel;
            }*/
        }

        public GameObject GetPlayer(int id)
        {
            return listPlayer[id];
        }

        public PlayerMaster GetPlayerMaster(int id)
        {
            return listPlayer[id].GetComponent<PlayerMaster>();
        }

        public PlayerStats GetPlayerSettings(int id)
        {
            return listPlayer[id].GetComponent<PlayerMaster>().GetPlayersSettings();
        }

        public void ReadyPlayer(bool ready)
        {
            for (int i = 0; i < listPlayer.Count; i++)
            {
                listPlayer[i].GetComponent<PlayerMaster>().AllowedMove(ready);
            }
        }
        public Levels GetActualLevel()
        {
            return actualLevel;
        }

        public int GetActualPath()
        {
            return m_ActualPath;
        }

        private void CheckPoolLevel(Vector3 pos)
        {
            /*if (!completePath && (actualLevel.levelMilstones[m_ActualPath] - pos).z <= 0)
            {
                actualLevel.CallUpdatePoolLevel(m_ActualPath);
                m_ActualPath++;
            }*/
        }

        public void GamePlayPause(bool setPause)
        {
            gamePaused = setPause;
        }

        public int HightScorePlayers()
        {
            /*if (listPlayer.Count > 0)
            {
                int score = 0;
                foreach (GameObject pl in listPlayer)
                {
                    if (score < pl.GetComponent<PlayerMaster>().playerSettings.score.Value)
                        score = (int)pl.GetComponent<PlayerMaster>().playerSettings.score.Value;
                }
                return score;
            }
            else*/
                return 1;
        }

        public void SaveAllPlayers()
        {
            /*for (int i = 0; i < numPlayers.Length; i++)
            {
                GameManagerSaves.Instance.SavePlayer(numPlayers[i]);
            }*/
        }

        public bool shouldPlayReady
        {
            get
            {
                foreach (var t in listPlayer)
                {
                    if (t.GetComponent<PlayerMaster>().shouldPlayerBeReady)
                        return true;
                }
                return false;
            }
        }

        public bool shouldStartPath
        {
            get
            {
                return true; //(gamePaused && !m_GameManager.isGamePaused && !m_GameManager.isGameOver && !gameBeat && !completePath) ? true : false;
            }
        }

        public bool shouldBePlayingGame
        {
            get
            {
                return true;//!m_GameManager.isGamePaused && !gamePaused && (m_GameManager.isGameOver || completePath) ? false : true;
            }
        }

        public bool shouldBeInGameOver
        {
            get
            {
                return true; //(m_GameManager.isGameOver && !m_GameManager.isGamePaused && !gamePaused) ? true : false;
            }
        }

        public bool shouldBeFinishPath
        {
            get
            {
                return true; //(completePath && !gamePaused && !m_GameManager.isGameOver && !m_GameManager.isGamePaused) ? true : false;
            }
        }

        public bool shouldFinishGame
        {
            get
            {
                return true; //(gameBeat && completePath && !m_GameManager.isGameOver && !m_GameManager.isGamePaused) ? true : false;
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
        public void CallEventShakeCam(float power, float intime)
        {
            EventShakeCam?.Invoke(power, intime);
        }
    #endregion

        private void OnDisable()
        {
            EventCheckPlayerPosition -= CheckPoolLevel;
            //m_GamePlayAudio.StopBGM();
            StopAllCoroutines();
        }
    }
}