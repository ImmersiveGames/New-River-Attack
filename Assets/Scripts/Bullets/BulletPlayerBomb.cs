using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class BulletPlayerBomb : Bullets
    {
        [SerializeField]
        ParticleSystem pSystem;
        [SerializeField]
        float radiusSize;
        [SerializeField]
        float radiusSpeed;
        [SerializeField]
        float shakeForce;
        [SerializeField]
        float shakeTime;
        [SerializeField]
        long millisecondsVibrate;
        float timeLife
        {
            get;
            set;
        }
        float m_EndLife;
        double m_TParam;
        Collider m_Collider;
        List<EnemiesMaster> m_CollisionEnemy = new List<EnemiesMaster>();

        #region UNITY METHODS
        void OnEnable()
        {
            timeLife = pSystem.main.duration;
            m_Collider = GetComponent<Collider>();
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
        }

        // Use this for initialization
        void Start()
        {
            m_EndLife = Time.time + timeLife;
        }
        void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponentInParent<EnemiesMaster>();
            Debug.Log(enemy);
            if (enemy && !m_CollisionEnemy.Contains(enemy))
            {
                m_CollisionEnemy.Add(enemy);
            }
        }
        void FixedUpdate()
        {
            ExpandCollider();
            AutoDestroy();
        }
  #endregion

        void AutoDestroy()
        {
            if (Time.time >= m_EndLife)
            {
                DestroyMe();
            }
        }
        void ExpandCollider()
        {
            m_TParam += Time.deltaTime * radiusSpeed;
            CameraShake.ShakeCamera(shakeForce, shakeTime);

//TODO: Arrumar nova forma de vibrar o celular
#if UNITY_ANDROID && !UNITY_EDITOR
            ToolsAndroid.Vibrate(millisecondsVibrate);
            Handheld.Vibrate();
#endif
            if (m_Collider.GetType() != typeof(SphereCollider))
                return;
            var sphere = (SphereCollider)m_Collider;
            sphere.radius = Mathf.Lerp(0.5f, radiusSize, (float)m_TParam);
        }
        new void DestroyMe()
        {
            //Debug.Log(m_CollisionEnemy.Count);
            if (m_CollisionEnemy.Count >= 3)
            {
                GameSteamManager.UnlockAchievement("ACH_HIT_BOMB_3");
            }
            GameObject o;
            (o = gameObject).SetActive(false);
            Destroy(o);
            
        }
    }
}
