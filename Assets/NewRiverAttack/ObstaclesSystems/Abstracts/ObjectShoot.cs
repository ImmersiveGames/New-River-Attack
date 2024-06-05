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
        private GameObject prefabBullet;

        [SerializeField] private int initialPoolSize = 10;
        public bool persistent;
        private string _poolName;

        [Header("Bullet Settings")] [SerializeField, Range(0.5f, 4f)]
        protected float bulletLifeTime = 2f;

        protected float CadenceShoot;
        private float _lastActionTime;

        protected BulletData BulletData;

        private Transform _poolRoot;
        private Transform _spawnPoint;
        private IPoolManager _poolManager;

        #region Unity Methods

        private void Awake()
        {
            _poolName = $"Pool ({gameObject.name})";
        }

        private void OnDisable()
        {
            _poolManager = null;
        }

        #endregion


        protected virtual void SetInitialReferences()
        {
            DebugManager.Log<ObjectShoot>($"Cria o POOL");
            _poolManager = null;
            _poolManager = new PoolObjectManager();
            _poolRoot = transform;
            _spawnPoint = _poolRoot;
            _poolManager.CreatePool(_poolName, prefabBullet, initialPoolSize, _poolRoot, persistent);
        }

        protected internal void AttemptShoot(ObjectMaster objectMaster)
        {
            if (!objectMaster.ObjectIsReady) return;
            var spawn = GetComponentInChildren<ShootSpawnPoint>().transform;
            _spawnPoint = transform;
            if (spawn != null && _spawnPoint != spawn)
            {
                _spawnPoint = spawn;
            }

            var cooldown = CadenceShoot;
            if (!IsOnCooldown(cooldown))
            {
                Fire();
                DebugManager.Log<ObjectShoot>($"Atira");
                _lastActionTime = Time.realtimeSinceStartup;
            }
            else
            {
                DebugManager.Log<ObjectShoot>($"Não pode Atirar");
            }
        }

        protected virtual void Fire()
        {
            var bullet = _poolManager.GetObjectFromPool<IPoolable>(_poolName, _spawnPoint, BulletData);
        }

        public abstract void SetDataBullet(ObjectMaster objectMaster);


        protected void MakeBullet(ObjectMaster obstacleMaster, float bulletSpeed, int damage, float lifetime, bool powerUp = false)
        {
            // Cria os dados da bala
            BulletData = new BulletData
            {
                BulletDamage = damage,
                BulletSpeed = bulletSpeed,
                BulletTimer = lifetime,
                BulletPowerUp = false,
                BulletOwner = obstacleMaster
            };
        }

        private bool IsOnCooldown(float cooldown)
        {
            return Time.realtimeSinceStartup - _lastActionTime < cooldown;
        }
    }
}