using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace RiverAttack
{
    public class BulletBoss : Bullets
    {
        public Vector3 moveDirection;
        const float LERP = 5.0f;

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
                moveDirection = directionVector3.normalized;
                var position = transform.position;
                var newDirection = position + moveDirection * (bulletSpeed * Time.deltaTime);
                position = Vector3.Lerp(position, newDirection, LERP * Time.deltaTime);
                transform.position = position;
            }
            else
            {
                DestroyMe();
            }
        }
    }
}
