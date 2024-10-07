using System;
using ImmersiveGames.PoolSystems;
using UnityEngine;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;

namespace NewRiverAttack.BulletsManagers
{
    public abstract class Bullet : MonoBehaviour, IPoolable
    {
        protected Vector3 Direction;
        protected ObjectMaster Owned;
        protected Vector3 Position;
        protected int Damage;
        protected float Speed;
        protected float LifeTimer;
        protected bool PowerUp;
        protected bool IsInitialized;
        
        
        private float _lifetime;
        

        public PoolObject Pool { get; set; } // Referência ao pool

        // Método chamado quando o projétil é instanciado do pool
        public void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            // Inicializa os dados da bala a partir do BulletSpawnData
            if (data is BulletSpawnData bulletData)
            {
                Direction = bulletData.Direction;
                Owned = bulletData.Owner;
                Position = bulletData.Position;
                Damage = bulletData.Damage;
                Speed = bulletData.Speed;
                PowerUp = bulletData.PowerUp;
                
                _lifetime = bulletData.Timer; // Usamos "Timer" como tempo de vida
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

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<ObjectMaster>() == Owned) return;
            if (other.GetComponentInParent<Bullet>() == this) return;
            ReturnToPool();
        }
    }
}