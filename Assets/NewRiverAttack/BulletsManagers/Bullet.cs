using System;
using ImmersiveGames.PoolSystems;
using UnityEngine;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;

namespace NewRiverAttack.BulletsManagers
{
    public abstract class Bullet : MonoBehaviour, IPoolable
    {
        protected BulletSpawnData BulletData;
        protected float Lifetime;

        public bool IsInitialize { get; private set; }

        private void OnDisable()
        {
            IsInitialize = false;
        }

        #region IPoolable

        public PoolObject Pool { get; set; } // Referência ao pool

        // Método chamado quando o projétil é instanciado do pool
        public virtual void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            BulletData = data as BulletSpawnData;
            if (BulletData != null)
            {
                Lifetime = BulletData.Timer;
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
            BulletData = null;    // Limpa os dados do projétil
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