using System;
using UnityEngine;
namespace RiverAttack
{
    public class BulletEnemy : Bullets
    {
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
            MoveShoot();
            AutoDestroyMe(m_StartTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            if ((collision.GetComponentInParent<EnemiesMaster>() && !collision.GetComponentInParent<CollectiblesMaster>()) ||
                collision.GetComponentInParent<BulletEnemy>()) return;
            
            if (collision.GetComponentInParent<WallsMaster>() ||
                collision.GetComponentInParent<EffectAreaMaster>()) return;
            
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
        }
    }
}
