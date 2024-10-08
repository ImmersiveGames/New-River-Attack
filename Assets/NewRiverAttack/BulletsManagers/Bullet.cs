using ImmersiveGames.PoolSystems;
using UnityEngine;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;

namespace NewRiverAttack.BulletsManagers
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        protected Vector3 Direction;
        private Vector3 _position;
        protected float Speed;
        private float _lifetime;
        protected float LifeTimer;
        protected bool IsInitialized;
        private ObjectMaster _owner;
        private int _damage;
        private bool _powerUp;

        public PoolObject Pool { get; set; } // Referência ao pool

        // Método chamado quando o projétil é instanciado do pool
        public void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            // Inicializa os dados da bala a partir do BulletSpawnData
            if (data is BulletSpawnData bulletData)
            {
                Direction = bulletData.Direction;
                Speed = bulletData.Speed;
                _lifetime = bulletData.Timer; // Usamos "Timer" como tempo de vida
                _owner = bulletData.Owner;
                _position = bulletData.Position;
                _damage = bulletData.Damage;
                _powerUp = bulletData.PowerUp;
            }

            LifeTimer = _lifetime;

            transform.position = spawnPosition.position;
            transform.rotation = spawnPosition.rotation;

            IsInitialized = true;
        }

        // Método para retornar o projétil ao pool
        protected void ReturnToPool()
        {
            IsInitialized = false; // Desativar lógica de movimento
            Pool?.ReturnObject(gameObject); // Retorna ao pool
        }

        
    }
}