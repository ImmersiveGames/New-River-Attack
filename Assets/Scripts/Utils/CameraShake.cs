using UnityEngine;
using Cinemachine;
using Utils;

namespace RiverAttack 
{
    public class CameraShake : Singleton<CameraShake>
    {

        CinemachineVirtualCamera m_MyVirtualCamera;
        CinemachineBasicMultiChannelPerlin m_CineMachineBasicMultiChannelPerlin;
        
        float m_ShakeTimer;
        float m_ShakeTimerTotal;
        float m_StartingIntensity;

        // Start is called before the first frame update
        void Awake()
        {
            m_MyVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            m_CineMachineBasicMultiChannelPerlin = m_MyVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        // Update is called once per frame
        void Update()
        {
            ShakeTimer();         
        }

        public void ShakeCamera(float intensity, float time) 
        {
            m_CineMachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            m_StartingIntensity = intensity;
            
            m_ShakeTimerTotal = time;
            m_ShakeTimer = time;
        }

        void ShakeTimer()
        {
            if (!(m_ShakeTimer > 0))
                return;
            m_ShakeTimer -= Time.deltaTime;

            m_CineMachineBasicMultiChannelPerlin =
                m_MyVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            m_CineMachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(m_StartingIntensity, 0f, (1 - (m_ShakeTimer / m_ShakeTimerTotal)));
        }
    }
}

