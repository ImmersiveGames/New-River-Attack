using UnityEngine;

namespace RiverAttack
{
    public class BulletPlayer : Bullets
    {
         #region Variable Private Inspector
        [SerializeField] bool bulletLifeTime = false;
        [SerializeField] float lifeTime = 2f;
        float m_StartTime;
        public PlayerMaster ownerShoot;
        Transform m_MyPool;
        #endregion

        #region UnityMethods
        void Awake()
        {
            ownerShoot = GetComponentInParent<PlayerMaster>();
            shootVelocity = ownerShoot.PlayersSettings().shootVelocity;
        }
        private void OnEnable()
        {
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
        }
        void Start()
        {
            m_StartTime = Time.time + lifeTime;
        }
        void FixedUpdate()
        {
            MoveShoot();
            AutoDestroy();
        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>()) return;
            var hitCollectable = (CollectibleScriptable)collision.transform.root.GetComponent<EnemiesMaster>().enemy;
            if (hitCollectable) return;
            DestroyMe();
        }
        void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .1f);
        }
        #endregion

        public void SetSpeedShoot(double speedy)
        {
            shootVelocity = (float)speedy;
        }
        void MoveShoot()
        {
            if (GamePlayManager.instance.shouldBePlayingGame)
            {
                float speedy = shootVelocity * Time.deltaTime;
                transform.Translate(Vector3.forward * speedy);
            }
            else
            {
                DestroyMe();
            }
        }

        void AutoDestroy()
        {
            if (bulletLifeTime && Time.time >= m_StartTime)
            {
                DestroyMe();
            }
        }
        void DestroyMe()
        {
            //Destroy(this.gameObject);

            gameObject.SetActive(false);
            gameObject.transform.SetParent(m_MyPool);
            gameObject.transform.SetAsLastSibling();
        }
    }
}
