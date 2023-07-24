using UnityEngine;

namespace RiverAttack
{
    public class BulletPlayer : Bullets
    {
         #region Variable Private Inspector
        Transform m_MyPool;
        #endregion

        #region UnityMethods
        void Awake()
        {
            ownerShoot = GetComponentInParent<PlayerMaster>();
            shootVelocity = ownerShoot.GetPlayersSettings().shootVelocity;
        }
        void OnEnable()
        {
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
        }
        void Start()
        {
            startTime = Time.time + lifeTime;
        }
        void FixedUpdate()
        {
            MoveShoot();
            AutoDestroy();
        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>() || collision.GetComponentInParent<Bullets>() ) return;
            var hitCollectable = (CollectibleScriptable)collision.transform.root.GetComponent<EnemiesMaster>().enemy;
            if (hitCollectable == null) return;
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
            if (bulletLifeTime && Time.time >= startTime)
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
