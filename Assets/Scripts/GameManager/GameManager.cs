using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Utils;
namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class GameManager : Singleton<GameManager>
    {
        /*
         * Este Script é dedicado apenas a guardar referencias e valores inerentes  
         * ao escopo macro do jogo para ser possivel acessa-lo pelo projeto.
         * Também é dedicado a criar o fluxo de estado (Finite Machine State)
         */
        public bool debugMode;
        [Header("Game Settings")]
        [SerializeField] internal GameSettings gameSettings;
        public LayerMask layerPlayer;
        
        [Header("Menus")]
        [SerializeField] PanelBase panelBaseGame;
        [Header("Menu Fades")]
        public Transform panelFade;
        [SerializeField] Animator fadeAnimator;
        internal float fadeInTime;
        internal float fadeOutTime;
        static readonly int FadeIn = Animator.StringToHash("FadeIn");
        static readonly int FadeOut = Animator.StringToHash("FadeOut");
        public T PanelBase<T>() where T : class
        {
            return panelBaseGame as T;
        }
        
        internal bool loadSceneFinish;
        public GameState currentGameState { get; private set; }

        #region UNITYMETHODS
        void Awake()
        {
            fadeInTime = Tools.GetAnimationTime(fadeAnimator, "FadeIn");
            fadeOutTime = Tools.GetAnimationTime(fadeAnimator, "FadeOut");
        }
        void Start()
        {
            ChangeState(new GameStateMenu());
        }
        void Update()
        {
            if(!loadSceneFinish)
                currentGameState?.UpdateState();
        }
        #endregion

        #region Machine State
        internal void ChangeState(GameState nextState)
        {
            if (currentGameState == nextState)
                return;
            loadSceneFinish = true;
            currentGameState?.ExitState();
            currentGameState = nextState;
            currentGameState?.EnterState();
        }
        
        internal void ChangeState(GameState nextState, string nextSceneName)
        {
            loadSceneFinish = true;
            StartCoroutine(LoadSceneAsync(nextState, nextSceneName));
        }
        IEnumerator LoadSceneAsync(GameState nextState,string nextSceneName)
        {
            if (currentGameState == nextState)
                yield break;
            PerformFadeOut();
            var loadSceneAsync = SceneManager.LoadSceneAsync(nextSceneName);
            loadSceneAsync.allowSceneActivation = false;
            yield return new WaitForSeconds(fadeOutTime);
            currentGameState?.ExitState();
            currentGameState = nextState;
            while (!loadSceneAsync.isDone)
            {
                // Verifique se a cena está 90% carregada.
                if (loadSceneAsync.progress >= 0.9f)
                {
                    loadSceneAsync.allowSceneActivation = true;
                    
                    yield return new WaitForSeconds(0.5f);
                    currentGameState?.OnLoadState();
                    yield return new WaitForSeconds(fadeInTime +.5f);
                    PerformFadeIn();
                    
                    currentGameState?.EnterState();
                    loadSceneFinish = false;
                }
                yield return null;
            }
        }
        #endregion
        protected internal void PerformFadeOut()
        {
            fadeAnimator.SetTrigger(FadeOut);
        }
        protected internal void PerformFadeIn()
        {
            fadeAnimator.SetTrigger(FadeIn);
        }
        
        
        ///////////////////////////////////////////////////

        /*

        bool m_OnTransition;
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
                [SerializeField] internal List<PlayerMaster> initializedPlayerMasters = new List<PlayerMaster>();
        
        
        
                GamePlayManager m_GamePlayManager;
                //PlayersInputActions m_InputSystem;
        
                #region UnityMethods
                void Awake()
                {
                    m_InputSystem = new PlayersInputActions();
                    m_InputSystem.Enable();
                }
                void OnEnable()
                {
                    m_GamePlayManager = GamePlayManager.instance;
                }
                
                
          #endregion
        
        void ChangeState(GameState actualState, GameState nextState)
        {
            actualState.ExitState();
        
            currentGameState = nextState;
            currentGameState.EnterState();
            if (currentGameState is GameStateGameOver)
            {
                Invoke(nameof(GameOverState), .2f);
            }
        }
                
                internal void ChangeState(GameState newState)
                {
                    //TODO: melhorar para fazer o state saltar as transições na entrada e na saida. talvez colocar na criação uma bolian para decidir
                    if (m_OnTransition) return;
                    if (currentGameState != null)
                    {
                        if (currentGameState is GameStateOpenCutScene)
                        {
                            ChangeState(currentGameState, newState);
                        }
                        else
                        {
                            if (newState is GameStateGameOver or GameStateEndCutScene)
                            {
                                ChangeState(currentGameState, newState);
                            }
                            else
                            {
                                m_OnTransition = true;
                                StartCoroutine(PerformStateTransition(currentGameState, newState));
                            }
                        }
        
                    }
                    else
                    {
                        currentGameState = newState;
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
                    if (initializedPlayerMasters.Count <= 0) return;
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
                    if (initializedPlayerMasters.Count <= 0) return;
                    foreach (var playerMaster in initializedPlayerMasters)
                    {
                        playerMaster.playerMovementStatus = PlayerMaster.MovementStatus.None;
                    }
                }
        
                
        
                IEnumerator PerformStateTransition(GameState actualState, GameState nextState)
                {
                    //Debug.Log($"Start Coroutine");
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
        
                public void RemoveAllPlayers()
                {
                    if (initializedPlayerMasters.Count <= 0) return;
                    foreach (var playerMaster in initializedPlayerMasters)
                    {
                        DestroyImmediate(playerMaster.gameObject);
                    }
                    initializedPlayerMasters = new List<PlayerMaster>();
                }
        
                void GameOverState()
                {
                    var curr = currentGameState as GameStateGameOver;
                    curr?.GameOverState();
                }
        
                public void InstantiatePlayers()
                {
                    if (initializedPlayerMasters.Count != 0)
                        return;
                    var playerSettings = playerSettingsList[^1];
                    var playerObject = Instantiate(playerPrefab, spawnPlayerPosition, Quaternion.identity);
                    playerObject.name = playerSettings.name;
                    var playerMaster = playerObject.GetComponent<PlayerMaster>();
                    playerMaster.SetPlayerSettingsToPlayMaster(playerSettings);
                    initializedPlayerMasters.Add(playerMaster);
                    // Atualiza a cutscene com o animator do jogador;
                    Tools.ChangeBindingReference("Animation Track", playerMaster.GetPlayerAnimator(), openCutDirector);
                    Tools.ChangeBindingReference("Animation Track", playerMaster.GetPlayerAnimator(), endCutDirector);
                    // Coloca o player como Follow da camra
                    Tools.SetFollowVirtualCam(virtualCamera, playerObject.transform);
                }
        
                public void PlayOpenCutScene()
                {
                    Invoke(nameof(InvokePlayOpenCutScene),0.2f);
                }
                void InvokePlayOpenCutScene()
                {
                    openCutDirector.Play();
                }
               
        
                #region Buttons Actions
                internal void BtnNewGame()
                {
                    ChangeState(new GameStateOpenCutScene(openCutDirector));
                }
        
                internal void BtnGameRestart()
                {
                    m_GamePlayManager.OnEventReSpawnEnemiesMaster();
                    m_GamePlayManager.OnEventEnemiesMasterForceRespawn();
                    m_GamePlayManager.OnEventUpdateRefugees(playerSettingsList[0].wealth);
                    ChangeState(new GameStateMenu());
                    GameMissionBuilder.instance.ResetBuildMission();
                }
                #endregion
        */
////////////////////////////////////////////////////////////////
    }
}
