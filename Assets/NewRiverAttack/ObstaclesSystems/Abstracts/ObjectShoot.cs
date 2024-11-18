using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers;
using UnityEngine;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.PoolSystems.Interfaces;
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
        [SerializeField]
        protected bool enableTargeting;  // Habilita ou desabilita a mira no alvo
        [SerializeField] public Transform target;      // Referência ao alvo, se existir

        //private TargetingSystem _targetingSystem;
        private Transform SpawnPoint { get; set; }
        public PoolingHelper PoolHelper;
        protected virtual void Awake()
        {
            if(string.IsNullOrEmpty(poolName))
                poolName = $"Pool ({gameObject.name})";
            // Inicializa o PoolingHelper e configura o ponto de spawn
            PoolHelper = new PoolingHelper(prefabBullet, transform, poolName, initialPoolSize, persistent);
            _audioSource = GetComponent<AudioSource>();
            UpdateSpawnPoint();
            //_targetingSystem = new TargetingSystem();
        }
        

        // Método para obter e ativar o projétil no SpawnPoint
        public void PoolingOut(Transform spawnPoint, ISpawnData bulletData)
        {
            PoolHelper.GetObject(spawnPoint, bulletData);
        }

        // Método para executar o padrão de tiro, ajustando a mira no alvo se habilitado
        public void ExecuteShootPattern()
        {
            if (enableTargeting && target != null)
            {
                TargetingSystem.AimAtTarget(SpawnPoint, target);
            }
            // Executa o padrão de tiro
            if (shootPattern == null || SpawnPoint == null) return;
            Debug.Log(shootPattern, SpawnPoint);
            shootPattern.Execute(SpawnPoint, this);
        }

        // Método para emitir o som de tiro
        public void ShootSound()
        {
            if (_audioSource == null || audioShoot == null) return;
            audioShoot.SimplePlay(_audioSource);
        }

        // Método abstrato para criar os dados do projétil
        public abstract BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position);

        
        protected void UpdateSpawnPoint()
        {
            var shootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
            SpawnPoint = shootSpawnPoint != null ? shootSpawnPoint.transform : transform;
        }
        // Define o alvo para mira
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        // Ativa ou desativa a mira no alvo
        public void EnableTargeting(bool enable)
        {
            enableTargeting = enable;
        }
        
        
    }
}
