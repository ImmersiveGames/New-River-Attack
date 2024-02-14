using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class UiHubPlayer: MonoBehaviour
    {
        [SerializeField] private Vector3 positionOffset;
        public float moveTime = 1.0f;
        private float m_Timer;
        private Vector3 m_NextPosition;
        private GameHubManager m_GameHubManager;

        private void OnEnable()
        {
            m_GameHubManager = GameHubManager.instance;
            if (GamePlayingLog.instance.activeMission)
            {
                var missions = m_GameHubManager.missions.Find(x=>x.levels == GamePlayingLog.instance.activeMission);
                transform.position = new Vector3(0, 0, missions.position) + positionOffset;
            }
            m_GameHubManager.ChangeMission += MovePlayer;
        }

        private void OnDisable()
        {
            m_GameHubManager.ChangeMission -= MovePlayer;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            Destroy(m_GameHubManager);
        }

        private void MovePlayer(int missionIndex)
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
