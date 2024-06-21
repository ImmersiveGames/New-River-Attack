using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using ImmersiveGames.PoolManagers.Interface;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems;
using NewRiverAttack.ObstaclesSystems.Abstracts;
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

        public delegate void PlayerMasterAreaEffectHandler(ObstacleTypes obstacleTypes);

        public event PlayerMasterAreaEffectHandler EventPlayerMasterAreaEffectStart;
        public event PlayerMasterAreaEffectHandler EventPlayerMasterAreaEffectEnd;

        public delegate void PlayerMasterToggleHandler(bool active);

        public event PlayerMasterToggleHandler EventPlayerMasterToggleSkin;

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

        protected internal override void InitializeObject()
        {
            //No caso aqui é pra inicializar da posição zero ao iniciar
            base.InitializeObject();
            SavePosition(Vector3.zero);
        }

        #endregion

        #region Object Master

        private void AttemptKillObstacle()
        {
            if (GamePlayManagerRef.IsBossFight) return;
            IsDisable = true;
            IsDead = true;
        }

        private void TryReSpawn()
        {
            var lives = (_playerLives) ? _playerLives.GetLives : GamePlayManagerRef.PlayersDefault.maxLives;

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
                Invoke(nameof(RepositionBoss), timeOutRespawnBoss);
                return;
            }

            Invoke(nameof(Reposition), timeoutReSpawn);
        }

        private void Reposition()
        {
            transform.position = new Vector3(GamePlayManagerRef.PlayersDefault.spawnPosition.x,
                GamePlayManagerRef.PlayersDefault.spawnPosition.y,
                GetLastPositionZ);
            transform.Rotate(GamePlayManagerRef.PlayersDefault.spawnPosition);
            OnEventPlayerMasterRespawn();
            Invoke(nameof(ReadyPlayer), timeoutReSpawn);
        }

        private void RepositionBoss()
        {
            OnEventPlayerMasterRespawn();
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
            GamePlayManager.instance.OnEventGameOver();
            await GameManager.StateManager.ChangeStateAsync("GameStateGameOver").ConfigureAwait(false);
        }

        #endregion

        internal void SetPlayerScore(int score)
        {
            _playerScore += score;
            GamePlayManager.instance.OnEventHudScoreUpdate(_playerScore, PlayerIndex);
        }

        internal int GetPlayerScore => _playerScore;


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

        internal void OnEventPlayerMasterAreaEffectStart(ObstacleTypes obstacleTypes)
        {
            EventPlayerMasterAreaEffectStart?.Invoke(obstacleTypes);
        }

        internal void OnEventPlayerMasterAreaEffectEnd(ObstacleTypes obstacleTypes)
        {
            EventPlayerMasterAreaEffectEnd?.Invoke(obstacleTypes);
        }

        internal void OnEventPlayerMasterToggleSkin(bool active)
        {
            EventPlayerMasterToggleSkin?.Invoke(active);
        }
        private void OnEventPlayerMasterForceExplode()
        {
            EventPlayerMasterForceExplode?.Invoke();
        }

        #endregion

        
    }
}