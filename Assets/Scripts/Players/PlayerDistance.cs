using UnityEngine;

namespace RiverAttack
{
    public class PlayerDistance : MonoBehaviour
    { 
        public float conversion;
        public float offsetInicial;
        
        Vector3 m_LastPosition; 
        float m_TravelledDistance; 
        float m_ConvertDistance; 
        float m_LastConvertDistance;
        float m_MaxTravelledDistance;

        GamePlayManager m_GamePlayManager;
        PlayerMaster m_PlayerMaster;
        PlayerSettings m_PlayerSettings;
        GamePlaySettings m_GamePlaySettings;

        #region UNITYMETHODS
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlaySettings = m_GamePlayManager.gamePlaySettings;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_PlayerSettings = m_PlayerMaster.getPlayerSettings;
        }
        void Start()
        {
            offsetInicial += GameManager.instance.spawnPlayerPosition.z;
            m_LastPosition = transform.position;
            m_TravelledDistance = offsetInicial;
            LoadMaxDistancia();
        }
        void Update()
        {
            // Calcula a distância percorrida no eixo Z desde o último frame
            float distanciaZ = Mathf.Abs(transform.position.z - m_LastPosition.z);
            
            if (transform.position.z < 0 && distanciaZ <= 0 && !m_PlayerMaster.shouldPlayerBeReady) return;

            m_TravelledDistance += distanciaZ;

            // Atualiza o ponto mais distante alcançado
            m_MaxTravelledDistance = Mathf.Max(m_MaxTravelledDistance, m_TravelledDistance);

            // Calcula a distância convertida com base na conversão configurada
            m_ConvertDistance = m_MaxTravelledDistance / conversion;
            
            int convertDistanceInt = Mathf.FloorToInt(m_ConvertDistance);
            if (convertDistanceInt > Mathf.FloorToInt(m_LastConvertDistance))
            {
                // Atualiza o valor anterior com o valor atual
                m_LastConvertDistance = m_ConvertDistance;
                m_GamePlayManager.OnEventUpdateDistance(Mathf.FloorToInt(m_ConvertDistance));
            }

            // Atualiza a posição anterior
            m_LastPosition = transform.position;
        }
        void OnDisable()
        {
            LogGamePlay(m_TravelledDistance,m_MaxTravelledDistance);
        }
        void OnApplicationQuit()
        {
            m_PlayerSettings.distance = m_MaxTravelledDistance;
        }
  #endregion
        void LoadMaxDistancia()
        {
            float settingDistance = m_PlayerMaster.getPlayerSettings.distance;
            m_MaxTravelledDistance = (settingDistance != 0) ? settingDistance : 0;
            m_ConvertDistance = m_MaxTravelledDistance / conversion;
            m_GamePlayManager.OnEventUpdateDistance(Mathf.FloorToInt(m_ConvertDistance));
        }

        void LogGamePlay(float distance, float maxDistance)
        {
            m_GamePlaySettings.maxPathDistance = maxDistance;
            m_GamePlaySettings.pathDistance += distance;
        }
    }
}
