using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Immersive
{
    public class GameManager : Utils.Singleton<GameManager>
    {
        [SerializeField] private bool isGameOver;
        [SerializeField] private bool isGamePaused;
        [SerializeField] private bool isGameBaet;

        public enum States
        {
            Menu,
            WaitGamePlay,
            GamePlay,
            GameOver,
            Results,
            Credits
        }
        [SerializeField]
        States states;
        [SerializeField]
        float countdownToStartTimer = 3f;

        /*[SerializeField]
        private GameSettings gameSettings;
        [SerializeField]
        public GamePlaySettings gamePlayLog;
        
        [SerializeField]
        public List<Levels> levelsFinish = new List<Levels>();
        [SerializeField]
        public Levels actualLevel;
        [SerializeField]
        private Player[] numPlayer;
        private Dictionary<string, object> gameplayDefault = new Dictionary<string, object>();
    
        private GameManagerSaves gameSaves;
        */

        public void ChangeStates(States newStates)
        {
            states = newStates;
        }
        public States GetStates()
        {
            return states;
        }

        public bool GetPaused()
        {
            return isGamePaused;
        }
        void Awake()
        {
            states = States.Menu;
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
            switch (states)
            {
                case States.Menu:
                    Debug.Log("Menu");
                    break;
                case States.WaitGamePlay:
                    countdownToStartTimer -= Time.deltaTime;
                    if (countdownToStartTimer <= 0)
                    {
                        states = States.GamePlay;
                    }
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
        
        void GameInput_onPauseAction(object sender, EventArgs e)
        {
            TogglePauseGame();
        }

        private void OnEnable()
        {
            //gameSaves = GameManagerSaves.Instance;
        }

        
        public void SetupGame()
        {
            isGameOver = false;
            isGamePaused = false;
        }

        void TogglePauseGame()
        {
            isGamePaused = !isGamePaused;
            if (isGamePaused)
            {
                Time.timeScale = 0f;
                //OnGamePaused.Invoke(this,EventArgs.Empty);
            }
            else
            {
                Time.timeScale = 1f;
                //OnGameUnPaused.Invoke(this,EventArgs.Empty);
            }

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

        /*public Player GetFirstPlayer(int num = 0)
        {
            return numPlayer[num];
        }*/
        /*private void OnDisable()
        {
            gamePlayLog.totalTime += Time.unscaledTime;
            gameSaves.SaveGamePlay(gamePlayLog);
        }*/
    }
}
