using UnityEngine;
using Utils;
namespace RiverAttack
{
    [RequireComponent(typeof(Collider), typeof(AudioSource))]
    public class BulletEnemy : Bullets, IPoolable
    {
        [HideInInspector]
        public Vector3 shootDirection;
        Collider m_Collider;
        AudioSource m_AudioSource;
#region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            audioShoot.Play(m_AudioSource);
            m_Collider.enabled = true;
            Invoke(nameof(DisableShoot), lifeTime);
        }
        void FixedUpdate()
        {
            float speedy = shootVelocity * Time.deltaTime;
            transform.Translate(shootDirection * speedy);
        }
        void OnBecameInvisible()
        {
            DisableShoot();
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMaster>() == null && !other.GetComponent<BulletPlayer>())
                return;
            gameObject.SetActive(false);
            m_Collider.enabled = false;
        }
  #endregion
        void SetInitialReferences()
        {
            m_Collider = GetComponent<Collider>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        void DisableShoot()
        {
            gameObject.SetActive(false);
            m_Collider.enabled = false;
        }
        
    }
}
