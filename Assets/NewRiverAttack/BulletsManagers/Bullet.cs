using ImmersiveGames.PoolSystems;
using UnityEngine;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.GamePlayManagers;

namespace NewRiverAttack.BulletsManagers
{
    public abstract class Bullet : MonoBehaviour, IPoolable
    {
        protected ISpawnData SpawnData;
        protected float Lifetime;
        private GamePlayManager _gamePlayManager;

        protected bool IsInitialize { get; private set; }

        protected virtual void OnEnable()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _gamePlayManager.EventGameResetClear += ReturnToPool;
            _gamePlayManager.EventGameOver += ReturnToPool;
            _gamePlayManager.EventGameFinisher += ReturnToPool;
        }

        private void OnDisable()
        {
            _gamePlayManager.EventGameResetClear -= ReturnToPool;
            _gamePlayManager.EventGameOver -= ReturnToPool;
            _gamePlayManager.EventGameFinisher -= ReturnToPool;
            IsInitialize = false;
        }

        public ISpawnData GetSpawnData => SpawnData;

        #region IPoolable

        public PoolObject Pool { get; set; } // Referência ao pool

        // Método chamado quando o projétil é instanciado do pool
        public virtual void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            SpawnData = data;
            if (SpawnData != null)
            {
                Lifetime = SpawnData.Timer;
            }

            transform.position = spawnPosition.position;
            transform.rotation = spawnPosition.rotation;

            IsInitialize = true;
        }

        // Método chamado quando o projétil é retornado ao pool
        public virtual void OnReturnedToPool()
        {
            IsInitialize = false; // Certifica-se de que a lógica de movimento e qualquer estado seja resetado
            Lifetime = 0;         // Reseta o tempo de vida da bala
            SpawnData = null;    // Limpa os dados do projétil
        }

        #endregion

        // Método para retornar o projétil ao pool
        protected void ReturnToPool()
        {
            IsInitialize = false; // Desativa a lógica de movimento
            Pool?.ReturnObject(gameObject); // Retorna ao pool
        }
    }
}