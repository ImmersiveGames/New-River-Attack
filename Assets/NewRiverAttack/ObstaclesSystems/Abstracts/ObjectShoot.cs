using UnityEngine;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers.Interfaces;
using NewRiverAttack.PlayerManagers.Tags;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObjectShoot : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] private GameObject prefabBullet;
        [SerializeField] private int initialPoolSize = 10;
        [SerializeField] private bool persistent;
        [SerializeField] private string poolName;

        protected PoolingHelper PoolHelper { get; private set; }
        private IShootPattern ShootPattern { get; set; }
        public Transform SpawnPoint { get; set; }

        private ObstacleMaster _obstacleMaster;

        protected virtual void Awake()
        {
            PoolHelper = new PoolingHelper(prefabBullet, transform, poolName, initialPoolSize, persistent);
            _obstacleMaster = GetComponent<ObstacleMaster>();
            UpdateSpawnPoint();
        }

        private void OnEnable()
        {
            _obstacleMaster.EventObstacleChangeSkin += UpdateSpawnPoint;
        }

        private void OnDisable()
        {
            _obstacleMaster.EventObstacleChangeSkin -= UpdateSpawnPoint;
        }

        protected void UpdateSpawnPoint()
        {
            var shootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
            SpawnPoint = shootSpawnPoint != null ? shootSpawnPoint.transform : transform;
        }

        protected void SetShootPattern(IShootPattern pattern)
        {
            ShootPattern = pattern;
        }

        public void ExecuteShootPattern()
        {
            ShootPattern?.Execute(SpawnPoint, this);
        }

        public void Fire(BulletSpawnData bulletData, Transform spawnPoint)
        {
            Debug.Log("Disparando projétil com direção: " + bulletData.Direction);
            PoolHelper.GetObject(spawnPoint, bulletData);
            /*if (obj == null)
            {
                Debug.LogError("Nenhum objeto foi recuperado do pool!");
            }*/
        }

        public virtual void ResetShoot()
        {
            
        }

        public abstract float GetCadenceShoot { get; }
        public abstract BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position);
    }
}
