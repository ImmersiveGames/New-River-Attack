using Steamworks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;

namespace RiverAttack
{
    public class PlayerDistance : MonoBehaviour
    {
        public float conversion;
        [FormerlySerializedAs("offsetInicial")] public float offsetInitial;

        private Vector3 m_LastPosition;
        private float m_TravelledDistance;
        private float m_ConvertDistance;
        private float m_LastConvertDistance;
        private float m_MaxTravelledDistance;

        private GamePlayManager m_GamePlayManager;
        private PlayerMaster m_PlayerMaster;
        private PlayerSettings m_PlayerSettings;
        private GamePlayingLog m_GamePlayingLog;

        #region UNITYMETHODS

        private void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlayingLog = m_GamePlayManager.gamePlayingLog;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;
        }

        private void Start()
        {
            offsetInitial += PlayerManager.instance.spawnPlayerPosition.z;
            m_LastPosition = transform.position;
            m_TravelledDistance = offsetInitial;
            LoadMaxDistance();
        }

        private void Update()
        {
            var position = transform.position;
            // Calcula a distância percorrida no eixo Z desde o último frame
            float distanceTraveledByFrame = position.z - m_LastPosition.z;

            // se a posição for maior que a inicial e não teveve nenhum movimento no ultimo frame e não ta permitindo joga então ignore; 
            if (position.z < 0 && distanceTraveledByFrame <= 0 && !m_PlayerMaster.shouldPlayerBeReady) return;

            m_TravelledDistance += distanceTraveledByFrame;
            // Atualiza o ponto mais distante alcançado
            m_MaxTravelledDistance = Mathf.Max(m_MaxTravelledDistance, m_TravelledDistance);
            // atualiza sempre que a distancia for maior que a ja coberta
            if (m_TravelledDistance > m_LastConvertDistance)
            {
                m_LastConvertDistance = m_TravelledDistance;
                int convertDistanceInt = Mathf.FloorToInt(m_LastConvertDistance / conversion);
                m_GamePlayManager.OnEventUpdateDistance(convertDistanceInt);
                AchievementHandle(m_GamePlayingLog.pathDistance + m_MaxTravelledDistance);
            }
            
            //Debug.Log($"Update Distance: Distancia Convertida :{m_TravelledDistance}, Last: {m_LastConvertDistance}, Position: {position.z}");
            m_LastPosition = position;
            
        }

        private void OnDisable()
        {
            LogGamePlay(m_TravelledDistance, m_MaxTravelledDistance);
            
        }

        private void OnApplicationQuit()
        {
            m_PlayerSettings.distance = m_MaxTravelledDistance;
        }
  #endregion

  private void LoadMaxDistance()
        {
            float settingDistance = m_PlayerMaster.getPlayerSettings.distance;
            m_MaxTravelledDistance = (settingDistance != 0) ? settingDistance : 0;
            m_ConvertDistance = m_MaxTravelledDistance / conversion;
            m_GamePlayManager.OnEventUpdateDistance(Mathf.FloorToInt(m_ConvertDistance));
        }

        private void LogGamePlay(float distance, float maxDistance)
        {
            m_GamePlayingLog.maxPathDistance = maxDistance;
            m_GamePlayingLog.pathDistance += distance;
            int resultInt = Mathf.FloorToInt(m_GamePlayingLog.pathDistance);
            GameSteamManager.SetStat("stat_FlightDistance", resultInt, true);
        }

        private static void AchievementHandle(float result)
        {
            //Debug.Log($"Valor entrando: {result}");
            int flight = GameSteamManager.GetStatInt("stat_FlightDistance");
            int resultInt = Mathf.FloorToInt(result);
            //Debug.Log($"Valor calculado: {resultInt}");
            if (flight >= resultInt) return;
            GameSteamManager.SetStat("stat_FlightDistance", resultInt, true);
        }
    }
}
