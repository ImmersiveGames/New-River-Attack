using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using NewRiverAttack.BulletsManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
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
        [SerializeField, Range(0.1f,1f)] private float shakeTime;
        [SerializeField, Range(100,1000)] private long millisecondsVibrate;

        private int _bomb;

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
            _gamePlayManager = GamePlayManager.instance;
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
            _bomb = defaultSettings.startBombs;
            _gamePlayManager.OnEventHudBombUpdate(_bomb, _playerMaster.PlayerIndex);
        }

        private void AttemptBomb(InputAction.CallbackContext obj)
        {
            if (!_playerMaster.ObjectIsReady || _bomb <= 0) return;
            DebugManager.Log<PlayerBombs>($"Disparar bomba");
            UseBomb();
        }

        private void UseBomb()
        {
            _bomb -= 1;
            if (_bomb < 0) _bomb = 0;
            var bomb = Instantiate(prefabBomb);
            var bombPlayer = bomb.GetComponent<BulletBombPlayer>();
            bombPlayer.OnSpawned(transform, _bombData);
            _gamePlayManager.OnEventHudBombUpdate(_bomb, _playerMaster.PlayerIndex);
            LogGamePlay(1);
        }

        public int GetBomb => _bomb;
        public int GetMaxBomb => _bomb;
        private void LogGamePlay(int bomb)
        {
            GamePlayLog.instance.playersBombs += bomb;
        }

        #region PowerUP New Bomb

        private void PowerUpAddBomb(ActivePowerUp activePowerUp)
        {
            if (activePowerUp.PowerUpData.powerUpType != PowerUpTypes.Bomb) return;
            _bomb += 1;
            _gamePlayManager.OnEventHudBombUpdate(_bomb, _playerMaster.PlayerIndex);
        }

        #endregion
    }
}