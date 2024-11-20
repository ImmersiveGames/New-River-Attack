using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public sealed class BulletPlayer : Bullet
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

        public BulletSpawnData GetData => _bulletSpawnData;

        public override void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            _bulletSpawnData = data as BulletSpawnData;
            base.OnSpawned(spawnPosition, _bulletSpawnData);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<Bullet>()) return;
            if (other.GetComponentInParent<PlayerMaster>()) return;
            ReturnToPool();
        }
    }
}