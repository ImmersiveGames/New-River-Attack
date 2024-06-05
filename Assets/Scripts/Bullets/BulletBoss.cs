using UnityEngine;
namespace RiverAttack
{
    public class BulletBoss : Bullets
    {
        private const float LERP = 5.0f;
        
        [SerializeField] private GameObject deadParticlePrefab;
        [SerializeField] private float timeoutDestroyExplosion;
        [SerializeField] private AudioEventSample enemyExplodeAudio;
        internal Vector3 moveDirection;

        private float m_StartTime;
        private AudioSource m_AudioSource;
        #region UNITYMETHODS

        private void OnEnable()
        {
            //GamePlayManager.instance.EventEnemiesMasterKillPlayer += DestroyMe;
            if (GamePlayManager.instance.playerDead) return;
            m_AudioSource = GetComponent<AudioSource>();
            audioShoot.Play(m_AudioSource);
            m_StartTime = Time.time + bulletLifeTime;
        }

        private void Start()
        {
            m_StartTime = Time.time + bulletLifeTime;
        }

        private void Update()
        {
            switch (GamePlayManager.instance.readyToFinish)
            {
                case true when gameObject.activeSelf:
                    DestroyMe();
                    return;
                case true:
                    return;
            }
            MoveShoot(moveDirection);
            if(m_StartTime > 0)
                AutoDestroyMe(m_StartTime);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponentInParent<PlayerMasterOld>() && !collision.GetComponentInParent<EffectAreaMasterOld>())
                return;
            DestroyMeExplosion();
        }

        private void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion

        private void MoveShoot(Vector3 directionVector3)
        {
            if (!GamePlayManager.instance.shouldBePlayingGame)
                return;
            moveDirection = directionVector3.normalized;
            var position = transform.position;
            var newDirection = position + moveDirection * (bulletSpeed * Time.deltaTime);
            position = Vector3.Lerp(position, newDirection, LERP * Time.deltaTime);
            transform.position = position;
        }

        private void DestroyMeExplosion()
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
