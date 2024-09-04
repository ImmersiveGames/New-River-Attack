using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using NewRiverAttack.BulletsManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerBombs : MonoBehaviour
    {
        [SerializeField] private GameObject prefabBomb;
        [SerializeField, Range(1,10)] private int bombDamage;
        [SerializeField, Range(1f,10f)] private float bombOffsetZ;
        [SerializeField, Range(1f,15f)] private float radiusSize;
        [SerializeField, Range(0.1f, 2f)] private float radiusSpeed;
        [SerializeField, Range(1f,10f)] private float shakeForce;
        [SerializeField, Range(0.01f,0.1f)] private float shakeTime;
        [SerializeField, Range(100,1000)] private long millisecondsVibrate;

        private BombData _bombData;
        private PlayerMaster _playerMaster;
        private GamePlayManager _gamePlayManager;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _playerMaster.EventPlayerMasterInitialize += InitializeBombs;
            _playerMaster.EventPlayerMasterStartPowerUp += PowerUpAddBomb;
        }

        private void Start()
        {
            InputGameManager.RegisterAction("Bomb", AttemptBomb);
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterInitialize -= InitializeBombs;
            _playerMaster.EventPlayerMasterStartPowerUp += PowerUpAddBomb;
        }

        #endregion

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _playerMaster = GetComponent<PlayerMaster>();
            _bombData = new BombData
            {
                BulletDamage = bombDamage,
                BulletOffSet = bombOffsetZ,
                BombRadius = radiusSize,
                BombRadiusSpeed = radiusSpeed,
                BombShakeForce = shakeForce,
                BombShakeTime = shakeTime,
                BombMillisecondsVibrate = millisecondsVibrate,
                BulletOwner = _playerMaster,
                BulletTimer = 0.1f,
            };
        }

        private void InitializeBombs(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            GetBomb = defaultSettings.startBombs;
            GetMaxBomb = defaultSettings.maxBombs;
            _gamePlayManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
        }

        private void AttemptBomb(InputAction.CallbackContext obj)
        {
            if (!_playerMaster.ObjectIsReady || GetBomb <= 0) return;
            DebugManager.Log<PlayerBombs>($"Disparar bomba");
            UseBomb();
        }

        private void UseBomb()
        {
            GetBomb -= 1;
            if (GetBomb < 0) GetBomb = 0;
            var bomb = Instantiate(prefabBomb);
            var bombPlayer = bomb.GetComponent<BulletBombPlayer>();
            bombPlayer.OnSpawned(transform, _bombData);
            _gamePlayManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
            GameStatisticManager.instance.LogBombs(1);
        }

        public int GetBomb { get; private set; }

        public int GetMaxBomb { get; private set; }

        #region PowerUP New Bomb

        private void PowerUpAddBomb(ActivePowerUp activePowerUp)
        {
            if (activePowerUp.PowerUpData.powerUpType != PowerUpTypes.Bomb) return;
            GetBomb += 1;
            _gamePlayManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
        }

        #endregion
    }
}