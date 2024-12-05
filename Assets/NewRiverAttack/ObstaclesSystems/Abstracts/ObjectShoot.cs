using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers;
using UnityEngine;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.PlayerManagers.Tags;
using NewRiverAttack.ShootSystems;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObjectShoot : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] protected GameObject prefabBullet;
        [SerializeField] protected string poolName;
        [SerializeField] protected int initialPoolSize = 10;
        [SerializeField] protected bool persistent;

        [Header("Audio Settings")]
        [SerializeField] protected AudioEvent audioShoot;
        private AudioSource _audioSource;

        [Header("Shooting Pattern")]
        [SerializeField] protected ShootPatternBase shootPattern;

        [Header("Targeting Settings")]
        [SerializeField] protected bool enableTargeting; // Habilita ou desabilita a mira no alvo
        [SerializeField] public Transform target;      // Referência ao alvo, se existir

        [Header("Cooldown Settings")]
        [SerializeField]
        protected float cooldown = 1.0f; // Cooldown padrão

        protected float LastShootTime = -Mathf.Infinity;

        protected Transform SpawnPoint { get; private set; }
        private PoolingHelper _poolHelper;

        public float Cooldown
        {
            get => cooldown;
            set
            {
                if (value >= 0)
                    cooldown = value;
                else
                    Debug.LogWarning("Cooldown não pode ser negativo.");
            }
        }

        protected virtual void Awake()
        {
            if (string.IsNullOrEmpty(poolName))
                poolName = $"Pool ({gameObject.name})";

            _poolHelper = new PoolingHelper(prefabBullet, transform, poolName, initialPoolSize, persistent);
            _audioSource = GetComponent<AudioSource>();
            UpdateSpawnPoint();
        }
        
        

        public void ExecuteShootPattern()
        {
            if (Time.realtimeSinceStartup < LastShootTime + cooldown) return;
            if (enableTargeting && target != null)
            {
                TargetingSystem.AimAtTarget(SpawnPoint, target);
            }

            if (shootPattern == null || SpawnPoint == null) return;
            shootPattern.Execute(SpawnPoint, this);
            GameStatisticManager.instance.LogShoots(this);
            LastShootTime = Time.realtimeSinceStartup; // Atualiza o cooldown
        }

        public void ShootSound()
        {
            if (_audioSource == null || audioShoot == null) return;
            audioShoot.SimplePlay(_audioSource);
        }

        public void PoolingOut(Transform spawnPoint, ISpawnData bulletData)
        {
            _poolHelper.GetObject(spawnPoint, bulletData);
        }

        public abstract ISpawnData CreateBulletData(Vector3 direction, Vector3 position);

        protected void UpdateSpawnPoint()
        {
            var shootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
            SpawnPoint = shootSpawnPoint != null ? shootSpawnPoint.transform : transform;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void EnableTargeting(bool enable)
        {
            enableTargeting = enable;
        }
    }
}
