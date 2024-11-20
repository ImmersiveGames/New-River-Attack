using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerBombs : ObjectShoot
    {
        [SerializeField, Range(1,10)] private int bombDamage;
        [SerializeField, Range(1f,5f)] private float bombLifeTimer = 2.0f;
        
        [SerializeField, Range(1f,15f)] private float radiusSize;
        [SerializeField, Range(0.1f, 2f)] private float radiusSpeed;
        
        [SerializeField, Range(1f,10f)] private float shakeForce;
        [SerializeField, Range(0.01f,0.1f)] private float shakeTime;
        
        public float coolDownBomb;
        private PlayerMaster _playerMaster;
        private GameHudManager _gameHudManager;
        public int GetBomb { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void OnEnable()
        {
            _gameHudManager = GameHudManager.Instance;
            InputGameManager.RegisterAction("Bomb", AttemptShoot);
            _playerMaster.EventPlayerMasterInitialize += InitializeBombs;
            _playerMaster.EventPlayerMasterStartPowerUp += PowerUpAddBomb;
        }

        private void OnDisable()
        {
            InputGameManager.UnregisterAction("Bomb", AttemptShoot);
            _playerMaster.EventPlayerMasterInitialize -= InitializeBombs;
            _playerMaster.EventPlayerMasterStartPowerUp -= PowerUpAddBomb;
        }
        
        private void Start()
        {
            // Configura o cooldown inicial com base no valor de ActualSkin
            if (shootPattern == null || _playerMaster.ActualSkin == null) return;
            cooldown = coolDownBomb;
            DebugManager.Log<PlayerShoot>($"PlayerShoot.Start: Cooldown inicial definido a partir de ActualSkin: {coolDownBomb}");
        }
        private void InitializeBombs(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            GetBomb = defaultSettings.startBombs;
            _gameHudManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
        }
        private void AttemptShoot(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (!_playerMaster.ObjectIsReady) return;
            if (shootPattern != null)
            {
                ExecuteShootPattern();
            }
            else
            {
                DebugManager.Log<PlayerShoot>("PlayerShoot.AttemptShoot: Cooldown ainda ativo.");
            }
        }
        
        #region PowerUP New Bomb

        private void PowerUpAddBomb(ActivePowerUp activePowerUp)
        {
            if (activePowerUp.PowerUpData.powerUpType != PowerUpTypes.Bomb) return;
            if(GetBomb +1 > PlayersManager.Instance.PlayersDefault.maxBombs) return;
            GetBomb += 1;
            _gameHudManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
        }

        #endregion

        public override ISpawnData CreateBulletData(Vector3 direction, Vector3 position)
        {
            return new BombSpawnData(
                _playerMaster,
                bombDamage,
                bombLifeTimer,
                radiusSize,
                radiusSpeed,
                shakeForce,
                shakeTime
            );
        }
        
        /*[SerializeField] private GameObject prefabBomb;
        [SerializeField, Range(1,10)] private int bombDamage;
        [SerializeField, Range(1f,10f)] private float bombOffsetZ;
        [SerializeField, Range(1f,15f)] private float radiusSize;
        [SerializeField, Range(0.1f, 2f)] private float radiusSpeed;
        [SerializeField, Range(1f,10f)] private float shakeForce;
        [SerializeField, Range(0.01f,0.1f)] private float shakeTime;
        [SerializeField, Range(100,1000)] private long millisecondsVibrate;

        private BombSpawnData _bombData;
        private PlayerMaster _playerMaster;
        private GameHudManager _gameHudManager;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _playerMaster.EventPlayerMasterInitialize += InitializeBombs;
            _playerMaster.EventPlayerMasterStartPowerUp += PowerUpAddBomb;
            InputGameManager.RegisterAction("Bomb", AttemptBomb);
        }
        private void OnDisable()
        {
            InputGameManager.UnregisterAction("Bomb", AttemptBomb);
            _playerMaster.EventPlayerMasterInitialize -= InitializeBombs;
            _playerMaster.EventPlayerMasterStartPowerUp -= PowerUpAddBomb;
        }

        #endregion

        private void SetInitialReferences()
        {
            _gameHudManager = GameHudManager.Instance;
            _playerMaster = GetComponent<PlayerMaster>();
            _bombData = new BombSpawnData
            {
                owner = null,
                /*BulletOffSet,
                BulletOffSet = bombOffsetZ,
                BombRadius = radiusSize,
                BombRadiusSpeed = radiusSpeed,
                BombShakeForce = shakeForce,
                BombShakeTime = shakeTime,
                BombMillisecondsVibrate = millisecondsVibrate,
                BulletOwner = _playerMaster,
                BulletTimer = 0.1f,#1#
            };
            _bombData = new BombSpawnData(null, Vector3 direction, Vector3 position, int damage, float speed, float timer, float bombRadius, float bombRadiusSpeed, float bombShakeForce, float bombShakeTime) : base(owner, direction, position, damage, speed, timer, false)
        }

        private void InitializeBombs(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            GetBomb = defaultSettings.startBombs;
            _gameHudManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
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
            _gameHudManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
            GameStatisticManager.instance.LogBombs(1);
        }

        public int GetBomb { get; private set; }

        #region PowerUP New Bomb

        private void PowerUpAddBomb(ActivePowerUp activePowerUp)
        {
            if (activePowerUp.PowerUpData.powerUpType != PowerUpTypes.Bomb) return;
            if(GetBomb +1 > PlayersManager.Instance.PlayersDefault.maxBombs) return;
            GetBomb += 1;
            _gameHudManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
        }

        #endregion

        public override BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position)
        {
            return new BulletSpawnData(
                _playerMaster,
                direction,
                position,
                _playerMaster.ActualSkin.bulletDamage,
                0,
                2.0f,
                false
            );
        }*/
    }
}