using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class BossMissileShoot : MonoBehaviour, IHasPool
    {
        
        [Header("Missile Settings")]
        [SerializeField] GameObject bulletMissile;
        [SerializeField] internal int[] numMissiles;
        [SerializeField] internal float[] angleCones;
        [SerializeField] int missileStartPool;

        internal Transform spawnPoint;
        void Start()
        {
            numMissiles ??= new[] { 5 };
            angleCones ??= new[] { 90f};
            // setup inicial do status
            StartMyPool(bulletMissile, missileStartPool);
            spawnPoint = GetComponentInChildren<EnemiesShootSpawn>().transform ? GetComponentInChildren<EnemiesShootSpawn>().transform : transform;
        }

        internal GameObject getBulletsMissile
        {
            get { return bulletMissile; }
        }
        public void StartMyPool(GameObject bullets, int quantity, bool isPersistent = false)
        {
            PoolObjectManager.CreatePool(this, bullets, quantity, transform, isPersistent);
        }
    }
}
