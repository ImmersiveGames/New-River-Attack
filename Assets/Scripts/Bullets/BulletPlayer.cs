using UnityEngine;

namespace RiverAttack
{
    public class BulletPlayer : Bullets
    {
         #region Variable Private Inspector
        float m_StartTime;
        #endregion

        #region UnityMethods
        void OnEnable()
        {
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
            m_StartTime = Time.time + bulletLifeTime;
        }
        void FixedUpdate()
        {
            MoveShoot();
            AutoDestroyMe(m_StartTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>() || collision.GetComponentInParent<BulletPlayer>() || collision.GetComponentInParent<BulletPlayerBomb>()) return;
            var hitCollectable = collision.transform.GetComponentInParent<ObstacleMaster>();
            Debug.Log($"Collider: {hitCollectable}");
            if (hitCollectable == null && hitCollectable.enemy is CollectibleScriptable) return;
            DestroyMe();
        }
        void OnBecameInvisible()
        {
            //DestroyMe();
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion
        
        
        void MoveShoot()
        {
            if (GamePlayManager.instance.shouldBePlayingGame)
            {
                float speedy = bulletSpeed * Time.deltaTime;
                transform.Translate(Vector3.forward * speedy);
            }
            else
            {
                DestroyMe();
            }
        }

        
        
    }
}
