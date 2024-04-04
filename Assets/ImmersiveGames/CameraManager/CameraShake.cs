using Cinemachine;
using UnityEngine;

namespace ImmersiveGames.CameraManager
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShake:MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        private static CinemachineBasicMultiChannelPerlin _cineMachineBasicMultiChannelPerlin;
        
        private static float _shakeTimer;
        private static float _shakeTimerTotal;
        private static float _startingIntensity;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cineMachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
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
                _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _cineMachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(_startingIntensity, 0f, (1 - (_shakeTimer / _shakeTimerTotal)));
        }
    }
}