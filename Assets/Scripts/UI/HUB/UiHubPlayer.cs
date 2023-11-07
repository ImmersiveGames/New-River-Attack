using System;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class UiHubPlayer: MonoBehaviour
    {
        [SerializeField] float positionOffset;
        public float moveTime = 1.0f;
        public float rotateTime = .5f;
        bool m_InMovement;
        
        float m_Timer;
        Vector3 m_NextPosition;
        GameHubManager m_GameHubManager;

        void Awake()
        {
            m_GameHubManager = GameHubManager.instance;
        }
        void OnEnable()
        {
            if (GamePlayingLog.instance.lastMissionIndex == 0)
            {
                transform.position = new Vector3(5f, 0, 5f);
            }
            else
            {
                float newPosition = m_GameHubManager.hubMilestones[GamePlayingLog.instance.lastMissionIndex];
                var transform1 = transform;
                var position = transform1.position;
                position = new Vector3(position.x, position.y, newPosition + positionOffset);
                transform1.position = position;
            }
            m_GameHubManager.MissionIndex += UpdatePosition;
        }

        void OnDisable()
        {
            m_GameHubManager.MissionIndex -= UpdatePosition;
        }
        void OnDestroy()
        {
            StopAllCoroutines();
        }
        
        public void MoveFalse()
        {
            m_InMovement = false;
        }

        void Reposition()
        {
            /*StartCoroutine(.1f.Tweeng((t) => transform.eulerAngles = t,
                new Vector3(0, 180, 0),
                Vector3.zero));*/
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        void UpdatePosition(int index)
        {
            if(m_GameHubManager.readyHub == false) return;
            float newPosition = m_GameHubManager.hubMilestones[index];
            if (m_InMovement) return;
            var position = transform.position;
            m_NextPosition = new Vector3(position.x, position.y, newPosition + positionOffset);
            m_InMovement = true;
            if (position.z > newPosition && transform.eulerAngles.y == 0f)
            {
                /*StartCoroutine(rotateTime.Tweeng((t) => transform.eulerAngles = t,
                    Vector3.zero,
                    new Vector3(0, 180, 0)));*/
                transform.eulerAngles = new Vector3(0, 180, 0);
                /*Invoke(nameof(Reposition),moveTime-rotateTime);*/
                Invoke(nameof(Reposition),moveTime);
            }
            StartCoroutine(moveTime.Tweeng( (p)=>transform.position=p,
                position,
                m_NextPosition) );
            Invoke(nameof(MoveFalse),moveTime);
        }
    }
}
