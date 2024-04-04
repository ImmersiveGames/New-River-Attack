using System;
using System.Collections;
using CarterGames.Assets.SaveManager;
using ImmersiveGames;
using Save;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using UnityEngine.Localization.Settings;
namespace RiverAttack
{
    [RequireComponent(typeof(AudioSource))]
    public class GameManager : Singleton<GameManager>
    {
        /*
         * Este Script é dedicado apenas a guardar referências e valores inerentes  
         * ao escopo macro do jogo para ser possível acessa-lo pelo projeto.
         * Também é dedicado a criar o fluxo de estado (Finite Machine State)
         */
        public bool debugMode;
        [Header("Game Settings")]
        [SerializeField] internal GameSettings gameSettings;
        public LayerMask layerPlayer;
        public LayerMask layerEnemies;
        public enum GameScenes { MainScene, MissionHub, GamePlay, GamePlayBoss, EndGameCredits, BriefingRoom }
        public GameScenes gameScenes;
        public enum GameModes {Classic,Mission}
        internal GameModes gameModes;
        [Header("Level Settings")]
        public Levels classicLevels;
        public ListLevels missionLevels;
        [Header("Menus")]
        [SerializeField]
        public PanelBase panelBaseGame;
        [Header("Menu Fades")]
        public string nameSceneTransition = "TransitionalScene";
        public Transform panelFade;
        public Image fadeImage;
        //[SerializeField] Animator fadeAnimator;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float fadeOutTime = 1f;
        public T PanelBase<T>() where T : class
        {
            return panelBaseGame as T;
        }
        
        internal bool onLoadScene;
        public GameState currentGameState { get; private set; }
        internal GameState lastGameState;
        private PlayerSaveSaveObject _playerSave;
        private PlayerManager _playerManager;
        
        internal PlayersInputActions inputSystem;

        private static bool shouldPlayerBeReady =>
            PlayerManager.instance.initializedPlayerMasters[0].isPlayerDead == false &&
            PlayerManager.instance.initializedPlayerMasters[0].playerMovementStatus !=  PlayerMaster.MovementStatus.Paused;

        #region UNITYMETHODS

        private void Awake()
        {
            //Application.targetFrameRate = -1;
            if (FindObjectsOfType(typeof(GameManager)).Length <= 1)
                return;
            Destroy(gameObject);
        }

