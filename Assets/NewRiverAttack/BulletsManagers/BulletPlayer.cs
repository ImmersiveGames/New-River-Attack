using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public sealed class BulletPlayer : Bullet
    {
        private void Update()
        {
            if (!IsInitialize) return;

            // Movimenta o projétil na direção e velocidade fornecidas
            transform.position += GetData.Direction * (GetData.Speed * Time.deltaTime);

            // Reduz o tempo de vida do projétil
            Lifetime -= Time.deltaTime;

            // Se o tempo de vida acabar, retorna ao pool
            if (Lifetime <= 0)
            {
                ReturnToPool();
            }
        }

        public BulletSpawnData GetData { get; private set; }

        public override void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            GetData = data as BulletSpawnData;
            base.OnSpawned(spawnPosition, GetData);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<BulletEnemies>()) return;
            if (other.GetComponentInParent<PlayerMaster>()) return;
            Invoke(nameof(ReturnToPool), 0.02f);
        }
    }
}