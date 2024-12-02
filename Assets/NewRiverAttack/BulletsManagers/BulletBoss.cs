using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletBoss : Bullet
    {
        private BulletSpawnData _bulletSpawnData;
        private void Update()
        {
            if (!IsInitialize) return;

            // Movimenta o projétil na direção e velocidade fornecidas
            transform.position += SpawnData.Direction * (_bulletSpawnData.Speed * Time.deltaTime);

            // Reduz o tempo de vida do projétil
            Lifetime -= Time.deltaTime;

            // Se o tempo de vida acabar, retorna ao pool
            if (Lifetime <= 0)
            {
                ReturnToPool();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<BulletBoss>()) return;
            if (other.GetComponentInParent<ObstacleMaster>()) return;
            ReturnToPool();
        }
        public override void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            base.OnSpawned(spawnPosition, data);
            _bulletSpawnData = SpawnData as BulletSpawnData;
        }
    }
}