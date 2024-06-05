using NewRiverAttack.LevelBuilder;
using UnityEngine;

namespace RiverAttack
{
    public class BulletPlayer : Bullets
    {
        private GamePlayManager m_GamePlayManager;
        private float m_StartTime;
        #region UnityMethods

        private void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
            var root = transform.root;
            var spawnPos = root.GetComponentInChildren<PlayerSkinAttach>() ?? root.GetComponent<PlayerSkinAttach>();
            if(spawnPos) 
                TransformSpawnPosition(spawnPos.transform);
            m_StartTime = Time.time + bulletLifeTime;
        }

        private void FixedUpdate()
        {
            MoveShoot();
            AutoDestroyMe(m_StartTime);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMasterOld>() || collision.GetComponentInParent<BulletPlayer>() || 
                collision.GetComponentInParent<BulletPlayerBomb>() || collision.GetComponent<LevelChangeBGM>()) return;
            var obstacleMaster = collision.GetComponent<ObstacleMasterOld>();
            if (obstacleMaster != null && !obstacleMaster.enemy.canDestruct) return;
            if (collision.GetComponentInParent<PowerUpMasterOld>()) return;
            DestroyMe();
        }

        private void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }

        private void OnDisable()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion

        private void MoveShoot()
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

        private void TransformSpawnPosition(Transform spawnTransform)
        {
            var myTransform = transform;
            var position = spawnTransform.position;
            myTransform.position = new Vector3(position.x, position.y, position.z);
            var rotation = spawnTransform.rotation;
            myTransform.rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
        }

    }
}
