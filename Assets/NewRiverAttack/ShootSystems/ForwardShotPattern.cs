using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ShootSystems
{
    [CreateAssetMenu(fileName = "ForwardShotPattern", menuName = "ImmersiveGames/RiverAttack/ShootPatterns/ForwardShot", order = 401)]
    public class ForwardShotPattern : ShootPatternBase
    {
        public override void Execute(Transform spawnPoint, ObjectShoot shooter)
        {
            if (!CanShoot()) return;

            var bulletData = shooter.CreateBulletData(spawnPoint.forward, spawnPoint.position);
            shooter.PoolingOut(spawnPoint, bulletData);
            shooter.ShootSound();

            UpdateLastShootTime();
        }
    }
}