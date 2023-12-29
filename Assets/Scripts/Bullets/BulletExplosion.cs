using System;
using System.Collections;
using UnityEngine;
namespace RiverAttack
{
    public class BulletExplosion : Bullets
    {
        [SerializeField] float radioSize;
        [SerializeField] float radiusSpeed;
        [SerializeField] float shakeForce;
        [SerializeField] float shakeTime;
        [SerializeField] long millisecondsVibrate;
        
        double m_TParam;
        
        float m_StartTime;
        
        ParticleSystem m_ParticleSystem;
        AudioSource m_AudioSource;
        SphereCollider m_Collider;
        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            audioShoot.Play(m_AudioSource);
            m_Collider = GetComponent<SphereCollider>();
            
            m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
            bulletLifeTime = m_ParticleSystem.main.duration;
            hasBulletLifeTime = bulletLifeTime > 0;
            m_StartTime = Time.time + bulletLifeTime;
            
        }
        void FixedUpdate()
        {
            m_TParam += Time.deltaTime * radiusSpeed;
            CameraShake.ShakeCamera(shakeForce, shakeTime);

//TODO: Arrumar nova forma de vibrar o celular
#if UNITY_ANDROID && !UNITY_EDITOR
            ToolsAndroid.Vibrate(millisecondsVibrate);
            Handheld.Vibrate();
#endif
            m_Collider.radius = Mathf.Lerp(0.3f, radioSize, (float)m_TParam);
            AutoDestroy();
        }
        void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        void AutoDestroy()
        {
            if (Time.time >= m_StartTime)
            {
                DestroyMe();
            }
        }
        protected override void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
