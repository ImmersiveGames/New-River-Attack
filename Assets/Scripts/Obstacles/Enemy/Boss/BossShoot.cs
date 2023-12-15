using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class BossShoot : MonoBehaviour, IHasPool
    {
        [SerializeField] GameObject bullet;
        [SerializeField] int startPool;

        [Header("Bullet Settings")]
        
        internal Transform spawnPoint;
        void Start()
        {
            // setup inicial do status
            StartMyPool(startPool);
            spawnPoint = GetComponentInChildren<EnemiesShootSpawn>().transform ? GetComponentInChildren<EnemiesShootSpawn>().transform : transform;
        }

        internal GameObject getBullets
        {
            get { return bullet; }
        }

        public void StartMyPool(int quantity, bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullet, quantity, transform, isPersistent);
        }
    }
}
