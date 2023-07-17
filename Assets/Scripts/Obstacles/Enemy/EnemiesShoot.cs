using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EnemiesShoot : ObstacleShoot, IHasPool
    {
        public int poolStart;
        public bool playerTarget;
    
        private EnemySkinParts m_SkinPart;
        Transform m_SpawnPosition;
        PoolObjectManager m_MyPool;

        Transform target { get;
            set; }
    
        protected override void OnEnable()
        {
            base.OnEnable();
            m_SkinPart = GetComponentInChildren<EnemySkinParts>();
            m_MyPool = PoolObjectManager.instance;
            if (!GetComponent<ObstacleDetectApproach>().enabled)
                InvokeRepeating(nameof(StartFire), 1, cadencyShoot);
        }
        private void Start()
        {
            SetTarget(GamePlayManager.instance.GetPlayer(0).transform);
            m_SpawnPosition = GetComponentInChildren<EnemyShootSpawn>().transform;
            StartMyPool();
        }
        new void Fire()
        {
            GameObject bullet = m_MyPool.GetObject(this);
            bullet.transform.position = m_SpawnPosition.position;
            bullet.transform.rotation = m_SpawnPosition.rotation;
            BulletEnemy enemyBullet = bullet.GetComponent<BulletEnemy>();
            enemyBullet.shootDirection = Vector3.forward;
            enemyBullet.shootVelocity = bulletSpeedy;
            if (playerTarget)
                enemyBullet.transform.LookAt(target);
        }

        new void SetTarget(Transform toTarget)
        {
            target = (playerTarget) ? toTarget : null;
        }
    
        private void StartFire()
        {
            if (ShouldFire())
            {
                Fire();
            }
        }
    
        private void FixedUpdate()
        {
            if (m_SkinPart != null && playerTarget)
            {
                var transform1 = m_SkinPart.transform;
                m_SkinPart.transform.rotation = Quaternion.Lerp(transform1.rotation, Quaternion.LookRotation(target.position - transform1.position), Time.deltaTime);
            }
        }
    
        public void StartMyPool(bool isPersistent = false)
        {
            m_MyPool.CreatePool(this, prefab, poolStart, m_SpawnPosition, isPersistent);
        }
    }

}

