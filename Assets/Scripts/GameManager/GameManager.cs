using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Utils;
namespace RiverAttack
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] internal bool isGameOver;
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
        [SerializeField]Transform Menu;
        [SerializeField]Transform Touch;
        [SerializeField]Transform Hud;
        [SerializeField]Transform BaseScenary;
        [SerializeField]PlayableDirector StartCutScene;
        
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
            Menu.gameObject.SetActive(true);
            Touch.gameObject.SetActive(false);
            Hud.gameObject.SetActive(false);
            BaseScenary.gameObject.SetActive(false);
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
            
        }
    }
}
