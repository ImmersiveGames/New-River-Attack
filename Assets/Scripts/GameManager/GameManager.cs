using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using Utils;
namespace RiverAttack
{
    public abstract class GameState
    {
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }
    
    public enum LevelTypes {Menu = 0, Hub = 1, Grass = 2, Forest = 3, Swamp = 4, Antique = 5, Desert = 6, Ice = 7, GameOver=8, Complete =9, HUD = 10 }
    public class GameManager : Singleton<GameManager>
    {
        
        GameState m_CurrentState;
        GameState m_NextState;
        
        public PanelManager startMenu;

        public float fadeDurationEnter = 0.5f;
        public float fadeDurationExit = 0.5f;
        
        [Header("Layer Settings")]
        public LayerMask layerPlayer;
        public LayerMask layerEnemies;
        public LayerMask layerCollection;
        public LayerMask layerWall;

        [Header("Player Settings")]
        public GameObject playerPrefab;
        public List<PlayerSettings> playerSettingsList = new List<PlayerSettings>();
        [SerializeField] List<PlayerMaster> initializedPlayerMasters = new List<PlayerMaster>();
        
        [Header("Camera Virtual Settings"), SerializeField]
        CinemachineVirtualCamera virtualCamera;
        
        [Header("CutScenes Settings")]
        public GameObject openCutScenePrefab;
        public GameObject endCutScenePrefab;

        #region UnityMethods
        void Start()
        {
            ChangeState(new GameStateMenu(startMenu));
        }
        void Update()
        {
            m_CurrentState?.UpdateState();

            // Exemplo de mudança de estado (pode ser em resposta a alguma condição)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(new GameStateOpenCutScene(startMenu));
            }
        }
  #endregion

        void ChangeState(GameState newState)
        {
            m_NextState = newState;

            if (m_CurrentState != null)
            {
                StartCoroutine(PerformStateTransition(m_CurrentState, m_NextState));
            }
            else
            {
                m_CurrentState = m_NextState;
                m_CurrentState.EnterState();
            }
        }
        
        IEnumerator PerformStateTransition(GameState actualState, GameState nextState)
        {
            // Implemente o efeito de fade out aqui
            yield return new WaitForSeconds(fadeDurationEnter);

            actualState.ExitState();
            yield return new WaitForSeconds(fadeDurationExit);

            m_CurrentState = nextState;
            m_CurrentState.EnterState();
        }

        public void InstantiatePlayers()
        {
            if (initializedPlayerMasters.Count != 0) 
                return;
            var playerObject = Instantiate(playerPrefab);
            initializedPlayerMasters.Add(playerObject.GetComponent<PlayerMaster>());
            initializedPlayerMasters[^1].SetPlayerSettingsToPlayMaster(playerSettingsList[^1]);
            SetFollowVirtualCam(playerObject.transform);
        }
        
        void SetFollowVirtualCam(Transform follow)
        {
            virtualCamera.Follow = follow;
        }

        #region Buttons Actions
        public void BtnNewGame()
        {
            ChangeState(new GameStateOpenCutScene(startMenu));
        }
  #endregion
        /*[SerializeField] internal bool isGameOver;
        [SerializeField] internal bool isGameStopped;
        [SerializeField] private bool isGameFinish;
        [Header("Layer Names")]
        public LayerMask layerPlayer;
        public LayerMask layerEnemies;
        public LayerMask layerCollection;
        public LayerMask layerWall;
        
        public enum States
        {
            Menu,
            InitialAnimation,
            WaitGamePlay,
            GamePlay,
            GameOver,
            Results,
            Credits
        }
        [Header("Game States")]
        [SerializeField]
        States actualGameState;
        [SerializeField]
        float countdownToStartTimer = 3f;
        [Header("Game Settings")]
        [SerializeField]
        GameSettings gameSettings;
        [SerializeField]
        public GamePlaySettings gamePlayLog;
        [SerializeField]
        protected internal List<Transform> playerObjectAvailableList;

        [Header("Menus")]
        [SerializeField] Transform Menu;
        [SerializeField] Transform Touch;
        [SerializeField] Transform Hud;
        [SerializeField] Transform BaseScenary;
        [SerializeField] public Transform pauseButton;
        [SerializeField] PlayableDirector StartCutScene;
        
        private Dictionary<string, object> m_GameplayDefault = new Dictionary<string, object>();

        GamePlayManager m_GamePlayManager;

        //private GameManagerSaves gameSaves;

    #region UNITYMETHODS
        void Awake()
        {
            SetupGame();
        }
        void OnEnable()
        {
            SetInitialReferences();
            
        }
        void Update()
        {
            switch (actualGameState)
            {
                case States.Menu:
                    Debug.Log("Menu+Settings");
                    SetupMenuInitial();
                    break;
                case States.WaitGamePlay:
                    // Animação de entrada
                    countdownToStartTimer -= Time.deltaTime;
                    if (countdownToStartTimer <= 0)
                    {
                        actualGameState = States.GamePlay;
                        m_GamePlayManager.CallEventStartPlayGame();
                    }
                    break;
                case States.InitialAnimation:
                    Debug.Log("GameOver");
                    break;
                case States.GamePlay:
                    //Debug.Log("Começou o jogo");
                    break;
                case States.GameOver:
                    isGameOver = true;
                    Debug.Log("GameOver");
                    break;
                case States.Results:
                    Debug.Log("Resultados  do jogo");
                    break;
                case States.Credits:
                    Debug.Log("Sobe os Creditos");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
        }

        public void ChangeStatesGamePlay(States newStates = States.GamePlay)
        {
            actualGameState = newStates;
        }
        public States GetActualGameState()
        {
            return actualGameState;
        }

        public bool GetGameOver()
        {
            return isGameOver;
        }

        public void SetupGame()
        {
            //actualGameState = States.Menu;
            isGameFinish = false;
            isGameOver = false;
            isGameStopped = false;
        }

        void SetupMenuInitial()
        {
            if(Menu != null)Menu.gameObject.SetActive(true);
            if(Touch != null)Touch.gameObject.SetActive(false);
            if(Hud != null)Hud.gameObject.SetActive(false);
            if(BaseScenary != null)BaseScenary.gameObject.SetActive(false);
            isGameStopped = true;
            m_GamePlayManager.SetGamePlayPause(true);
            foreach (var player in playerObjectAvailableList)
            {
                player.gameObject.SetActive(true);
            }
        }

        public void ButtonNewGame()
        {
            ChangeStatesGamePlay(States.WaitGamePlay);
            isGameStopped = false;
            BaseScenary.gameObject.SetActive(true);
            m_GamePlayManager.SetGamePlayPause(true);
            StartCutScene.Play();
        }

        void ToggleStopGame()
        {
            isGameStopped = !isGameStopped;
            Time.timeScale = isGameStopped ? 0f : 1f;
        }
        
        public Transform GetActivePlayerTransform(int num = 0)
        {
            return playerObjectAvailableList[num];
        }

        internal void AddActivePlayerObject(Transform activePlayer)
        {
            playerObjectAvailableList.Add(activePlayer);
            
        }*/
    }
}
