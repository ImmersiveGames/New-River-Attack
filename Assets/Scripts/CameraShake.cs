using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RiverAttack 
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance { get; private set; }
        
        CinemachineVirtualCamera myVirtualCamera;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
        
        float shakeTimer;
        float shakeTimerTotal;
        float startingIntensity;

        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
            myVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            cinemachineBasicMultiChannelPerlin = myVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        // Update is called once per frame
        void Update()
        {
            ShakeTimer();         
        }

        public void ShakeCamera(float intensity, float time) 
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            startingIntensity = intensity;
            
            shakeTimerTotal = time;
            shakeTimer = time;
        }

        void ShakeTimer() 
        {
            if (shakeTimer > 0) 
            {
                shakeTimer -= Time.deltaTime;

                cinemachineBasicMultiChannelPerlin =
                    myVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(startingIntensity, 0f, (1 - (shakeTimer / shakeTimerTotal)));
            }            
        }
    }
}

