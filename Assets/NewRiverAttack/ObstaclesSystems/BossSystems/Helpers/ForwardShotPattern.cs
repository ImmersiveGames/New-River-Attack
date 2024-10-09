using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers.Interfaces;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class ForwardShotPattern : IShootPattern
    {
        private readonly CooldownSystem _cooldownSystem;
        private readonly float _cooldown;

        public ForwardShotPattern(float cooldown)
        {
            _cooldownSystem = new CooldownSystem();
            _cooldown = cooldown;
        }
        public bool CanShoot() => _cooldownSystem.IsCooldownComplete(_cooldown);
        public void Execute(Transform spawnPoint, ObjectShoot shooter)
        {
            // Verifica o cooldown
            if (!_cooldownSystem.IsCooldownComplete(_cooldown)) return;

            // Cria os dados do projétil para disparar na direção do SpawnPoint
            var bulletData = shooter.CreateBulletData(spawnPoint.forward, spawnPoint.position);
            shooter.Fire(bulletData, spawnPoint);

            // Atualiza o tempo do último disparo
            _cooldownSystem.UpdateLastActionTime();
        }
    }
}