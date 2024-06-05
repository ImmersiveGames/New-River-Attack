using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerLives : MonoBehaviour
    {
        private PlayerMaster _playerMaster;
        private GamePlayManager _gamePlayManager;
        [SerializeField] private int _lives;
        public int GetLives => _lives;

        #region Unity Methodos

        private void OnEnable()
        {
            SetInitialReferences();
            _playerMaster.EventPlayerMasterGetHit += LoseLive;
            _playerMaster.EventPlayerMasterInitialize += InitializeLives;
            _playerMaster.EventPlayerMasterStartPowerUp += PowerUpAddLive;
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterGetHit -= LoseLive;
            _playerMaster.EventPlayerMasterInitialize -= InitializeLives;
            _playerMaster.EventPlayerMasterStartPowerUp -= PowerUpAddLive;
        }

        #endregion
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _gamePlayManager = GamePlayManager.instance;
        }
        
        private void InitializeLives(int indexPlayer, PlayersDefaultSettings defaultSettings)
        {
            _lives = defaultSettings.startLives;
            _gamePlayManager.OnEventHudLivesUpdate(_lives, _playerMaster.PlayerIndex);
        }

        private void ChangeLives(int quantity)
        {
            _lives += quantity;
            //LogGamePlay(1);
            _gamePlayManager.OnEventHudLivesUpdate(_lives, _playerMaster.PlayerIndex);
        }
        
        private void LoseLive()
        {
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