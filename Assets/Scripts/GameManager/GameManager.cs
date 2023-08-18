using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using Utils;
using Object = UnityEngine.Object;
namespace RiverAttack
{
    public abstract class GameState
    {
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }

    public enum LevelTypes { Menu = 0, Hub = 1, Grass = 2, Forest = 3, Swamp = 4, Antique = 5, Desert = 6, Ice = 7, GameOver=8, Complete =9, HUD = 10 }
    public class GameManager : Singleton<GameManager>
    {

        public bool debugMode;
        bool m_OnTransition = false;
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
        public Vector3 spawnPlayerPosition;
        public List<PlayerSettings> playerSettingsList = new List<PlayerSettings>();
        [SerializeField] List<PlayerMaster> initializedPlayerMasters = new List<PlayerMaster>();

        [Header("Camera Settings"), SerializeField]
        CinemachineVirtualCamera virtualCamera;

        [Header("CutScenes Settings")]
        [SerializeField] PlayableDirector openCutDirector;

        #region UnityMethods
        void Start()
        {
            if (debugMode)
            {
                ChangeState(new GameStatePlayGame());
                return;
            }
            ChangeState(new GameStateMenu());
        }
        void Update()
        {
            currentGameState?.UpdateState();
            
        }
  #endregion
        public GameState currentGameState
        {
            get;
            private set;
        }
        internal void ChangeState(GameState newState)
        {
            m_NextState = newState;
            if (m_OnTransition) return;
            if (currentGameState != null)
            {
                if (currentGameState is GameStateOpenCutScene)
                {
                    ChangeState(currentGameState, m_NextState);
                }
                else
                {
                    m_OnTransition = true;
                    StartCoroutine(PerformStateTransition(currentGameState, m_NextState));
                }
                
            }
            else
            {
                currentGameState = m_NextState;
                currentGameState.EnterState();
            }
        }
        public void PauseGame()
        {
            ChangeState(new GameStatePause());
        }
        public void UnPauseGame()
        {
            ChangeState(new GameStatePlayGame());
        }

        public void ActivePlayers(bool active)
        {
            if(initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                playerMaster.gameObject.SetActive(active);
            }
        }
        public bool haveAnyPlayerInitialized
        {
            get { return initializedPlayerMasters.Count > 0; }
        }

        public void UnPausedMovementPlayers()
        {
            if(initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                playerMaster.playerMovementStatus = PlayerMaster.MovementStatus.None;
            }
        }
        
        void ChangeState(GameState actualState, GameState nextState)
        {
            actualState.ExitState();

            currentGameState = nextState;
            currentGameState.EnterState();
        }

        IEnumerator PerformStateTransition(GameState actualState, GameState nextState)
        {
            Debug.Log($"Start Coroutine");
            // Implemente o efeito de fade out aqui
            startMenu.PerformFadeOut();
            
            yield return new WaitForSeconds(fadeDurationEnter);

            actualState.ExitState();
            yield return new WaitForSeconds(fadeDurationExit);
            // Implemente o efeito de fade in aqui
            startMenu.PerformFadeIn();

            currentGameState = nextState;
            currentGameState.EnterState();
            m_OnTransition = false;
        }

        public void InstantiatePlayers()
        {
            if (initializedPlayerMasters.Count != 0) 
                return;
            var playerSettings = playerSettingsList[^1];
            var playerObject = Instantiate(playerPrefab,spawnPlayerPosition, quaternion.identity);
            playerObject.name = playerSettings.name;
            var playerMaster = playerObject.GetComponent<PlayerMaster>();
            playerMaster.SetPlayerSettingsToPlayMaster(playerSettings);
            initializedPlayerMasters.Add(playerMaster);
            // Atualiza a cutscene com o animator do jogador;
            ChangeBindingReference("Animation Track", playerMaster.GetPlayerAnimator());
            // Coloca o player como Follow da camra
            SetFollowVirtualCam(playerObject.transform);
        }

        void ChangeBindingReference(string track, Object animator)
        {
            foreach (var playableBinding in openCutDirector.playableAsset.outputs)
            {
                if (playableBinding.streamName != track)
                    continue;
                var bindingReference = openCutDirector.GetGenericBinding(playableBinding.sourceObject);

                if (bindingReference == null)
                {
                    // Substituir a referência nula pelo Animator desejado
                    openCutDirector.SetGenericBinding(playableBinding.sourceObject, animator);
                }
            }
        }

        public void PlayOpenCutScene()
        {
            openCutDirector.Play();
        }

        void SetFollowVirtualCam(Transform follow)
        {
            virtualCamera.Follow = follow;
        }

        #region Buttons Actions
        public void BtnNewGame()
        {
            ChangeState(new GameStateOpenCutScene(openCutDirector));
        }
        #endregion
        /*
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
