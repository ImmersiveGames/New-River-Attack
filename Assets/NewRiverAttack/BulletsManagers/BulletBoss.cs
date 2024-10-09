using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletBoss : Bullet
    {
        private void Update()
        {
            if (!IsInitialize) return;

            // Movimenta o projétil na direção e velocidade fornecidas
            transform.position += BulletData.Direction * (BulletData.Speed * Time.deltaTime);

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
            if (other.GetComponentInParent<Bullet>()) return;
            if (other.GetComponentInParent<ObstacleMaster>()) return;
            ReturnToPool();
        }
    }
}