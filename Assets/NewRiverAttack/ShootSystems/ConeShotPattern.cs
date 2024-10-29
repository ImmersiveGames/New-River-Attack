using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ShootSystems
{
    [CreateAssetMenu(fileName = "ConeShotPattern", menuName = "ImmersiveGames/RiverAttack/ShootPatterns/ConeShot", order = 403)]
    public class ConeShotPattern : ShootPatternBase
    {
        [SerializeField] private int projectileCount = 3;
        [SerializeField] private float coneAngle = 45.0f;

        public override void Execute(Transform spawnPoint, ObjectShoot shooter)
        {
            if (!CanShoot()) return;

            var directionToShoot = spawnPoint.forward;
            var angleStep = coneAngle / Mathf.Max(projectileCount - 1, 1);
            var startAngle = -coneAngle / 2;

            for (var i = 0; i < projectileCount; i++)
            {
                var currentAngle = startAngle + (i * angleStep);
                var rotation = Quaternion.Euler(0, currentAngle, 0);
                var shootDirection = rotation * directionToShoot;

                var bulletData = shooter.CreateBulletData(shootDirection, spawnPoint.position);
                shooter.PoolingOut(spawnPoint, bulletData);
            }

            shooter.ShootSound();
            UpdateLastShootTime();
        }
    }
}