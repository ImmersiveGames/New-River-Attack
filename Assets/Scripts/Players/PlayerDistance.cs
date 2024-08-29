using NewRiverAttack.SteamGameManagers;
using UnityEngine;

namespace RiverAttack
{
    public class PlayerDistance : MonoBehaviour
    {
        public float conversion;
        public float offsetInitial;

        private Vector3 _lastPosition;
        private float _travelledDistance;
        private float _convertDistance;
        private float _lastConvertDistance;
        private float _maxTravelledDistance;

        private GamePlayManager _gamePlayManager;
        private PlayerMasterOld _playerMasterOld;
        private PlayerSettings _playerSettings;
        private GamePlayingLog _gamePlayingLog;

        #region UNITYMETHODS

        private void OnEnable()
        {
            _gamePlayManager = GamePlayManager.instance;
            _gamePlayingLog = _gamePlayManager.gamePlayingLog;
            _playerMasterOld = GetComponent<PlayerMasterOld>();
            _playerSettings = _playerMasterOld.getPlayerSettings;
        }

        private void Start()
        {
            offsetInitial += PlayerManager.instance.spawnPlayerPosition.z;
            _lastPosition = transform.position;
            _travelledDistance = offsetInitial;
            LoadMaxDistance();
        }

        private void Update()
        {
            var position = transform.position;
            // Calcula a distância percorrida no eixo Z desde o último frame
            var distanceTraveledByFrame = position.z - _lastPosition.z;

            // se a posição for maior que a inicial e não teve nenhum movimento no ultimo frame e não ta permitindo joga então ignore; 
            if (position.z < 0 && distanceTraveledByFrame <= 0 && !_playerMasterOld.ShouldPlayerBeReady) return;

            _travelledDistance += distanceTraveledByFrame;
            // Atualiza o ponto mais distante alcançado
            _maxTravelledDistance = Mathf.Max(_maxTravelledDistance, _travelledDistance);
            // atualiza sempre que a distancia for maior que a ja coberta
            if (_travelledDistance > _lastConvertDistance)
            {
                _lastConvertDistance = _travelledDistance;
                int convertDistanceInt = Mathf.FloorToInt(_lastConvertDistance / conversion);
                _gamePlayManager.OnEventUpdateDistance(convertDistanceInt);
                AchievementHandle(_gamePlayingLog.pathDistance + _maxTravelledDistance);
            }
            
            //Debug.Log($"Update Distance: Distancia Convertida :{_travelledDistance}, Last: {_lastConvertDistance}, Position: {position.z}");
            _lastPosition = position;
            
        }

        private void OnDisable()
        {
            LogGamePlay(_travelledDistance, _maxTravelledDistance);
            
        }

        private void OnApplicationQuit()
        {
            _playerSettings.distance = _maxTravelledDistance;
        }
  #endregion

  private void LoadMaxDistance()
        {
            var settingDistance = _playerMasterOld.getPlayerSettings.distance;
            _maxTravelledDistance = (settingDistance != 0) ? settingDistance : 0;
            _convertDistance = _maxTravelledDistance / conversion;
            _gamePlayManager.OnEventUpdateDistance(Mathf.FloorToInt(_convertDistance));
        }

        private void LogGamePlay(float distance, float maxDistance)
        {
            _gamePlayingLog.maxPathDistance = maxDistance;
            _gamePlayingLog.pathDistance += distance;
            var resultInt = Mathf.FloorToInt(_gamePlayingLog.pathDistance);
            //SteamGameManager.SetStat("stat_FlightDistance", resultInt, true);
        }

        private static void AchievementHandle(float result)
        {
            //Debug.Log($"Valor entrando: {result}");
            //if (!SteamGameManager.ConnectedToSteam) return;
            //var flight = SteamGameManager.GetStatInt("stat_FlightDistance");
            var resultInt = Mathf.FloorToInt(result);
            //Debug.Log($"Valor calculado: {resultInt}");
            //if (flight >= resultInt) return;
            //SteamGameManager.SetStat("stat_FlightDistance", resultInt, true);
        }
    }
}
