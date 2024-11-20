using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerShoot : ObjectShoot
    {
        [Header("Power-Up Settings")]
        [SerializeField, Range(0, 5)] private int cadenceDivider = 2;  // Redução do cooldown com power-up
        private float _originalCooldown;  // Armazena o cooldown original para restaurar após o power-up

        private PlayerMaster _playerMaster;
        
        protected override void Awake()
        {
            base.Awake();
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void OnEnable()
        {
            InputGameManager.RegisterAction("Shoot", AttemptShoot);
            _playerMaster.EventPlayerMasterStartPowerUp += StartPowerUp;
            _playerMaster.EventPlayerMasterEndPowerUp += EndPowerUp;
        }

        private void Start()
        {
            // Configura o cooldown inicial com base no valor de ActualSkin
            if (shootPattern == null || _playerMaster.ActualSkin == null) return;
            _originalCooldown = _playerMaster.ActualSkin.cooldownShoot;
            cooldown = _originalCooldown;
            DebugManager.Log<PlayerShoot>($"PlayerShoot.Start: Cooldown inicial definido a partir de ActualSkin: {_originalCooldown}");
        }

        private void OnDisable()
        {
            InputGameManager.UnregisterAction("Shoot", AttemptShoot);
            _playerMaster.EventPlayerMasterStartPowerUp -= StartPowerUp;
            _playerMaster.EventPlayerMasterEndPowerUp -= EndPowerUp;
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

        // Reduz temporariamente o cooldown com power-up de tiro rápido
        private void StartPowerUp(ActivePowerUp activePowerUp)
        {
            if (activePowerUp.PowerUpData.powerUpType != PowerUpTypes.RapidFire || shootPattern == null) return;
            var newCooldown = _originalCooldown / cadenceDivider;
            cooldown = newCooldown;
            DebugManager.Log<PlayerShoot>($"PlayerShoot.StartPowerUp: Cooldown reduzido temporariamente para {newCooldown}");
        }

        // Restaura o cooldown original ao término do power-up
        private void EndPowerUp(ActivePowerUp activePowerUp)
        {
            if (activePowerUp.PowerUpData.powerUpType != PowerUpTypes.RapidFire || shootPattern == null) return;
            cooldown = _originalCooldown;
            DebugManager.Log<PlayerShoot>("PlayerShoot.EndPowerUp: Cooldown restaurado ao valor original.");
        }

        public override ISpawnData CreateBulletData(Vector3 direction, Vector3 position)
        {
            return new BulletSpawnData(
                _playerMaster,
                direction,
                position,
                _playerMaster.ActualSkin.bulletDamage,
                _playerMaster.ActualSkin.playerSpeed * _playerMaster.ActualSkin.bulletSpeedMultiply,
                2.0f,
                false
            );
        }
    }
}
