using Cinemachine;
using UnityEngine;

namespace ImmersiveGames.CameraManagers
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance { get; private set; }
        
        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineBasicMultiChannelPerlin _cineMachineBasicMultiChannelPerlin;
        
        private float _shakeTimer;
        private float _shakeTimerTotal;
        private float _startingIntensity;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Opcional: se você quiser manter a instância entre cenas
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cineMachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            ShakeTimer();
        }

        public void ShakeCamera(float intensity, float time)
        {
            _cineMachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            _startingIntensity = intensity;
            
            _shakeTimerTotal = time;
            _shakeTimer = time;
        }

        private void ShakeTimer()
        {
            if (_shakeTimer > 0)
            {
                //Debug.Log($"Timer: {_shakeTimer}");
                _shakeTimer -= Time.deltaTime;
                _cineMachineBasicMultiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(_startingIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
            }
            else
            {
                _cineMachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }

        public void StopShake()
        {
            _cineMachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }
}