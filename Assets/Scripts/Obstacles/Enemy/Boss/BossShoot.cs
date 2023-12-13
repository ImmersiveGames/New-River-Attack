using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class BossShoot : ObstacleDetectApproach, IHasPool
    {
        [SerializeField] GameObject bullet;
        [SerializeField] int startPool;
        
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
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
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
