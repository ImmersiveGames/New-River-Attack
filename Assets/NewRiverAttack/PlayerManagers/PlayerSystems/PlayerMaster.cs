using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using ImmersiveGames.PoolManagers.Interface;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using NewRiverAttack.SaveManagers;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public sealed class PlayerMaster : ObjectMaster, IHasPool
    {
        [Header("player Settings")] [SerializeField]
        public bool godMode;

        [SerializeField] private float timeoutReSpawn;
        
        [SerializeField] private float timeOutRespawnBoss = 1.5f;
        [SerializeField] private float timeOutRepositionBoss = .5f;

        private int _playerScore;
        private PlayerLives _playerLives;
        internal bool AutoPilot;
        internal bool BossController;

        #region Player Config Settings (privates)

        public int PlayerIndex { get; private set; }
        public ShopProductSkin ActualSkin { get; private set; }

        #endregion

        #region Delagates

        public delegate void PlayerMasterGeneralHandler();

        public event PlayerMasterGeneralHandler EventPlayerMasterRespawn;
        public event PlayerMasterGeneralHandler EventPlayerMasterReady;
        public event PlayerMasterGeneralHandler EventPlayerMasterGetHit;
        public event PlayerMasterGeneralHandler EventPlayerMasterForceExplode;
        public delegate void PlayerMasterObstacleHandler(ICollectable collectable);

        public event PlayerMasterObstacleHandler EventPlayerMasterCollect;

        public delegate void PlayerMasterSkinHandler(ShopProductSkin shopProductSkin);

        public event PlayerMasterSkinHandler EventPlayerMasterChangeSkin;

        public delegate void AxisEventHandler(Vector2 dir);

        public event AxisEventHandler EventPlayerMasterAxisMovement;

        public delegate void PlayerMasterEventHandler(int indexPlayer, PlayersDefaultSettings defaultSettings);

        public event PlayerMasterEventHandler EventPlayerMasterInitialize;

        public delegate void PlayerMasterPowerUpHandler(ActivePowerUp activePowerUp);

        public event PlayerMasterPowerUpHandler EventPlayerMasterStartPowerUp;
        public event PlayerMasterPowerUpHandler EventPlayerMasterEndPowerUp;

        public delegate void PlayerMasterAreaEffectHandler(AreaEffectScriptable areaEffectScriptable);

        public event PlayerMasterAreaEffectHandler EventPlayerMasterAreaEffectStart;
        public event PlayerMasterAreaEffectHandler EventPlayerMasterAreaEffectEnd;

        public delegate void PlayerMasterToggleHandler(bool active);

        public event PlayerMasterToggleHandler EventPlayerMasterToggleSkin;
        public event PlayerMasterToggleHandler EventPlayerMasterStopDecoyFuel;

        #endregion

        #region Initializations

        private void SetPlayerSettings(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            PlayerIndex = indexPlayer;
            _playerScore = 0;
            var gameOptionSave = GameOptionsSave.instance;
            ActualSkin = gameOptionSave.playerSettings[indexPlayer].actualSkin;
            gameOptionSave.SetDefaultSkinPlayer(indexPlayer, defaultSettings.skinDefault);
            _playerLives = GetComponent<PlayerLives>();
            IsDisable = true;
            IsDead = false;
            AutoPilot = false;
            BossController = false;
        }

        protected internal override void SavePosition(Vector3 myPosition)
        {
            DebugManager.Log<PlayerMaster>($"Save Position: {myPosition}");
            base.SavePosition(myPosition);
        }

        #endregion

        #region Object Master

        private void AttemptKillObstacle()
        {
            if (GamePlayManagerRef.IsBossFight) return;
            IsDead = true;
            IsDisable = true;
        }

        private void TryReSpawn()
        {
            var lives = (_playerLives) ? _playerLives.GetLives : GamePlayManagerRef.PlayersDefault.maxLives;
            GameStatisticManager.instance.OnEventServiceUpdate();
            if (lives <= 0)
            {
                if (BossController)
                {
                    IsDisable = true;
                    IsDead = true;
                    OnEventPlayerMasterForceExplode();
                }
                Invoke(nameof(ChangeGameOver), timeoutReSpawn);
                return;
            }

            if (BossController)
            {
                DebugManager.Log<PlayerMaster>("Try respawn Boss ");
                Invoke(nameof(RepositionBoss), timeOutRespawnBoss);
                return;
            }
            Invoke(nameof(Reposition), timeoutReSpawn);
        }

        private void Reposition()
        {
            transform.position = new Vector3(GetLastPositionX,
                GamePlayManagerRef.PlayersDefault.spawnPosition.y,
                GetLastPositionZ);
            transform.Rotate(GamePlayManagerRef.PlayersDefault.spawnPosition);
            OnEventPlayerMasterRespawn();
            Invoke(nameof(ReadyPlayer), timeoutReSpawn);
        }

        private void RepositionBoss()
        {
            DebugManager.Log<PlayerMaster>("RepositionBoss Boss ");
            OnEventPlayerMasterRespawn();
            DebugManager.Log<PlayerMaster>("RepositionBoss OnEventPlayerMasterRespawn");
            Invoke(nameof(ReadyPlayer), timeOutRepositionBoss);
            
        }

        private void ReadyPlayer()
        {
            OnEventPlayerMasterReady();
            IsDisable = false;
            IsDead = false;
        }

        private async void ChangeGameOver()
        {
            DebugManager.Log<PlayerMaster>($"Enter in Game Over");
            GameStatisticManager.instance.OnEventServiceUpdate();
            GamePlayManagerRef.OnEventGameOver();
            await GameManager.StateManager.ChangeStateAsync("GameStateGameOver").ConfigureAwait(false);
        }

        #endregion

        internal void SetPlayerScore(int score)
        {
            _playerScore += score;
            GamePlayManagerRef.OnEventHudScoreUpdate(_playerScore, PlayerIndex);
        }

        internal int GetPlayerScore => _playerScore;
        internal PlayerSettings GetPlayerSettings => GameOptionsSave.instance.playerSettings[PlayerIndex];

        #region Calls

        public void OnEventPlayerMasterInitialize(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            SetPlayerSettings(indexPlayer, defaultSettings);
            EventPlayerMasterInitialize?.Invoke(indexPlayer, defaultSettings);
        }

        public void OnEventPlayerMasterAxisMovement(Vector2 dir)
        {
            EventPlayerMasterAxisMovement?.Invoke(dir);
        }

        public void OnEventPlayerMasterChangeSkin(ShopProductSkin shopProductSkin)
        {
            EventPlayerMasterChangeSkin?.Invoke(shopProductSkin);
        }

        public void OnEventPlayerMasterGetHit()
        {
            AttemptKillObstacle();
            GamePlayManagerRef.OnEventHudRapidFireEnd(0, PlayerIndex);
            EventPlayerMasterGetHit?.Invoke();
            TryReSpawn();
        }

        private void OnEventPlayerMasterRespawn()
        {
            EventPlayerMasterRespawn?.Invoke();
            GamePlayManagerRef.OnEventGameRestart();
        }

        private void OnEventPlayerMasterReady()
        {
            EventPlayerMasterReady?.Invoke();
            GamePlayManagerRef.OnEventGameReady();
        }

        public void OnEventPlayerMasterCollect(ICollectable collectable)
        {
            EventPlayerMasterCollect?.Invoke(collectable);
        }

        public void OnEventPlayerMasterStartPowerUp(ActivePowerUp activePowerUp)
        {
            EventPlayerMasterStartPowerUp?.Invoke(activePowerUp);
        }

        public void OnEventPlayerMasterEndPowerUp(ActivePowerUp activePowerUp)
        {
            EventPlayerMasterEndPowerUp?.Invoke(activePowerUp);
        }

        internal void OnEventPlayerMasterAreaEffectStart(AreaEffectScriptable areaEffectScriptable)
        {
            EventPlayerMasterAreaEffectStart?.Invoke(areaEffectScriptable);
        }

        internal void OnEventPlayerMasterAreaEffectEnd(AreaEffectScriptable areaEffectScriptable)
        {
            EventPlayerMasterAreaEffectEnd?.Invoke(areaEffectScriptable);
        }

        internal void OnEventPlayerMasterToggleSkin(bool active)
        {
            EventPlayerMasterToggleSkin?.Invoke(active);
        }
        private void OnEventPlayerMasterForceExplode()
        {
            EventPlayerMasterForceExplode?.Invoke();
        }
        internal void OnEventPlayerMasterStopDecoyFuel(bool pause)
        {
            EventPlayerMasterStopDecoyFuel?.Invoke(pause);
        }
        #endregion
        
    }
}