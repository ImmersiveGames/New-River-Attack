using UnityEngine;
using Cinemachine;
using Utils;

namespace RiverAttack 
{
    public class CameraShake : MonoBehaviour
    {
        private CinemachineVirtualCamera m_MyVirtualCamera;
        private static CinemachineBasicMultiChannelPerlin _cineMachineBasicMultiChannelPerlin;

        private static float _shakeTimer;
        private static float _shakeTimerTotal;
        private static float _startingIntensity;

        // Start is called before the first frame update
        private void Awake()
        {
            m_MyVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cineMachineBasicMultiChannelPerlin = m_MyVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        // Update is called once per frame
        private void Update()
        {
            ShakeTimer();         
        }

        public static void ShakeCamera(float intensity, float time) 
        {
            _cineMachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            _startingIntensity = intensity;
            
            _shakeTimerTotal = time;
            _shakeTimer = time;
        }

        private void ShakeTimer()
        {
            if (!(_shakeTimer > 0))
                return;
            _shakeTimer -= Time.deltaTime;

            _cineMachineBasicMultiChannelPerlin =
                m_MyVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _cineMachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(_startingIntensity, 0f, (1 - (_shakeTimer / _shakeTimerTotal)));
        }
    }
}

