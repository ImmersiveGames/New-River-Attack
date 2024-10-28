using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.CameraManagers;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using NewRiverAttack.SaveManagers;
using UnityEngine;

namespace NewRiverAttack.GamePlayManagers
{
    public sealed class PlayersManager : MonoBehaviour
    {
        [Header("Player Initialize")] [SerializeField]
        private PlayersDefaultSettings allPlayersDefaultSettings;

        private readonly List<PlayerMaster> _initializedPlayers = new List<PlayerMaster>();
        private GameOptionsSave _gameOptionsSave;
        private GamePlayManager _gamePlayManager;

        public static PlayersManager Instance { get; private set; }

        #region Events

        public event Action<PlayerMaster> EventPlayerInitialize;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            SetInitialReferences();
            if (Instance == null)
            {
                Instance = this;

                DebugManager.Log<PlayersManager>("PlayersManager instanciado.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<PlayersManager>(
                    "Tentativa de criar uma segunda instância de PlayersManager foi evitada.");
            }
        }

        private void OnEnable()
        {
            _gamePlayManager.EventGameReadyGo += StartPlayerOnReadyGame;
            _gamePlayManager.EventGameFinisher += PlayerSetFinish;
            _gamePlayManager.EventGameReset += ResetPlayer;
        }

        private void Start()
        {
            InitializePlayers(allPlayersDefaultSettings);
        }

        private void OnDisable()
        {
            _gamePlayManager.EventGameReadyGo -= StartPlayerOnReadyGame;
            _gamePlayManager.EventGameFinisher -= PlayerSetFinish;
            _gamePlayManager.EventGameReset -= ResetPlayer;
            DestroyPlayers();
        }

        #endregion

        private void SetInitialReferences()
        {
            _gameOptionsSave = GameOptionsSave.Instance;
            _gamePlayManager = GamePlayManager.Instance;
        }

        #region Controle de Jogo

        public bool HasPlayersActive { get; private set; }

        #endregion

        #region Inicialização de Jogadores

        private void InitializePlayers(PlayersDefaultSettings playersDefaultSettings)
        {
            DebugManager.Log<PlayersManager>($"Inicializando Jogadores");

            var rotationQuaternion = Quaternion.Euler(playersDefaultSettings.spawnRotation);
            DestroyPlayers();
            for (var index = 0; index < _gameOptionsSave.playerSettings.Length; index++)
            {
                var playerSetting = GameOptionsSave.Instance.playerSettings[index];
                var playerName = string.IsNullOrEmpty(playerSetting.playerName)
                    ? $"Player {index}"
                    : playerSetting.playerName;

                var newPlayer = Instantiate(playersDefaultSettings.playerPrefab, playersDefaultSettings.spawnPosition,
                    rotationQuaternion);
                newPlayer.name = playerName;

                var playerMaster = newPlayer.GetComponent<PlayerMaster>();
                if (playerMaster == null)
                {
                    throw new MissingComponentException(
                        $"Componente PlayerMaster não encontrado no prefab {playersDefaultSettings.playerPrefab.name}");
                }

                DebugManager.Log<PlayersManager>(
                    $"{playerName} instanciação na posição {playersDefaultSettings.spawnPosition} e rotação {playersDefaultSettings.spawnRotation}");
                _initializedPlayers.Add(playerMaster);
                playerMaster.OnEventPlayerMasterInitialize(index, playersDefaultSettings);
            }

            CameraMaster.TargetPlayer(_initializedPlayers[0]);
            OnEventPlayerInitialize(_initializedPlayers[0]);
        }

        private void StartPlayerOnReadyGame()
        {
            //Aqui é Apos o Go da Animação
            HasPlayersActive = true;
            _initializedPlayers[0].SavePosition(Vector3.zero);
        }

        private void PlayerSetFinish()
        {
            var player = _initializedPlayers[0].transform.position;
            CameraManager.RepositionEndCamera(new Vector3(player.x, 40, player.z));
            CameraManager.ActiveEndCamera(true);
        }

        private void DestroyPlayers()
        {
            // Verifica se a lista de jogadores inicializados não está vazia
            if (_initializedPlayers is not { Count: > 0 }) return;
            // Itera pela lista de jogadores
            foreach (var playerMaster in _initializedPlayers.ToList()
                         .Where(playerMaster => playerMaster != null && playerMaster.gameObject != null))
            {
                DestroyImmediate(playerMaster.gameObject);
            }

            // Limpa a lista de jogadores após a destruição
            _initializedPlayers.Clear();
        }

        private void ResetPlayer()
        {
            HasPlayersActive = false;
            InitializePlayers(allPlayersDefaultSettings);
        }

        public int GetNumberOfPlayers => _initializedPlayers.Count;
        public PlayersDefaultSettings PlayersDefault => allPlayersDefaultSettings;

        public PlayerMaster GetPlayerMaster(int playerIndex)
        {
            return _initializedPlayers.ElementAtOrDefault(playerIndex);
        }

        #endregion

        private void OnEventPlayerInitialize(PlayerMaster obj)
        {
            EventPlayerInitialize?.Invoke(obj);
        }
    }
}