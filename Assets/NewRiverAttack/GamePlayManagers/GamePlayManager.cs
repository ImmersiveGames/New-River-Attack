using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using ImmersiveGames;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.StateManagers.States;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewRiverAttack.GamePlayManagers
{
    public sealed class GamePlayManager : Singleton<GamePlayManager>
    {
        #region Variáveis
        
        [Header("GameLog"), SerializeField]
        private GamePlayLog gamePlayLog;
        
        internal bool IsBossFight;
        internal bool IsPause;

        [Header("Level Builder")]

        [Header("Player Initialize")]
        [SerializeField] private PlayersDefaultSettings allPlayersDefaultSettings;

        private readonly List<PlayerMaster> _initializedPlayers = new List<PlayerMaster>();
        
        private LevelBuilderManager _levelBuilderManager;
        private LevelData _actualLevel;

        private bool _activePlayers;

        private GameManager _gameManager;
        private GameOptionsSave _gameOptionsSave;

        #endregion

        #region Delegates
        public delegate void PlayerMasterEventHandler(PlayerMaster playerMaster);
        public event PlayerMasterEventHandler EventPlayerGetHit;
        public event PlayerMasterEventHandler EventPlayerInitialize;
        public delegate void GamePlayGeneralEventHandler();
        public event GamePlayGeneralEventHandler EventPostStateGameInitialize;
        public event GamePlayGeneralEventHandler EventGameReady;
        public event GamePlayGeneralEventHandler EventGameRestart;
        public event GamePlayGeneralEventHandler EventGameOver;
        public event GamePlayGeneralEventHandler EventGameFinisher;
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

        #region Unity Methods
        
        private void OnEnable()
        {
            SetInitialReferences();
            //TODO: Vai precisar pegar o valor do index da missão
            SetGameMode();
        }
        private void SetInitialReferences()
        {
            _gameManager = GameManager.instance;
            _gameOptionsSave = GameOptionsSave.instance;
            _levelBuilderManager = LevelBuilderManager.instance;
            gamePlayLog = GamePlayLog.instance;
        }

        private void Start()
        {
            IsPause = false;
            InitializePlayers(allPlayersDefaultSettings);
            StartCoroutine(WaitForInitialization());
        }

        private void OnDisable()
        {
            CleanUpGame();
        }

        private void CleanUpGame()
        {
            DestroyPlayers();
            _gameManager = null;
            _levelBuilderManager.DestroyLevel();
            _levelBuilderManager = null;
        }

        #endregion

        #region Controle de Jogo

        public bool ShouldBePlayingGame =>
            GameManager.StateManager.GetCurrentState() is GameStatePlay && _activePlayers;
        

        #endregion

        #region Inicialização de Jogadores

        private void InitializePlayers(PlayersDefaultSettings playersDefaultSettings)
        {
            DebugManager.Log<GamePlayManager>($"Inicializando Jogadores");

            var rotationQuaternion = Quaternion.Euler(playersDefaultSettings.spawnRotation);

            for (var index = 0; index < _gameOptionsSave.playerSettings.Length; index++)
            {
                var playerSetting = GameOptionsSave.instance.playerSettings[index];
                var playerName = string.IsNullOrEmpty(playerSetting.playerName) ? $"Player {index}" : playerSetting.playerName;

                var newPlayer = Instantiate(playersDefaultSettings.playerPrefab, playersDefaultSettings.spawnPosition, rotationQuaternion);
                newPlayer.name = playerName;

                var playerMaster = newPlayer.GetComponent<PlayerMaster>();
                if (playerMaster == null)
                {
                    throw new MissingComponentException($"Componente PlayerMaster não encontrado no prefab {playersDefaultSettings.playerPrefab.name}");
                }

                DebugManager.Log<GamePlayManager>(
                    $"{playerName} instanciação na posição {playersDefaultSettings.spawnPosition} e rotação {playersDefaultSettings.spawnRotation}");
                _initializedPlayers.Add(playerMaster);
                playerMaster.OnEventPlayerMasterInitialize(index, playersDefaultSettings);
                OnEventPlayerInitialize(playerMaster);
            }
        }

        private void DestroyPlayers()
        {
            // Verifica se a lista de jogadores inicializados não está vazia
            if (_initializedPlayers is not { Count: > 0 }) return;
            // Itera pela lista de jogadores
            foreach (var playerMaster in _initializedPlayers.ToList().Where(playerMaster => playerMaster != null && playerMaster.gameObject != null))
            {
                DestroyImmediate(playerMaster.gameObject);
            }
            // Limpa a lista de jogadores após a destruição
            _initializedPlayers.Clear();
        }


        #endregion

        public void StartReadyGame()
        {
            //Aqui é Apos o Go da Animação
            _activePlayers = true;
            OnEventGameReady();
            //OnEventGameStart();
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

        private void SetGameMode()
        {
            _actualLevel = GetLevel(_gameManager.gamePlayMode);
            AudioManager.PlayBGM(_actualLevel.setLevelList[0].levelType.ToString());
            _levelBuilderManager.StartToBuild(_actualLevel);
        }

        private IEnumerator WaitForInitialization()
        {
            while (!GameManager.StateManager.GetCurrentState().StateFinalization)
            {
                yield return null;
            }
            DebugManager.Log<GamePlayManager>("Inicia o jogo");
            OnEventPostStateGameInitialize();
            //Aqui são as configurações assim que a cena for totalmente carregada.
        }

        public void SetActiveLevel(LevelData dataLevel)
        {
            _actualLevel = dataLevel;
        }

        public LevelData GetLevelData => _actualLevel;
        public int GetNumberOfPlayers => _initializedPlayers.Count;
        public PlayersDefaultSettings PlayersDefault => allPlayersDefaultSettings;
        public PlayerMaster GetPlayerMaster(int playerIndex)
        {
            return _initializedPlayers.ElementAtOrDefault(playerIndex);
        }
        #endregion

        #region Calls

        private void OnEventPlayerInitialize(PlayerMaster playerMaster)
        {
            EventPlayerInitialize?.Invoke(playerMaster);
        }

        private void OnEventPostStateGameInitialize()
        {
            EventPostStateGameInitialize?.Invoke();
        }
        internal void OnEventGameReady()
        {
            EventGameReady?.Invoke();
        }
        internal void OnEventGameRestart()
        {
            EventGameRestart?.Invoke();
        }
        internal void OnEventHudScoreUpdate(int valueUpdate, int playerIndex )
        {
            EventHudScoreUpdate?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventHudDistanceUpdate(int valueUpdate, int playerIndex )
        {
            EventHudDistanceUpdate?.Invoke(valueUpdate, playerIndex);
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
        internal void OnEventPlayerGetHit(PlayerMaster playerMaster)
        {
            EventPlayerGetHit?.Invoke(playerMaster);
        }
        internal void OnEventGamePause()
        {
            IsPause = true;
            EventGamePause?.Invoke();
            Time.timeScale = 0;
        }
        internal void OnEventGameUnPause()
        {
            IsPause = false;
            EventGameUnPause?.Invoke();
            Time.timeScale = 1;
        }
        internal void OnEventGameOver()
        {
            EventGameOver?.Invoke();
        }
        internal void OnEventHudRapidFireEnd(float valueUpdate, int playerIndex)
        {
            EventHudRapidFireEnd?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventGameFinisher()
        {
            EventGameFinisher?.Invoke();
        }
        #endregion


        
    }
}
