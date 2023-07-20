using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    public class GameManager : Utils.Singleton<GameManager>
    {
        [SerializeField] internal bool isGameOver;
        [SerializeField] internal bool isGameStopped;
        [SerializeField] private bool isGameFinish;

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
        [SerializeField]
        States actualGameState;
        [SerializeField]
        float countdownToStartTimer = 3f;

        [SerializeField]
        GameSettings gameSettings;

        [SerializeField]
        public GamePlaySettings gamePlayLog;

        [SerializeField]
        public List<Levels> levelsFinish = new List<Levels>();
        [SerializeField]
        public Levels actualLevel;
        [SerializeField]
        private PlayerSettings[] numPlayer;
        private Dictionary<string, object> m_GameplayDefault = new Dictionary<string, object>();

        //private GameManagerSaves gameSaves;

    #region UNITYMETHODS
        void Awake()
        {
            //states = States.Menu;
            isGameFinish = false;
            isGameOver = false;
            isGameStopped = false;
        }
        private void Start()
        {
            //GameInput.Instance.OnPauseAction += GameInput_onPauseAction;
            /*gameplayDefault.Add("config_resetSaves", false);
            Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(gameplayDefault);
            Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
            bool resetSaves = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("config_resetSaves").BooleanValue;
            if (resetSaves)
                gameSaves.SaveGameClear();
            gameSaves.LoadGamePlay(gamePlayLog);
            levelsFinish = gameSaves.LoadLevelComplete("levelComplete");
            firebase = new GameManagerFirebase();
            StartCoroutine(LoadAsyncScene());*/
        }
        void Update()
        {
            switch (actualGameState)
            {
                case States.Menu:
                    Debug.Log("Menu");
                    break;
                case States.WaitGamePlay:
                    // Animação de entrada
                    countdownToStartTimer -= Time.deltaTime;
                    if (countdownToStartTimer <= 0)
                    {
                        actualGameState = States.GamePlay;
                    }
                    break;
                case States.InitialAnimation:
                    Debug.Log("GameOver");
                    break;
                case States.GamePlay:
                    Debug.Log("Começou o jogo");
                    break;
                case States.GameOver:
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
        public void ChangeStatesGamePlay(States newStates = States.GamePlay)
        {
            actualGameState = newStates;
        }
        public States GetActualGameState()
        {
            return actualGameState;
        }

        public bool HasGameStopped()
        {
            return isGameStopped;
        }

        public bool GetGameOver()
        {
            return isGameOver;
        }

        void GameInput_onStopAction(object sender, EventArgs e)
        {
            ToggleStopGame();
        }

        private void OnEnable()
        {
            //gameSaves = GameManagerSaves.Instance;
        }


        public void SetupGame()
        {
            isGameOver = false;
            isGameStopped = false;
        }

        void ToggleStopGame()
        {
            isGameStopped = !isGameStopped;
            Time.timeScale = isGameStopped ? 0f : 1f;
        }

        public IEnumerator LoadAsyncScene()
        {
            /*FadeScenesManager.Instance.loadAsync.SetActive(true);
            while (firebase.dependencyStatus != Firebase.DependencyStatus.Available)
            {
                yield return null;
            }
            FadeScenesManager.Instance.loadAsync.SetActive(false);*/
            return null;
        }

        public PlayerSettings GetFirstPlayer(int num = 0)
        {
            return numPlayer[num];
        }
        /*private void OnDisable()
        {
            gamePlayLog.totalTime += Time.unscaledTime;
            gameSaves.SaveGamePlay(gamePlayLog);
        }*/
    }
}
