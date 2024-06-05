using System.Collections.Generic;
using NewRiverAttack.SteamGameManagers;
using UnityEngine;

namespace RiverAttack
{
    public class BulletPlayerBomb : Bullets
    {
        [SerializeField] private ParticleSystem pSystem;
        [SerializeField] private float radiusSize;
        [SerializeField] private float radiusSpeed;
        [SerializeField] private float shakeForce;
        [SerializeField] private float shakeTime;
        [SerializeField] private long millisecondsVibrate;

        private float timeLife
        {
            get;
            set;
        }

        private float m_EndLife;
        private double m_TParam;
        private Collider m_Collider;
        private readonly List<EnemiesMasterOld> m_CollisionEnemy = new List<EnemiesMasterOld>();

        #region UNITY METHODS

        private void OnEnable()
        {
            timeLife = pSystem.main.duration;
            m_Collider = GetComponent<Collider>();
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
        }

        // Use this for initialization
        private void Start()
        {
            m_EndLife = Time.time + timeLife;
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponentInParent<EnemiesMasterOld>();
            if (enemy && !m_CollisionEnemy.Contains(enemy))
            {
                m_CollisionEnemy.Add(enemy);
            }
        }

        private void FixedUpdate()
        {
            ExpandCollider();
            AutoDestroy();
        }
  #endregion

  private void AutoDestroy()
        {
            if (Time.time >= m_EndLife)
            {
                DestroyMe();
            }
        }

        private void ExpandCollider()
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

        private new void DestroyMe()
        {
            //Debug.Log(m_CollisionEnemy.Count);
            if (m_CollisionEnemy.Count >= 3)
            {
                SteamGameManager.UnlockAchievement("ACH_HIT_BOMB_3");
            }
            GameObject o;
            (o = gameObject).SetActive(false);
            Destroy(o);
        }
    }
}
