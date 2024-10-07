using UnityEngine;

namespace NewRiverAttack.BulletsManagers
{
    public class BulletBoss : Bullet
    {
        private void Update()
        {
            if (!IsInitialized) return;

            // Movimenta o projétil na direção e velocidade fornecidas
            transform.position += Direction * Speed * Time.deltaTime;

            // Reduz o tempo de vida do projétil
            LifeTimer -= Time.deltaTime;

            // Se o tempo de vida acabar, retorna ao pool
            if (LifeTimer <= 0)
            {
                ReturnToPool();
            }
        }
    }
}