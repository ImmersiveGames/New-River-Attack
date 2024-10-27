using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerLives : MonoBehaviour
    {
        private PlayerMaster _playerMaster;
        private GamePlayManager _gamePlayManager;
        private GameHudManager _gameHudManager;
        private PlayersManager _playersManager;
        private int _lives;
        public int GetLives => _lives;

        #region Unity Methodos

        private void OnEnable()
        {
            SetInitialReferences();
            _playerMaster.EventPlayerMasterGetHit += LoseLive;
            _playerMaster.EventPlayerMasterInitialize += InitializeLives;
            _playerMaster.EventPlayerMasterStartPowerUp += PowerUpAddLive;
            _gamePlayManager.EventGameReset += InitializeLives;
            _gamePlayManager.EventObstacleReload += InitializeLives;
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterGetHit -= LoseLive;
            _playerMaster.EventPlayerMasterInitialize -= InitializeLives;
            _playerMaster.EventPlayerMasterStartPowerUp -= PowerUpAddLive;
            _gamePlayManager.EventGameReset -= InitializeLives;
            _gamePlayManager.EventObstacleReload -= InitializeLives;
        }

        #endregion
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _gamePlayManager = GamePlayManager.Instance;
            _playersManager = PlayersManager.Instance;
            _gameHudManager = GameHudManager.Instance;
        }
        
        private void InitializeLives(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            _lives = defaultSettings.startLives;
            _gameHudManager.OnEventHudLivesUpdate(_lives, _playerMaster.PlayerIndex);
        }
        private void InitializeLives()
        {
            _gameHudManager.OnEventHudLivesUpdate(_playersManager.PlayersDefault.startLives, _playerMaster.PlayerIndex);
        }

        private void ChangeLives(int quantity)
        {
            _lives += quantity;
            _gameHudManager.OnEventHudLivesUpdate(_lives, _playerMaster.PlayerIndex);
        }
        
        private void LoseLive()
        {
            GameStatisticManager.instance.LogDeaths(1);
            ChangeLives(-1);
        }

        #region Power Up ExtraLlive

        private void PowerUpAddLive(ActivePowerUp activePowerUp)
        {
            if (activePowerUp.PowerUpData.powerUpType != PowerUpTypes.Live) return;
            ChangeLives(1);
        }

        #endregion
        
    }
}