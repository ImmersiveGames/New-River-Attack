using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.PoolManagers;
using ImmersiveGames.PoolManagers.Interface;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObjectShoot : MonoBehaviour
    {
        [Header("Pool Settings")] [SerializeField]
        public GameObject prefabBullet;

        [SerializeField] private int initialPoolSize = 10;
        public bool persistent;
        public bool lockAtTarget;
        
        public string poolName;

        [Header("Bullet Settings")] [SerializeField, Range(0.5f, 4f)]
        protected float bulletLifeTime = 2f;

        protected float CadenceShoot;
        protected float LastActionTime;

        protected BulletData BulletData;

        private Transform _poolRoot;
        protected Transform SpawnPoint;
        protected ShootSpawnPoint ShootSpawnPoint;
        protected IPoolManager PoolManager;

        protected GameObject Bullet;
        private Transform _target;
        

        #region Unity Methods

        private void Awake()
        {
            if(string.IsNullOrEmpty(poolName))
                poolName = $"Pool ({gameObject.name})";
        }

        private void OnDisable()
        {
            PoolManager = null;
        }

        #endregion
        
        protected virtual void SetInitialReferences()
        {
            DebugManager.Log<ObjectShoot>($"Cria o POOL");
            PoolManager = null;
            PoolManager = new PoolObjectManager();
            _poolRoot = transform;
            PoolManager.CreatePool(poolName, prefabBullet, initialPoolSize, _poolRoot, persistent);
        }

        internal virtual void AttemptShoot(ObjectMaster objectMaster, Transform target = null)
        {
            _target = target;
            if (!objectMaster.ObjectIsReady) return;
            var spawn = ShootSpawnPoint.transform;
            SpawnPoint = transform;
            if (spawn != null && SpawnPoint != spawn)
            {
                SpawnPoint = spawn;
                if (lockAtTarget && _target != null)
                {
                    PointTowardsTarget(SpawnPoint, _target.transform.position);
                }
            }

            var cooldown = CadenceShoot;
            if (!IsOnCooldown(cooldown))
            {
                Fire();
                DebugManager.Log<ObjectShoot>($"Atira");
                LastActionTime = Time.realtimeSinceStartup;
            }
            else
            {
                DebugManager.Log<ObjectShoot>($"Não pode Atirar");
            }
        }

        protected virtual void Fire()
        {
            Bullet = PoolManager.GetObjectFromPool<IPoolable>(poolName, SpawnPoint, BulletData);
        }

        private static void PointTowardsTarget(Transform spawnPoint, Vector3 target)
        {
            // Calcula a direção do vetor entre o spawn point e o alvo
            var directionToTarget = target - spawnPoint.position;

            // Altera a rotação do spawn point para apontar para o alvo
            spawnPoint.rotation = Quaternion.LookRotation(directionToTarget);
        }
        
        public abstract void SetDataBullet(ObjectMaster objectMaster);


        protected void MakeBullet(ObjectMaster obstacleMaster, float bulletSpeed, int damage, float lifetime , Vector3 direction, bool powerUp = false)
        {
            // Cria os dados da bala
            BulletData = new BulletData
            {
                BulletDamage = damage,
                BulletSpeed = bulletSpeed,
                BulletTimer = lifetime,
                BulletDirection = direction,
                BulletPowerUp = false,
                BulletOwner = obstacleMaster
            };
        }

        protected bool IsOnCooldown(float cooldown)
        {
            return Time.realtimeSinceStartup - LastActionTime < cooldown;
        }
    }
}