using UnityEngine;
namespace RiverAttack
{
    public class BulletEnemy : Bullets
    {
        private float m_StartTime;

        #region UNITYMETHODS

        private void OnEnable()
        {
            GamePlayManager.instance.EventEnemiesMasterKillPlayer += DestroyMe;
            if (GamePlayManager.instance.playerDead) return;
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
            m_StartTime = Time.time + bulletLifeTime;
        }

        private void FixedUpdate()
        {
            MoveShoot();
            AutoDestroyMe(m_StartTime);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if ((collision.GetComponentInParent<EnemiesMaster>() && !collision.GetComponentInParent<CollectiblesMaster>()) ||
                collision.GetComponentInParent<BulletEnemy>()|| collision.GetComponentInParent<BulletBoss>()||
                collision.GetComponentInParent<BossMaster>()) return;
            
            if (collision.GetComponentInParent<WallsMaster>() ||
                collision.GetComponentInParent<EffectAreaMaster>()) return;
            
            DestroyMe();
        }

        private void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion

        private void MoveShoot()
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
        }
    }
}
