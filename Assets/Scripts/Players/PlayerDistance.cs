using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    public class PlayerDistance : MonoBehaviour
    {
        [SerializeField] float cadenceDistance;
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
            if (m_PlayerMaster.ShouldPlayerMove())
                pathDistance = (int)(transform.position.z / cadenceDistance) - distanceOffset;
        }

        void ClearDistance()
        {
            pathDistance = (int)(transform.position.z / cadenceDistance) - distanceOffset;
        }
    }
}
