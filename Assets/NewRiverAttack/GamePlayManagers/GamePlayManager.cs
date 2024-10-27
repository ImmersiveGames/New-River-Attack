using System;
using System.Collections;
using ImmersiveGames;
using ImmersiveGames.CameraManagers;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.HUBManagers;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.StateManagers;
using NewRiverAttack.StateManagers.States;
using UnityEngine;

namespace NewRiverAttack.GamePlayManagers
{
    public sealed class GamePlayManager : MonoBehaviour
    {
        #region Variáveis

        [Header("Default Layers")]
        public LayerMask layerEnemies;
        
        internal bool IsBossFight;
        private bool _isPause;
        
        private LevelBuilderManager _levelBuilderManager;
        private LevelData _actualLevel;

        private GameManager _gameManager;

        #endregion

        #region Delegates
        public event Action EventGameReadyGo;
        public event Action EventGameFinisher;
        public event Action EventGameReset; //Hard Reset
        
        public event Action EventObstacleReload; //SoftReload (Respawn)
        
        public delegate void PlayerMasterEventHandler(PlayerMaster playerMaster);
        public delegate void GamePlayGeneralEventHandler();
        public event GamePlayGeneralEventHandler EventPostStateGameInitialize;
        //public event GamePlayGeneralEventHandler EventGameRestart;
        public event GamePlayGeneralEventHandler EventGameOver;
        
        public event GamePlayGeneralEventHandler EventGamePause;
        public event GamePlayGeneralEventHandler EventGameUnPause;
        
        public delegate void GamePlayHudFloatEventHandler(float valueUpdate, int playerIndex);
        public event GamePlayHudFloatEventHandler EventHudRapidFireUpdate;
        public event GamePlayHudFloatEventHandler EventHudRapidFireEnd;
        public delegate void GamePlayHudEventHandler(int valueUpdate, int playerIndex);
        public event GamePlayHudEventHandler EventHudScoreUpdate;
        public event GamePlayHudEventHandler EventHudDistanceUpdate;
        public event GamePlayHudEventHandler EventHudLivesUpdate;
        public event GamePlayHudEventHandler EventHudBombUpdate;
        public event GamePlayHudEventHandler EventHudRefugiesUpdate;

        #endregion
        
        public static GamePlayManager Instance { get; private set; }

