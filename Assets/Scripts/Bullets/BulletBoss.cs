using UnityEngine;
namespace RiverAttack
{
    public class BulletBoss : Bullets
    {
        const float LERP = 5.0f;
        
        [SerializeField] GameObject deadParticlePrefab;
        [SerializeField] float timeoutDestroyExplosion;
        [SerializeField] AudioEventSample enemyExplodeAudio;
        internal Vector3 moveDirection;
        
        float m_StartTime;
        AudioSource m_AudioSource;
        #region UNITYMETHODS
        void OnEnable()
        {
            //GamePlayManager.instance.EventEnemiesMasterKillPlayer += DestroyMe;
            if (GamePlayManager.instance.playerDead) return;
            m_AudioSource = GetComponent<AudioSource>();
            audioShoot.Play(m_AudioSource);
            m_StartTime = Time.time + bulletLifeTime;
        }
        void Start()
        {
            m_StartTime = Time.time + bulletLifeTime;
        }
        void Update()
        {
            MoveShoot(moveDirection);
            if(m_StartTime > 0)
                AutoDestroyMe(m_StartTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponentInParent<PlayerMaster>() && !collision.GetComponentInParent<EffectAreaMaster>())
                return;
            DestroyMeExplosion();
        }
        void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion
        void MoveShoot(Vector3 directionVector3)
        {
            if (!GamePlayManager.instance.shouldBePlayingGame)
                return;
            moveDirection = directionVector3.normalized;
            var position = transform.position;
            var newDirection = position + moveDirection * (bulletSpeed * Time.deltaTime);
            position = Vector3.Lerp(position, newDirection, LERP * Time.deltaTime);
            transform.position = position;
        }
        void DestroyMeExplosion()
        {
            var transform1 = transform;
            if (m_AudioSource != null && enemyExplodeAudio != null)
                enemyExplodeAudio.Play(m_AudioSource);

            if (deadParticlePrefab)
            {
                var explosion = Instantiate(deadParticlePrefab,transform1.position, transform1.rotation);
                Destroy(explosion, timeoutDestroyExplosion);
            }
            DestroyMe();
        }

    }
}
