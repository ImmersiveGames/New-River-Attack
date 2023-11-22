using System.Collections;
using UnityEngine;
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
        [SerializeField] float awaitLoad = 0.5f;
        public LayerMask layerPlayer;
        public enum GameScenes {MainScene, MissionHub, GamePlay, EndGameCredits }
        public GameScenes gameScenes;
        public enum GameModes {Classic,Mission}
        internal GameModes gameModes;
        [Header("Level Settings")]
        [SerializeField]
        public Levels classicLevels;
        public ListLevels missionLevels;
        [Header("Menus")]
        [SerializeField]
        public PanelBase panelBaseGame;
        [Header("Menu Fades")]
        public Transform panelFade;
        [SerializeField] Animator fadeAnimator;
        float m_FadeInTime;
        float m_FadeOutTime;
        static readonly int FadeIn = Animator.StringToHash("FadeIn");
        static readonly int FadeOut = Animator.StringToHash("FadeOut");
        public T PanelBase<T>() where T : class
        {
            return panelBaseGame as T;
        }
        
        internal bool onLoadScene;
        public GameState currentGameState { get; private set; }

        #region UNITYMETHODS
        public void Awake()
        {
            if (FindObjectsOfType(typeof(GameManager)).Length > 1)
            {
                gameObject.SetActive(false);
                Destroy(this);
            }
            m_FadeInTime = Tools.GetAnimationTime(fadeAnimator, "FadeIn");
            m_FadeOutTime = Tools.GetAnimationTime(fadeAnimator, "FadeOut");
        }
        void Start()
        {
            ChangeState(new GameStateMenu());
        }
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
                GameModes.Mission => missionLevels.Index(GamePlayingLog.instance.lastMissionIndex),
                _ => classicLevels
            };
            return level;
        }
        #region Machine State
        internal void ChangeState(GameState nextState)
        {
            if (currentGameState == nextState)
                return;
            if(onLoadScene) return;
            onLoadScene = true;
            currentGameState?.ExitState();
            currentGameState = nextState;
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
            PerformFadeOut();
            var loadSceneAsync = SceneManager.LoadSceneAsync(nextSceneName);
            loadSceneAsync.allowSceneActivation = false;
            currentGameState?.ExitState();
            yield return new WaitForSeconds(m_FadeOutTime + awaitLoad);
            
            currentGameState = nextState;
            while (!loadSceneAsync.isDone)
            {
                // Verifique se a cena está 90% carregada.
                if (loadSceneAsync.progress >= 0.9f)
                {
                    loadSceneAsync.allowSceneActivation = true;
                    currentGameState?.OnLoadState();
                    yield return new WaitForSeconds(m_FadeInTime + awaitLoad);
                    PerformFadeIn();
                    
                    currentGameState?.EnterState();
                    onLoadScene = false;
                }
                yield return null;
            }
        }

        internal static void DestroyGamePlay()
        {
         DestroyImmediate(PlayerManager.instance);
         
        }
        #endregion
        void PerformFadeOut()
        {
            fadeAnimator.SetTrigger(FadeOut);
        }
        void PerformFadeIn()
        {
            fadeAnimator.SetTrigger(FadeIn);
        }
    }
}