        #region Unity Methods
        private void Awake()
        {
            SetInitialReferences();
            if (Instance == null)
            {
                Instance = this;
                
                DebugManager.Log<GamePlayManager>("GamePlayManager instanciado.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<GamePlayManager>("Tentativa de criar uma segunda instância de GamePlayManager foi evitada.");
            }
        }
        private void OnEnable()
        {
            BuildLevel();
        }
        private void SetInitialReferences()
        {
            _gameManager = GameManager.instance;
            _levelBuilderManager = LevelBuilderManager.Instance;
        }

        private void Start()
        {
            _isPause = false;
            StartCoroutine(WaitForInitialization());
        }

        private void OnDisable()
        {
            GameSaveHandler.Instance.SaveGameData();
            CleanUpGame();
        }

        private void OnApplicationQuit()
        {
            GameSaveHandler.Instance.SaveGameData();
        }

        private void CleanUpGame()
        {
            _gameManager = null;
            _levelBuilderManager.DestroyLevel();
            _levelBuilderManager = null;
        }

        #endregion

        #region Controle de Jogo

        public bool ShouldBePlayingGame =>
            GameManager.StateManager.GetCurrentState is GameStatePlay && PlayersManager.Instance.HasPlayersActive && !_isPause;
        

        #endregion

        //Chamado Na Animação pós o texto de "GO"
        public void StartReadyGame()
        {
            OnEventGameReadyGo();
        }
        public void FinisherGame()
        {
            AudioManager.instance.PlayBGMOneShot("Finish");
            OnEventGameFinisher();
        }

       internal void SendTo(GamePlayModes gamePlayModes)
        {
            switch (gamePlayModes)
            {
                case GamePlayModes.MissionMode:
                    GameManager.instance.ActiveLevel.hudPath.levelsStates = LevelsStates.Complete;
                    Invoke(nameof(SendToHub), 2f);
                    break;
                case GamePlayModes.ClassicMode:
                    Invoke(nameof(SendToCompleteGame), 2f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private async void SendToHub()
        {
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateHub.ToString()).ConfigureAwait(false);
        }
        
        private async void SendToCompleteGame()
        {
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateEndGame.ToString()).ConfigureAwait(false);
        }

        #region Métodos Auxiliares

        private LevelData GetLevel(GamePlayModes modes)
        {
            switch (modes)
            {
                case GamePlayModes.ClassicMode:
                    return _gameManager.classicModeLevels;
                case GamePlayModes.MissionMode:
                    return _gameManager.ActiveLevel;
                default:
                    DebugManager.LogError<GamePlayManager>("Não existe um arquivo de data para construir uma cena");
                    return null;
            }
        }

        private void BuildLevel()
        {
            _actualLevel = GetLevel(_gameManager.gamePlayMode);
            IsBossFight = _actualLevel.levelType == LevelTypes.Boss;
            AudioManager.instance.PlayBGM(_actualLevel.setLevelList[0].levelType.ToString());
            _levelBuilderManager.StartToBuild(_actualLevel);
        }

        private IEnumerator WaitForInitialization()
        {
            while (!GameManager.StateManager.GetCurrentState.StateFinalization)
            {
                yield return null;
            }
            DebugManager.Log<GamePlayManager>("Inicia o jogo");
            OnEventPostStateGameInitialize();
            //Aqui são as configurações assim que a cena for totalmente carregada.
        }
        
        #endregion

        #region Calls

        private void OnEventPostStateGameInitialize()
        {
            EventPostStateGameInitialize?.Invoke();
        }
        internal void OnEventGameReadyGo()
        {
            EventGameReadyGo?.Invoke();
        }
        internal void OnEventHudScoreUpdate(int valueUpdate, int playerIndex )
        {
            EventHudScoreUpdate?.Invoke(valueUpdate, playerIndex);
            GameStatisticManager.instance.LogMaxScore(valueUpdate);
        }
        internal void OnEventHudDistanceUpdate(int valueUpdate, int playerIndex )
        {
            // Converte a distância total acumulada em um valor inteiro com base na conversão
            EventHudDistanceUpdate?.Invoke(valueUpdate, playerIndex);
            GameStatisticManager.instance.LogDistance(valueUpdate);
        }
        internal void OnEventHudLivesUpdate(int valueUpdate, int playerIndex)
        {
            EventHudLivesUpdate?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventHudRefugiesUpdate(int valueUpdate, int playerIndex)
        {
            EventHudRefugiesUpdate?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventHudBombUpdate(int valueUpdate, int playerIndex)
        {
            EventHudBombUpdate?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventHudRapidFireUpdate(float valueUpdate, int playerIndex)
        {
            EventHudRapidFireUpdate?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventGamePause()
        {
            _isPause = true;
            EventGamePause?.Invoke();
            Time.timeScale = 0;
        }
        internal void OnEventGameUnPause()
        {
            _isPause = false;
            EventGameUnPause?.Invoke();
            Time.timeScale = 1;
        }
        internal void OnEventGameOver()
        {
            GameSaveHandler.Instance.SaveGameData();
            EventGameOver?.Invoke();
        }
        internal void OnEventHudRapidFireEnd(float valueUpdate, int playerIndex)
        {
            EventHudRapidFireEnd?.Invoke(valueUpdate, playerIndex);
        }

        private void OnEventGameFinisher()
        {
            GameSaveHandler.Instance.SaveGameData();
            EventGameFinisher?.Invoke();
        }
        
        internal void OnEventGameReset()
        {
            _isPause = false;
            _levelBuilderManager.CleanUpLevel();
            CameraManager.ActiveEndCamera(false);
            BuildLevel();
            CameraManager.ActiveStartCamera();
            GameManager.StateManager.ForceChangeState(StatesNames.GameStatePlay.ToString());
            GameSaveHandler.Instance.SaveGameData();
            EventGameReset?.Invoke();
        }
        internal void OnEventObstacleReload()
        {
            EventObstacleReload?.Invoke();
        }
        #endregion
        
    }
}