        private void Start()
        {
            inputSystem = new PlayersInputActions();
            inputSystem.Player.Disable();
            inputSystem.UiControls.Enable();
            if (SteamClient.IsValid)
            {
                SteamFriends.OnGameOverlayActivated += ChangeStateToPause;
            }
            SetOptionsOnStartUp();
            //Debug.Log($"GameScene: {gameScenes}");
            switch (gameScenes)
            {
                case GameScenes.MainScene:
                    break;
                case GameScenes.MissionHub:
                    break;
                case GameScenes.GamePlay:
                    break;
                case GameScenes.BriefingRoom:
                    break;
                case GameScenes.GamePlayBoss:
                    gameModes = GameModes.Mission;
                    ChangeState(new GameStateOpenCutScene(), "GamePlayBoss");
                    break;
                case GameScenes.EndGameCredits:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ChangeState(new GameStateMenu());
        }

        #region Actions Application
        protected void OnApplicationFocus(bool hasFocus)
        {
            if(debugMode) return;
            ChangeStateToPause(!hasFocus);
        }

        protected void OnApplicationPause(bool pauseStatus)
        {
            if(debugMode) return;
            ChangeStateToPause(pauseStatus);
        }
        #endregion

        private void Update()
        {
            if(!onLoadScene)
                currentGameState?.UpdateState();
        }
        protected override void OnDestroy()
        {
            //base.OnDestroy();
        }
        #endregion
        
        public Levels GetLevel()
        {
            var level = gameModes switch
            {
                GameModes.Classic => classicLevels,
                GameModes.Mission => GamePlayingLog.instance.activeMission,
                _ => classicLevels
            };
            return level;
        }

        internal void ChangeStateToPause(bool pause)
        {      
            if (currentGameState is not GameStatePlayGame &&
                currentGameState is not GameStatePlayGameBoss) return;
            if (currentGameState == lastGameState) return;
            if (!shouldPlayerBeReady) { return; }

            ChangeState(pause ? new GameStatePause() : (lastGameState is GameStatePlayGame)?new GameStatePlayGame(): new GameStatePlayGameBoss());
        }
        
        #region Machine State
        internal void ChangeState(GameState nextState)
        {
            if (currentGameState == nextState)
                return;
            if(onLoadScene) return;
            onLoadScene = true;
            currentGameState?.ExitState();
            lastGameState = currentGameState;
            currentGameState = nextState;
            StartCoroutine(currentGameState?.OnLoadState());
            currentGameState?.EnterState();
            onLoadScene = false;
        }
        
        internal void ChangeState(GameState nextState, string nextSceneName)
        {
            if(onLoadScene) return;
            onLoadScene = true;
            StartCoroutine(LoadSceneAsync(nextState, nextSceneName));
        }

        private IEnumerator LoadSceneAsync(GameState nextState,string nextSceneName)
        {
            if (currentGameState == nextState)
                yield break;
            var unloadScene = SceneManager.GetActiveScene().name;
            yield return StartCoroutine(FadeCanvas(false));
            // chama o status de saida
            currentGameState?.ExitState();
            // Chamar a cena de transição
            yield return SceneManager.LoadSceneAsync(nameSceneTransition, LoadSceneMode.Additive); // Carrega a cena de transição
            lastGameState = currentGameState;
            currentGameState = nextState;
            yield return currentGameState?.OnLoadState();
            //Descarrega a scena aterior
            SceneManager.UnloadSceneAsync(unloadScene);

            while (SceneManager.GetSceneByName(unloadScene).isLoaded) {
                yield return null; // Aguarda até que a cena anterior seja totalmente descarregada
            }
            //Carregando a nova scene
            var loadScene = SceneManager.LoadSceneAsync(nextSceneName);
            loadScene.allowSceneActivation = false;
            
            // wait for the scene to load
            while (!loadScene.isDone)
            {
                if (loadScene.progress >= 0.9f)
                {
                    
                    //Se precisar load bar
                    break;
                }
                yield return null;
            }
            loadScene.allowSceneActivation = true;
            while (!loadScene.isDone)
            {
                yield return null;
            } 
            currentGameState?.EnterState();
            yield return StartCoroutine(FadeCanvas(true));
            while (SceneManager.GetSceneByName(nameSceneTransition).isLoaded) {
                yield return null; // Aguarda até que a cena anterior seja totalmente descarregada
            }
            onLoadScene = false;
        }

        internal static void DestroyGamePlay()
        { 
            DestroyImmediate(PlayerManager.instance);
        }

        private IEnumerator FadeCanvas(bool faceIn) {
            var corInitial = fadeImage.color;
            var corAlpha = (faceIn) ? 0.0f : 1.0f; // in:out
            var corFinal = new Color(corInitial.r, corInitial.g, corInitial.b, corAlpha);
            var timeSpend = 0.0f;
            var timeDuration = (faceIn) ? fadeInTime : fadeOutTime; // in:out 
            
            while (timeSpend < timeDuration) {
                timeSpend += Time.deltaTime;
                //Debug.Log($"FADE: {timeSpend}, {corInitial}, {corFinal}");
                fadeImage.color = Color.Lerp(corInitial, corFinal, timeSpend / timeDuration);
                yield return null;
            }
        }
        #endregion

        private void SetOptionsOnStartUp()
        {
            if (gameSettings.startLocale == null)
            {
                _playerSave = SaveManager.GetSaveObject<PlayerSaveSaveObject>();
                if (_playerSave && _playerSave.startLocale.Value)
                {
                    gameSettings.startLocale = _playerSave.startLocale.Value;
                }
                gameSettings.startLocale = LocalizationSettings.SelectedLocale;
            }
            
            if (Application.targetFrameRate != OptionsFrameRatePanel.FrameRate(gameSettings.indexFrameRate))
            {
                OptionsFrameRatePanel.UpdateFrameRate(gameSettings.indexFrameRate);
            }

            var actualResolution = Screen.currentResolution;
            var vectorActualResolution = new Vector2Int(actualResolution.width, actualResolution.height);
            //Debug.Log($"Setting : {gameSettings.actualResolution} - Actual {actualResolution}");
            if (gameSettings.actualResolution == Vector2Int.zero)
            {
                gameSettings.actualResolution = vectorActualResolution;
                //Debug.Log($"Setting : {gameSettings.actualResolution} - Actual {vectorActualResolution}");
            }
            
            if (gameSettings.actualResolution != vectorActualResolution)
            {
                //Debug.Log($"Setting é diferente do atual");
                var fps = OptionsFrameRatePanel.actualFrameRate;
                var selectedFramerate = OptionsResolutionPanel.GetRefreshRate(fps, 1);
                Screen.SetResolution(
                    gameSettings.actualResolution.x, gameSettings.actualResolution.y, 
                    OptionsResolutionPanel.FullScreenMode, selectedFramerate );
            }

            var actualQualityLevel = QualitySettings.GetQualityLevel();
            switch (gameSettings.indexQuality)
            {
                case < 0:
                    gameSettings.indexQuality = actualQualityLevel;
                    break;
                case >= 0 when actualQualityLevel != gameSettings.indexQuality:
                    QualitySettings.SetQualityLevel(gameSettings.indexQuality);
                    break;
            }
        }
    }
}
