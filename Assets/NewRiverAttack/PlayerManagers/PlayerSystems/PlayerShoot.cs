using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerShoot : ObjectShoot
    {
        [Header("PowerUp Settings"), SerializeField, Range(0,5)] private int cadenceDivider;
        private PlayerMaster _playerMaster;
        
        private float _originalCadence;
        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _playerMaster.EventPlayerMasterChangeSkin += UpdateCadenceShoot;
            _playerMaster.EventPlayerMasterStartPowerUp += StartPowerUp;
            _playerMaster.EventPlayerMasterEndPowerUp += EndPowerUp;
        }
        private void Start()
        {
            InputGameManager.RegisterAction("Shoot", AttemptShoot);
            SetDataBullet(_playerMaster);
            UpdateCadenceShoot(_playerMaster.ActualSkin);
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterChangeSkin -= UpdateCadenceShoot;
            _playerMaster.EventPlayerMasterStartPowerUp -= StartPowerUp;
            _playerMaster.EventPlayerMasterEndPowerUp -= EndPowerUp;
        }

        #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _playerMaster = GetComponent<PlayerMaster>();
        }
        
        
        private void UpdateCadenceShoot(ShopProductSkin shopProductSkin)
        {
            CadenceShoot = shopProductSkin.cadenceShoot;
            ShootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
        }

        public override void SetDataBullet(ObjectMaster objectMaster)
        {
            var bulletSpeed = 0f;
            var damage = 0;

            // Verifica se o obstáculo é um jogador
            var playerMaster = objectMaster as PlayerMaster;
            
            if (playerMaster != null)
            {
                bulletSpeed = playerMaster.ActualSkin.playerSpeed * playerMaster.ActualSkin.bulletSpeedMultiply;
                damage = playerMaster.ActualSkin.bulletDamage;
            }
            MakeBullet(playerMaster, bulletSpeed, damage, bulletLifeTime, Vector3.forward);
        }

        private void AttemptShoot(InputAction.CallbackContext obj)
        {
            AttemptShoot(_playerMaster);
        }

        protected override void Fire()
        {
            base.Fire();
            LogGamePlay(1);
        }

        #region PowerUP - Rapid Fire
        
        private void EndPowerUp(ActivePowerUp activePowerUp)
        {
            if (activePowerUp.PowerUpData.powerUpType != PowerUpTypes.RapidFire) return;
            CadenceShoot = _originalCadence;
            BulletData.BulletPowerUp = false;
        }

        private void StartPowerUp(ActivePowerUp activePowerUp)
        {
            if (activePowerUp.PowerUpData.powerUpType != PowerUpTypes.RapidFire) return;
            _originalCadence = CadenceShoot;
            CadenceShoot /= cadenceDivider;
            BulletData.BulletPowerUp = true;
        }

        #endregion
        
        private void LogGamePlay(int shoot)
        {
            GamePlayLog.instance.playersShoots += shoot;
        }
        
    }
}