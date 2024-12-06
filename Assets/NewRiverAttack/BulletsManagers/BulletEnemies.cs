using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using NewRiverAttack.WallsManagers;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletEnemies : Bullet
    {
        private BulletSpawnData _bulletSpawnData;
        private void Update()
        {
            if (!IsInitialize) return;

            // Movimenta o projétil na direção e velocidade fornecidas
            transform.position += _bulletSpawnData.Direction * (_bulletSpawnData.Speed * Time.deltaTime);

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
            if (other.GetComponentInParent<BulletEnemies>() || 
                other.GetComponentInParent<ObstacleMaster>()||
                other.GetComponentInParent<PowerUpMaster>() || 
                other.GetComponentInParent<WallMaster>()) return;
            ReturnToPool();
        }
        public override void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            _bulletSpawnData = data as BulletSpawnData;
            base.OnSpawned(spawnPosition, _bulletSpawnData);
        }
    }
}