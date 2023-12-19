using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class BossMinesShoot: MonoBehaviour, IHasPool
    {

        [Header("Mines Settings")]
        [SerializeField] GameObject bulletMines;
        [SerializeField] int minesStartPool;
        
        internal Transform spawnPoint;
        void Start()
        {
            // setup inicial do status
            StartMyPool(bulletMines,minesStartPool);
            spawnPoint = GetComponentInChildren<EnemiesShootSpawn>().transform ? GetComponentInChildren<EnemiesShootSpawn>().transform : transform;
        }
        internal GameObject getBulletsMines
        {
            get { return bulletMines; }
        }

        public void StartMyPool(GameObject bullets, int quantity, bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullets, quantity, transform, isPersistent);
        }
    }
}
