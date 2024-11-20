using NewRiverAttack.BulletsManagers.Interface;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletBossGasStation : Bullet
    {
        private BulletSpawnData _bulletSpawnData;
        protected override void OnEnable()
        {
            base.OnEnable();
            _bulletSpawnData = SpawnData as BulletSpawnData;
        }
        private void Update()
        {
            if (!IsInitialize) return;
            if (transform.position.y > .03)
            {
                var vector3 = transform.position;
                vector3.y = 0.1f;
                transform.position = vector3;
            }
            // Movimenta o projétil na direção e velocidade fornecidas
            transform.position += -transform.forward * (_bulletSpawnData.Speed * Time.deltaTime);

            // Reduz o tempo de vida do projétil
            Lifetime -= Time.deltaTime;

            // Se o tempo de vida acabar, retorna ao pool
            if (Lifetime <= 0)
            {
                ReturnToPool();
            }
        }
    }
}