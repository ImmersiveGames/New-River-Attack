using ImmersiveGames.DebugManagers;
using UnityEngine;
using GamePlayManager = NewRiverAttack.GamePlayManagers.GamePlayManager;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerDistance : MonoBehaviour
    {

        private const int BaseConversion = 10;
        private Vector3 _lastPosition;
        private float _travelledDistance;
        
        private PlayerMaster _playerMaster;
        private PlayerAchievements _playerAchievements;
        private GamePlayManager _gamePlayManager;

        #region Unity Region

        private void Start()
        {
            SetInitialReferences();
            _lastPosition = transform.position;
            _travelledDistance = _gamePlayManager.PlayersDefault.spawnPosition.z;
        }

        private void Update()
        {
            UpdatePlayerDistance();
        }

        #endregion
        
        private void UpdatePlayerDistance()
        {
            var position = transform.position;
            // Calcula a distância percorrida no eixo Z desde o último frame
            var distanceTraveledByFrame = position.z - _lastPosition.z;
            // se a posição for maior que a inicial e não teve nenhum movimento no ultimo frame e não ta permitindo joga então ignore;
            if (position.z < 0 || distanceTraveledByFrame <= 0 || !_playerMaster.ObjectIsReady || _playerMaster.InFinishPath) return;
            _travelledDistance += distanceTraveledByFrame;
            // Atualiza o ponto mais distante alcançado
            //_maxTravelledDistance = Mathf.Max(_maxTravelledDistance, _travelledDistance);
            
            DebugManager.Log<PlayerDistance>($"Distance: {_travelledDistance}");
            
            var convertDistanceInt = Mathf.FloorToInt(_travelledDistance / BaseConversion);
            _gamePlayManager.OnEventHudDistanceUpdate(convertDistanceInt, _playerMaster.PlayerIndex);
            var resultInt = Mathf.FloorToInt(_travelledDistance);
            _playerAchievements.LogDistanceReach(resultInt);
            
            _lastPosition = position;
        }
        
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>(); 
            _playerAchievements = GetComponent<PlayerAchievements>();
            _gamePlayManager = GamePlayManager.instance;
        }
    }
}