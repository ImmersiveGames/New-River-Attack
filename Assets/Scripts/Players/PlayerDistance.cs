using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerDistance : MonoBehaviour
    {
        public float conversaoXParaY = 10.0f; // Valor de conversão
    public string unidadeDeMedida = "KM"; // Unidade de medida
    public float offsetInicial = 0.0f; // Offset para o início da distância

    Vector3 lastPosition;
    float distanciaPercorridaZ;
    float distanciaConvertida;
    float maxDistanciaPercorridaZ; // Ponto mais distante alcançado

    const string MaxDistanciaKey = "MaxDistanciaKey"; // Chave para o valor máximo da distância

    void Start()
    {
        lastPosition = transform.position;
        distanciaPercorridaZ = offsetInicial;
        LoadMaxDistancia();
    }

    void Update()
    {
        // Calcula a distância percorrida no eixo Z desde o último frame
        float distanciaZ = Mathf.Abs(transform.position.z - lastPosition.z);

        // Verifica se a distância é positiva (movimento para frente)
        if (distanciaZ > 0)
        {
            // Adiciona a distância ao registro total
            distanciaPercorridaZ += distanciaZ;

            // Atualiza o ponto mais distante alcançado
            maxDistanciaPercorridaZ = Mathf.Max(maxDistanciaPercorridaZ, distanciaPercorridaZ);
        }

        // Calcula a distância convertida com base na conversão configurada
        distanciaConvertida = maxDistanciaPercorridaZ / conversaoXParaY;

        // Atualiza a posição anterior
        lastPosition = transform.position;

        // Exibe a distância percorrida e convertida no console
        Debug.Log($"Distância percorrida no eixo Z: {distanciaPercorridaZ} unidades | Distância convertida: {distanciaConvertida} {unidadeDeMedida}");
    }

    void OnApplicationQuit()
    {
        SaveMaxDistancia();
    }

    private void SaveMaxDistancia()
    {
        PlayerPrefs.SetFloat(MaxDistanciaKey, maxDistanciaPercorridaZ);
        PlayerPrefs.Save();
    }

    void LoadMaxDistancia()
    {
        maxDistanciaPercorridaZ = PlayerPrefs.GetFloat(MaxDistanciaKey, offsetInicial);
    }
        /*[SerializeField] float cadenceDistance;
        GamePlayManager m_GamePlayManager;
        PlayerMaster m_PlayerMaster;
        int distanceOffset { get; set; }
        [SerializeField] int pathDistance;

        [SerializeField] float checkTime = 2;
        [SerializeField] float lifeTime;

    #region UNITYMETHODS
        void Awake()
        {
            pathDistance = 0;
        }
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerMasterReSpawn += ClearDistance;
            m_GamePlayManager.EventCheckPoint += Log;
        }
        void LateUpdate()
        {
            UpdateDistance();
            if (!(Time.time > lifeTime))
                return;
            lifeTime = Time.time + checkTime;
            m_GamePlayManager.CallEventCheckPlayerPosition(transform.position);
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterReSpawn -= ClearDistance;
            m_GamePlayManager.EventCheckPoint -= Log;
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_GamePlayManager = GamePlayManager.instance;
            distanceOffset = (int)(m_GamePlayManager.GetActualLevel().levelMilestones[0].z / cadenceDistance);
            lifeTime = Time.time + checkTime;
        }

        void Log(Vector3 position)
        {
            GamePlaySettings.instance.pathDistance = pathDistance;
        }

        void UpdateDistance()
        {
            if (m_PlayerMaster.ShouldPlayerBeReady())
                pathDistance = (int)(transform.position.z / cadenceDistance) - distanceOffset;
        }

        void ClearDistance()
        {
            pathDistance = (int)(transform.position.z / cadenceDistance) - distanceOffset;
        }*/
    }
}
