using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers.Interfaces;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class SingleShotPattern : IShootPattern
    {
        private readonly CooldownSystem _cooldownSystem;
        private readonly float _cooldown;

        public SingleShotPattern(float cooldown)
        {
            _cooldown = cooldown;
            _cooldownSystem = new CooldownSystem();
        }

        public bool CanShoot() => _cooldownSystem.IsCooldownComplete(_cooldown);

        public void Execute(Transform spawnPoint, ObjectShoot shooter)
        {
            if (!CanShoot()) return;

            var target = GamePlayManager.Instance.GetPlayerMaster(0).transform;
            var targetingSystem = new TargetingSystem();
            if (!targetingSystem.AimAtTarget(spawnPoint, target)) return;

            var bulletData = shooter.CreateBulletData(spawnPoint.forward, spawnPoint.position);
            shooter.Fire(bulletData, spawnPoint);
            _cooldownSystem.UpdateLastActionTime();
        }
    }
}