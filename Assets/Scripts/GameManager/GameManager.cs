using System;
using System.Collections;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeOutTime = 1f;
        public T PanelBase<T>() where T : class
        {
            return panelBaseGame as T;
        }
        
        internal bool onLoadScene;
        public GameState currentGameState { get; private set; }
        internal GameState lastGameState;

        #region UNITYMETHODS
        void Awake()
        {
            if (FindObjectsOfType(typeof(GameManager)).Length <= 1)
                return;
            Destroy(gameObject);
        }
        void Start()
        {
            if (SteamClient.IsValid && SteamClient.IsLoggedOn)
            {
                SteamFriends.OnGameOverlayActivated += PauseGame;
            }
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
            PauseGame(!hasFocus);
        }

        protected void OnApplicationPause(bool pauseStatus)
        {
            PauseGame(pauseStatus);
        }
        #endregion

        void Update()
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

        void PauseGame(bool pause)
        {
            Time.timeScale = pause ? 0 : 1;
            if (currentGameState is not GameStatePlayGame)
                return;
            if (pause)
            {
                ChangeState(new GameStatePause());
            }
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
        IEnumerator LoadSceneAsync(GameState nextState,string nextSceneName)
        {
            if (currentGameState == nextState)
                yield break;
            string unloadScene = SceneManager.GetActiveScene().name;
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
            float corAlpha = (faceIn) ? 0.0f : 1.0f; // in:out
            var corFinal = new Color(corInitial.r, corInitial.g, corInitial.b, corAlpha);
            float timeSpend = 0.0f;
            float timeDuration = (faceIn) ? fadeInTime : fadeOutTime; // in:out 
            
            while (timeSpend < timeDuration) {
                timeSpend += Time.deltaTime;
                //Debug.Log($"FADE: {timeSpend}, {corInitial}, {corFinal}");
                fadeImage.color = Color.Lerp(corInitial, corFinal, timeSpend / timeDuration);
                yield return null;
            }
        }
        #endregion
        
    }
}
