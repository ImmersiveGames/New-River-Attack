using ImmersiveGames.DebugManagers;
using ImmersiveGames.PoolSystems;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObjectShoot : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] private GameObject prefabBullet;
        [SerializeField] private int initialPoolSize = 10;
        [SerializeField] private bool persistent;
        [SerializeField] private string poolName;

        private float _lastActionTime;
        protected Transform SpawnPoint { get; set; }
        private IPoolManager _poolManager;
        private PoolObject _bulletPool; // Agora armazena o PoolObject
        private GamePlayManager _gamePlayManagerRef;

        #region Unity Methods

        protected virtual void Awake()
        {
            if (string.IsNullOrEmpty(poolName))
                poolName = $"Pool ({gameObject.name})";
            SetInitialReferences();
        }

        protected virtual void OnEnable()
        {
            _gamePlayManagerRef.EventGameReload += ResetShoot;
            _gamePlayManagerRef.EventGameRestart += ResetShoot;
        }

        protected virtual void OnDisable()
        {
            _gamePlayManagerRef.EventGameReload -= ResetShoot;
            _gamePlayManagerRef.EventGameRestart -= ResetShoot;
        }

        #endregion

        // Configura referências iniciais e cria o pool de objetos
        protected virtual void SetInitialReferences()
        {
            _gamePlayManagerRef = GamePlayManager.Instance;
            _poolManager = new PoolObjectManager();

            // Obtém o PoolObject diretamente
            _poolManager.CreatePool(poolName, prefabBullet, initialPoolSize, transform, persistent);
            _bulletPool = _poolManager.GetPool(poolName);
        }

        // Método que tenta disparar respeitando o cooldown
        internal virtual void AttemptShoot(ObjectMaster objectMaster)
        {
            if (!objectMaster.ObjectIsReady) return;

            if (!IsOnCooldown(GetCadenceShoot()))
            {
                var bulletData = CreateBulletData(SpawnPoint.forward, SpawnPoint.position);
                Fire(bulletData, SpawnPoint);
                _lastActionTime = Time.realtimeSinceStartup;
            }
            else
            {
                DebugManager.Log<ObjectShoot>($"Cooldown ativo. Não pode atirar.");
            }
        }

        // Método para disparar a bala
        protected virtual void Fire(BulletSpawnData bulletData, Transform spawnPoint)
        {
            _bulletPool.GetObject(spawnPoint, bulletData); // Usa o PoolObject para obter o objeto
        }

        // Método que verifica se o disparo está em cooldown
        private bool IsOnCooldown(float cooldown)
        {
            return Time.realtimeSinceStartup - _lastActionTime < cooldown;
        }

        protected void ReturnMarkedObjects()
        {
            _bulletPool.ReturnMarkedObjects();
        }

        // Método que reseta todos os objetos ativos e retorna ao pool
        protected void ResetSpawnedObjects()
        {
            _bulletPool.ReturnAllActiveObjects(); // Retorna todos os objetos ativos
        }

        // Método que marca um objeto para retorno ao pool posteriormente
        public void MarkForReturn(GameObject obj)
        {
            _bulletPool.MarkForReturn(obj); // Marca o objeto
        }

        // Função abstrata para cadência de tiro
        protected abstract float GetCadenceShoot();

        // Método abstrato para criar dados da bala (definido nas classes derivadas)
        protected abstract BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position);

        // Função para resetar o tiro
        public abstract void ResetShoot();

        // Função para obter um alvo fixo, sempre mira no jogador
        protected Transform AlwaysTarget()
        {
            var master = _gamePlayManagerRef.GetPlayerMaster(0);
            return master ? master.transform : null;
        }
    }
}
