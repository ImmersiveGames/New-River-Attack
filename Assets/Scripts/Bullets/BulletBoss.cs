using UnityEngine;
using UnityEngine.Serialization;
namespace RiverAttack
{
    public class BulletBoss : Bullets
    {
        public Vector3 moveDirection;
        float lerp = 5.0f;
        
        float m_StartTime;

        #region UNITYMETHODS
        void OnEnable()
        {
            GamePlayManager.instance.EventEnemiesMasterKillPlayer += DestroyMe;
            if (GamePlayManager.instance.playerDead) return;
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
            m_StartTime = Time.time + bulletLifeTime;

        }
        void FixedUpdate()
        {
            MoveShoot(moveDirection);
            AutoDestroyMe(m_StartTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            if ((collision.GetComponentInParent<EnemiesMaster>() || collision.GetComponentInParent<BossMaster>() && !collision.GetComponentInParent<CollectiblesMaster>()) ||
                collision.GetComponentInParent<Bullets>()) return;
            
            if (collision.GetComponentInParent<WallsMaster>() ||
                collision.GetComponentInParent<EffectAreaMaster>()) return;
            
            DestroyMe();
        }
        void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion

        void MoveShoot(Vector3 directionVector3)
        {
            if (GamePlayManager.instance.shouldBePlayingGame)
            {
                //look at target.
                
                float speedy = bulletSpeed * Time.deltaTime;
                transform.Translate(Vector3.forward * speedy);
                moveDirection = directionVector3.normalized;
                Vector3 newDirection = transform.position + moveDirection * bulletSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, newDirection, lerp * Time.deltaTime);
            }
            else
            {
                DestroyMe();
            }
        }
        /*Vector3 moveDirection;
        float suavizacao = 5.0f;
        float m_StartTime;

        #region UNITYMETHODS
        void OnEnable()
        {
            //GamePlayManager.instance.EventEnemiesMasterKillPlayer += DestroyMe;
            //if (GamePlayManager.instance.playerDead) return;
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
            m_StartTime = Time.time + bulletLifeTime;

        }
        void FixedUpdate()
        {
            MoveShoot(moveDirection);
            //AutoDestroyMe(m_StartTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponentInParent<PlayerMaster>()) return;
            DestroyMe();
        }
        void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion
        public void MoveShoot(Vector3 directionVector3)
        {
            if (GamePlayManager.instance.shouldBePlayingGame)
            {
                /*float speedy = bulletSpeed * Time.deltaTime;
                transform.Translate(Vector3.forward * speedy);#1#
                moveDirection = directionVector3.normalized;
                Vector3 newDirection = transform.position + moveDirection * bulletSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, newDirection, suavizacao * Time.deltaTime);
                //look at target.
            }
            else
            {
                DestroyMe();
            }
        }*/
        
    }
}
