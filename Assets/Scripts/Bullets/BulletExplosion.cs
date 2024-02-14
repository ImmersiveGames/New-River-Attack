using System;
using System.Collections;
using UnityEngine;
namespace RiverAttack
{
    public class BulletExplosion : Bullets
    {
        [SerializeField] private float radioSize;
        [SerializeField] private float radiusSpeed;
        [SerializeField] private float shakeForce;
        [SerializeField] private float shakeTime;
        [SerializeField] private long millisecondsVibrate;

        private double m_TParam;

        private float m_StartTime;

        private ParticleSystem m_ParticleSystem;
        private AudioSource m_AudioSource;
        private SphereCollider m_Collider;

        private void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            audioShoot.Play(m_AudioSource);
            m_Collider = GetComponent<SphereCollider>();
            
            m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
            bulletLifeTime = m_ParticleSystem.main.duration;
            hasBulletLifeTime = bulletLifeTime > 0;
            m_StartTime = Time.time + bulletLifeTime;
            
        }

        private void FixedUpdate()
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

        private void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }

        private void AutoDestroy()
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
