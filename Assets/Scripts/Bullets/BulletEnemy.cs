using UnityEngine;
namespace RiverAttack
{
    public class BulletEnemy : Bullets
    {
        /*#region Variable Private Inspector
        
        float m_StartTime;
        
        #endregion
        
        #region UnityMethods
        void OnEnable()
        {
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
            m_StartTime = Time.time + bulletLifeTime;
            
            GamePlayManager.instance.EventEnemyDestroyPlayer += DestroyMe;
        }
        void FixedUpdate()
        {
            MoveShoot();
            AutoDestroyMe(m_StartTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            if ((collision.GetComponentInParent<EnemiesMaster>() && !collision.GetComponentInParent<CollectiblesMaster>())|| collision.GetComponentInParent<BulletEnemy>() ) return;
            DestroyMe();
        }
        void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion
        
        void MoveShoot()
        {
            if (GamePlayManager.instance.shouldBePlayingGame)
            {
                float speedy = bulletSpeed * Time.deltaTime;
                transform.Translate(Vector3.forward * speedy);
                //look at target.
            }
            else
            {
                DestroyMe();
            }
        }*/
    }
}
