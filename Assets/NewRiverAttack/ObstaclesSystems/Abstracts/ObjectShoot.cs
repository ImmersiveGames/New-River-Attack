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
        public Transform SpawnPoint { get; set; }
        private IPoolManager _poolManager;
        private GamePlayManager _gamePlayManagerRef;

        #region Unity Methods

        protected virtual void Awake()
        {
            if (string.IsNullOrEmpty(poolName))
                poolName = $"Pool ({gameObject.name})";
            SetInitialReferences();
        }

        protected virtual  void OnEnable()
        {
            _gamePlayManagerRef.EventGameReload += ResetShoot;
            _gamePlayManagerRef.EventGameRestart += ResetShoot;
        }

        protected virtual void OnDisable()
        {
            _poolManager = null;  // Limpa referência quando desativado
            _gamePlayManagerRef.EventGameReload -= ResetShoot;
            _gamePlayManagerRef.EventGameRestart -= ResetShoot;
        }

        #endregion

        // Método para configurar as referências e criar o pool de objetos
        protected virtual void SetInitialReferences()
        {
            DebugManager.Log<ObjectShoot>($"Criando Pool: {poolName}");
            _gamePlayManagerRef = GamePlayManager.Instance;
            _poolManager = new PoolObjectManager();
            _poolManager.CreatePool(poolName, prefabBullet, initialPoolSize, transform, persistent);
        }

        // Método para tentar disparar, respeitando o cooldown
        internal virtual void AttemptShoot(ObjectMaster objectMaster)
        {
            if (!objectMaster.ObjectIsReady) return;
            
            if (!IsOnCooldown(GetCadenceShoot()))
            {
                var bulletData = CreateBulletData(SpawnPoint.forward);
                Fire(bulletData);
                _lastActionTime = Time.realtimeSinceStartup;
            }
            else
            {
                DebugManager.Log<ObjectShoot>($"Cooldown ativo. Não pode atirar.");
            }
        }

        // Método que dispara a bala (configuração de dados fica na classe concreta)
        protected virtual void Fire(BulletSpawnData bulletData)
        {
            _poolManager.GetObjectFromPool(poolName, SpawnPoint, bulletData);
        }

        // Verifica se o disparo está em cooldown
        private bool IsOnCooldown(float cooldown)
        {
            return Time.realtimeSinceStartup - _lastActionTime < cooldown;
        }

        // Método abstrato para a cadência de tiro (definido na classe concreta)
        protected abstract float GetCadenceShoot();

        // Método abstrato para criar os dados da bala (definido na classe concreta)
        protected abstract BulletSpawnData CreateBulletData(Vector3 direction);

        public abstract void ResetShoot();

        protected Transform AlwaysTarget()
        {
            var master = _gamePlayManagerRef.GetPlayerMaster(0);
            return master ? master.transform : null;
        }
    }
}
