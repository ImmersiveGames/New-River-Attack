using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerBombs : ObjectShoot
    {
        [Header("BOMB SETTINGS")]
        [SerializeField, Range(1,10)] private int bombDamage;
        [SerializeField, Range(1f,5f)] private float bombLifeTimer = 2.0f;
        
        [SerializeField, Range(1f,15f)] private float radiusSize;
        [SerializeField, Range(0.1f, 2f)] private float radiusSpeed;
        
        [SerializeField, Range(1f,10f)] private float shakeForce;
        [SerializeField, Range(0.01f,0.1f)] private float shakeTime;
        
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
            GamePlayManager.Instance.EventGameReadyGo += UpdateBomb;
        }

        private void OnDisable()
        {
            InputGameManager.UnregisterAction("Bomb", AttemptShoot);
            _playerMaster.EventPlayerMasterInitialize -= InitializeBombs;
            _playerMaster.EventPlayerMasterStartPowerUp -= PowerUpAddBomb;
            GamePlayManager.Instance.EventGameReadyGo -= UpdateBomb;
        }

        private void UpdateBomb()
        {
            _gameHudManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
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
                if(GetBomb <=0)return;
                ExecuteShootPattern();
                GetBomb -= 1;
                if (GetBomb < 0) GetBomb = 0;
                _gameHudManager.OnEventHudBombUpdate(GetBomb, _playerMaster.PlayerIndex);
                GameStatisticManager.instance.LogBombs(1);
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
    }
}