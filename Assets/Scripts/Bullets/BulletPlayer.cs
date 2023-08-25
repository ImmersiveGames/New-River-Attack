using UnityEngine;

namespace RiverAttack
{
    public class BulletPlayer : Bullets
    {
        GamePlayManager m_GamePlayManager;
        #region UnityMethods
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
            TransformSpawnPosition(transform.root.GetComponentInChildren<PlayerSkinAttach>().transform);
        }
        void FixedUpdate()
        {
            MoveShoot();
            AutoDestroyMe(bulletLifeTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>() || collision.GetComponentInParent<BulletPlayer>() || collision.GetComponentInParent<BulletPlayerBomb>()) return;
            var obstacleMaster = collision.GetComponent<ObstacleMaster>();
            if (obstacleMaster != null && !obstacleMaster.enemy.canDestruct) return;
            if (collision.GetComponentInParent<PowerUpMaster>()) return;
            DestroyMe();
        }
        void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion

        void MoveShoot()
        {
            if (m_GamePlayManager.shouldBePlayingGame)
            {
                float speedy = bulletSpeed * Time.deltaTime;
                transform.Translate(Vector3.forward * speedy);
            }
            else
            {
                DestroyMe();
            }
        }
        void TransformSpawnPosition(Transform spawnTransform)
        {
            var myTransform = transform;
            var position = spawnTransform.position;
            myTransform.position = new Vector3(position.x, position.y, position.z);
            var rotation = spawnTransform.rotation;
            myTransform.rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
        }
    }
}
