using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class BossShoot : MonoBehaviour, IHasPool
    {
        [SerializeField] GameObject bullet;
        [SerializeField] int startPool;
        [SerializeField] internal float bulletLifeTime;
        [SerializeField] internal float shootCadence;
        
        [Header("Bullet Settings")]
        [SerializeField]
        internal float bulletSpeed;
        
        BossMaster m_BossMaster;
        internal Transform spawnPoint;
        void OnEnable()
        {
            SetInitialReferences();
        }
        void Start()
        {
            // setup inicial do status
            StartMyPool();
            spawnPoint = GetComponentInChildren<EnemiesShootSpawn>().transform ? GetComponentInChildren<EnemiesShootSpawn>().transform : transform;
        }
        void SetInitialReferences()
        {
            m_BossMaster = GetComponent<BossMaster>();
        }

        internal GameObject GetBullets()
        {
            return bullet;
        }

        public void StartMyPool(bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullet, startPool, transform, isPersistent);
        }
    }
}
