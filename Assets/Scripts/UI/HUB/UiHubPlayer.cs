using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class UiHubPlayer: MonoBehaviour
    {
        [SerializeField] Vector3 positionOffset;
        public float moveTime = 1.0f;
        float m_Timer;
        Vector3 m_NextPosition;
        GameHubManager m_GameHubManager;

        void OnEnable()
        {
            m_GameHubManager = GameHubManager.instance;
            if (GamePlayingLog.instance.activeMission)
            {
                var missions = m_GameHubManager.missions.Find(x=>x.levels == GamePlayingLog.instance.activeMission);
                transform.position = new Vector3(0, 0, missions.position) + positionOffset;
            }
            m_GameHubManager.ChangeMission += MovePlayer;
        }

        void OnDisable()
        {
            m_GameHubManager.ChangeMission -= MovePlayer;
        }

        void OnDestroy()
        {
            StopAllCoroutines();
            Destroy(m_GameHubManager);
        }

        void MovePlayer(int missionIndex)
        {
            float newPosition = GameHubManager.instance.missions[missionIndex].position;

            m_NextPosition = new Vector3(0, 0, newPosition) + positionOffset;
            
            StartCoroutine(moveTime.Tweeng( (p)=>transform.position=p,
                transform.position,
                m_NextPosition) );
            Invoke(nameof(MoveFalse),moveTime);
            
        }
        public void MoveFalse()
        {
            m_GameHubManager.readyHub = true;
        }
    }
}
