using ImmersiveGames.AudioEvents;
using ImmersiveGames.PoolSystems.Interfaces;
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
        [SerializeField]
        protected GameObject prefabBullet;
        [SerializeField] protected AudioEvent audioShoot;
        [SerializeField] protected int initialPoolSize = 10;
        [SerializeField] protected bool persistent;
        [SerializeField] protected string poolName;
        
        public Transform SpawnPoint { get; private set; }
        protected PoolingHelper PoolHelper { get; private set; }
        private IShootPattern ShootPattern { get; set; }
        private AudioSource _audioSource;
        private ObstacleMaster _obstacleMaster;
        protected virtual void Awake()
        {
            _obstacleMaster = GetComponent<ObstacleMaster>();
            _audioSource = GetComponent<AudioSource>();
            PoolHelper = new PoolingHelper(prefabBullet, transform, poolName, initialPoolSize, persistent);
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
        public void PoolingOut(Transform spawnPoint,ISpawnData bulletData)
        {
            PoolHelper.GetObject(spawnPoint, bulletData);
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

        protected void ExecuteShootPattern()
        {
            ShootPattern?.Execute(SpawnPoint, this);
        }
        public void ShootSound()
        {
            if (_audioSource == null || audioShoot == null) return;
            audioShoot.SimplePlay(_audioSource);
        }
        public abstract float GetCadenceShoot { get; }
        public abstract BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position);
    }
}
