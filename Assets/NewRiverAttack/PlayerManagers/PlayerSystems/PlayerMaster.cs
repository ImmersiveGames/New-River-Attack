using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using ImmersiveGames.PoolManagers.Interface;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
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

        private PlayersManager _playersManager;
        private GameHudManager _gameHudManager;

        #region Player Config Settings (privates)

        public int PlayerIndex { get; private set; }
        public ShopProductSkin ActualSkin { get; private set; }

        #endregion

        #region Delagates
        public event Action EventPlayerMasterRespawn;
        public event Action EventPlayerMasterReady;
        public event Action EventPlayerMasterGetHit;
        public event Action EventPlayerMasterForceExplode;
        public event Action<ICollectable> EventPlayerMasterCollect;
        public event Action<ShopProductSkin> EventPlayerMasterChangeSkin;
        public event Action<Vector2> EventPlayerMasterAxisMovement;
        public event Action<int,PlayersDefaultSettings> EventPlayerMasterInitialize;
        public event Action<ActivePowerUp> EventPlayerMasterStartPowerUp;
        public event Action<ActivePowerUp> EventPlayerMasterEndPowerUp;
        public event Action<AreaEffectScriptable> EventPlayerMasterAreaEffectStart;
        public event Action<AreaEffectScriptable> EventPlayerMasterAreaEffectEnd;
        public event Action<bool> EventPlayerMasterToggleSkin;
        public event Action<bool> EventPlayerMasterStopDecoyFuel;

        #endregion

        #region Initializations

        private void SetPlayerSettings(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            PlayerIndex = indexPlayer;
            _playerScore = 0;
            var gameOptionSave = GameOptionsSave.Instance;
            ActualSkin = gameOptionSave.playerSettings[indexPlayer].actualSkin;
            gameOptionSave.SetDefaultSkinPlayer(indexPlayer, defaultSettings.skinDefault);
            _playerLives = GetComponent<PlayerLives>();
            IsDisable = true;
            IsDead = false;
            AutoPilot = false;
            BossController = false;
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _playersManager = PlayersManager.Instance;
            _gameHudManager = GameHudManager.Instance;
        }

        #endregion

        #region Object Master

        private void AttemptKillObstacle()
        {
            if (GameLevelManager.Instance.IsBossFight) return;
            IsDead = true;
            IsDisable = true;
        }

        private void TryReSpawn()
        {
            var lives = (_playerLives) ? _playerLives.GetLives : _playersManager.PlayersDefault.maxLives;
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
                _playersManager.PlayersDefault.spawnPosition.y,
                GetLastPositionZ);
            transform.Rotate(_playersManager.PlayersDefault.spawnPosition);
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
            DebugManager.Log<PlayerMaster>("ReadyPlayer ");
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
            _gameHudManager.OnEventHudScoreUpdate(_playerScore, PlayerIndex);
        }

        internal int GetPlayerScore => _playerScore;
        internal PlayerSettings GetPlayerSettings => GameOptionsSave.Instance.playerSettings[PlayerIndex];

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
            _gameHudManager.OnEventHudRapidFireEnd(0, PlayerIndex);
            EventPlayerMasterGetHit?.Invoke();
            TryReSpawn();
        }

        private void OnEventPlayerMasterRespawn()
        {
            EventPlayerMasterRespawn?.Invoke();
            GamePlayManagerRef.OnEventObstacleReload();
        }

        private void OnEventPlayerMasterReady()
        {
            EventPlayerMasterReady?.Invoke();
            GamePlayManagerRef.OnEventGameReadyGo();
        }

        public void OnEventPlayerMasterCollect(ICollectable collectable)
        {
            DebugManager.Log<PlayerMaster>($"Event Collect");
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