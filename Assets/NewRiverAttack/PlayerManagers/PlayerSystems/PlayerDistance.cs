using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.GameStatisticsSystem;
using UnityEngine;
using GamePlayManager = NewRiverAttack.GamePlayManagers.GamePlayManager;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerDistance : MonoBehaviour
    {
        // Configurações de conversão
        
        private float _lastPosition;
        private float _travelledDistance;
        
        private float _totalAccumulatedDistance;
        private float _distanceThisAttempt;
        
        private PlayerMaster _playerMaster;
        private GamePlayManager _gamePlayManager;
        

        #region Unity Region

        private void OnEnable()
        {
            SetInitialReferences();
            _playerMaster.EventPlayerMasterGetHit += ResetDistance;
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterGetHit -= ResetDistance;
        }

        private void Update()
        {
            UpdatePlayerDistance();
        }

        #endregion

        private void ResetDistance()
        {
            _distanceThisAttempt = 0;
        }
        
        private void UpdatePlayerDistance()
        {
            var positionZ = transform.position.z;
            if (!(positionZ > 0)) return;
            var distanceTraveledByFrame = positionZ - _lastPosition;
            if (distanceTraveledByFrame > 0 && _playerMaster.ObjectIsReady && !_playerMaster.AutoPilot)
            {
                _distanceThisAttempt += distanceTraveledByFrame;
                _totalAccumulatedDistance += distanceTraveledByFrame;
                GameStatisticManager.instance.LogAmountDistance(_totalAccumulatedDistance);

                // Converte a distância total acumulada em um valor inteiro com base na conversão
                var convertDistanceInt = Mathf.FloorToInt(_distanceThisAttempt / GemeStatisticsDataLog.BaseConversion);
                _gamePlayManager.OnEventHudDistanceUpdate(convertDistanceInt, _playerMaster.PlayerIndex);
            }
            _lastPosition = transform.position.z;
        }
        
        
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>(); 
            _gamePlayManager = GamePlayManager.Instance;
        }
    }
}