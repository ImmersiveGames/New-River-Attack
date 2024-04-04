using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.LevelBuilder;
using ImmersiveGames.PlayerManagers.PlayerSystems;
using ImmersiveGames.PlayerManagers.ScriptableObjects;
using ImmersiveGames.SaveManagers;
using ImmersiveGames.StateManagers.States;
using ImmersiveGames.Utils;
using UnityEngine;

namespace ImmersiveGames.GamePlayManagers
{
    public sealed class GamePlayManager : Singleton<GamePlayManager>
    {
        #region Variáveis

        internal bool IsBossFight;
        [Header("Camera System & Animations")]
        
        
        [Header("Level Builder")]
        public LevelData levelClassic;
        public List<LevelData> levelsMission;

        [Header("Player Initialize")]
        [SerializeField] private PlayersDefaultSettings allPlayersDefaultSettings;

        private List<PlayerMaster> _initializedPlayers = new List<PlayerMaster>();

        private static GamePlayModes _gamePlayModes;
        private LevelBuilderManager _levelBuilderManager;
        private LevelData _actualLevel;

        private bool _activePlayers;

        private GameManager _gameManager;

        #endregion

        #region Delegates
        public delegate void PlayerMasterEventHandler(PlayerMaster playerMaster);
        public delegate void GamePlayGeneralEventHandler();
        public event PlayerMasterEventHandler EventPlayerInitialize;
        public event GamePlayGeneralEventHandler EventGameInitialize;

        #endregion

        #region Unity Methods
        
        private void OnEnable()
        {
            SetInitialReferences();
            SetGameMode(0);
        }


        private void SetInitialReferences()
        {
            _gameManager = GameManager.instance;
            _levelBuilderManager = LevelBuilderManager.instance;
        }

        private void Start()
        {
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

        private void StartGame()
        {
            DebugManager.Log<GamePlayManager>("Inicia o jogo");
            OnEventGameInitialize();
            _activePlayers = true;
            //Aqui são as configurações assim que a cena for totalmente carregada.
        }

        #endregion

        #region Inicialização de Jogadores

        private void InitializePlayers(PlayersDefaultSettings playersDefaultSettings)
        {
            DebugManager.Log<GamePlayManager>($"Inicializando Jogadores");

            var rotationQuaternion = Quaternion.Euler(playersDefaultSettings.spawnRotation);

            for (var index = 0; index < playersDefaultSettings.playerSettings.Length; index++)
            {
                var playerSetting = playersDefaultSettings.playerSettings[index];
                var playerName = string.IsNullOrEmpty(playerSetting.playerName) ? $"Player {index}" : playerSetting.playerName;
/*
                var newPlayer = Instantiate(playersDefaultSettings.playerPrefab, playersDefaultSettings.spawnPosition, rotationQuaternion);
                newPlayer.name = playerName;

                var playerMaster = newPlayer.GetComponent<PlayerMaster>();
                if (playerMaster == null)
                {
                    throw new MissingComponentException($"Componente PlayerMaster não encontrado no prefab {playersDefaultSettings.playerPrefab.name}");
                }

                DebugManager.Log<GamePlayManager>(
                    $"{playerName} instanciado na posição {playersDefaultSettings.spawnPosition} e rotação {playersDefaultSettings.spawnRotation}");
                _initializedPlayers.Add(playerMaster);
                playerMaster.OnEventPlayerMasterInitialize(index, playersDefaultSettings);
                OnEventPlayerInitialize(playerMaster);
                */
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

        #region Métodos Auxiliares

        private LevelData GetLevel(int levelIndex, GamePlayModes modes)
        {
            if (levelClassic == null && levelsMission.Count <= 0)
            {
                DebugManager.LogError<GamePlayManager>("Não existe um arquivo de data para construir uma cena");
            }

            switch (modes)
            {
                case GamePlayModes.ClassicMode:
                    if (levelClassic == null)
                    {
                        DebugManager.LogError<GamePlayManager>("Não existe um arquivo de data para construir no modo Classic");
                    }
                    return levelClassic;

                case GamePlayModes.MissionMode:
                    if (levelsMission.Count <= 0)
                    {
                        DebugManager.LogError<GamePlayManager>("Não existe um arquivo de data para construir no modo Mission");
                    }
                    var saveIndex = GameOptionsSave.instance.activeIndexMissionLevel;
                    levelIndex = (levelIndex == 0 && saveIndex != 0) ? GameOptionsSave.instance.activeIndexMissionLevel : levelIndex;
                    return levelsMission[levelIndex];

                default:
                    return null;
            }
        }

        private void SetGameMode(int levelIndex)
        {
            _gamePlayModes = _gameManager.gamePlayMode;
            _actualLevel = GetLevel(levelIndex, _gamePlayModes);
            _levelBuilderManager.StartToBuild(_actualLevel);
        }

        private IEnumerator WaitForInitialization()
        {
            while (!GameManager.StateManager.GetCurrentState().StateFinalization)
            {
                yield return null;
            }

            StartGame();
        }

        public void SetActiveLevel(LevelData dataLevel)
        {
            _actualLevel = dataLevel;
        }

        public LevelData GetLevelData()
        {
            return _actualLevel;
        }

        public PlayerMaster GetPlayerMaster(int playerIndex)
        {
            return _initializedPlayers.ElementAtOrDefault(playerIndex);
        }

        private void OnEventPlayerInitialize(PlayerMaster playerMaster)
        {
            EventPlayerInitialize?.Invoke(playerMaster);
        }

        #endregion

        public int GetNumberOfPlayers()
        {
            return _initializedPlayers.Count;
        }

        private void OnEventGameInitialize()
        {
            EventGameInitialize?.Invoke();
        }
    }
}
