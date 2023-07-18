using UnityEngine;
using Utils;
namespace RiverAttack
{
    [RequireComponent(typeof(Collider), typeof(AudioSource))]
    public class BulletEnemy : Bullets, IPoolable
    {
        [HideInInspector]
        public Vector3 shootDirection;
        [SerializeField]
        private float timeToDestroy;

        private GameSettings m_GameSettings;
        private Collider m_Collider;
        private AudioSource m_AudioSource;

        private void OnEnable()
        {
            SetInitialReferences();
            audioShoot.Play(m_AudioSource);
            m_Collider.enabled = true;
            Invoke(nameof(DisableShoot), timeToDestroy);
        }

        private void SetInitialReferences()
        {
            m_GameSettings = GameSettings.instance;
            m_Collider = GetComponent<Collider>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            float speedy = shootVelocity * Time.deltaTime;
            transform.Translate(shootDirection * speedy);
        }

        private void OnBecameInvisible()
        {
            DisableShoot();
        }
        private void DisableShoot()
        {
            gameObject.SetActive(false);
            m_Collider.enabled = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerMaster>() && !other.GetComponent<BulletPlayer>())
                return;
            gameObject.SetActive(false);
            m_Collider.enabled = false;
        }
    }
}
