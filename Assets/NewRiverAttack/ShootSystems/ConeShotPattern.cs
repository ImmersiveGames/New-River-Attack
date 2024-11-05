using ImmersiveGames.DebugManagers;
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
            TryShoot(() =>
            {
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
            });
        }

        public override void SetParameter(EnumShootParameter parameter, object value)
        {
            base.SetParameter(parameter, value);  // Chama a base para cooldown

            switch (parameter)
            {
                case EnumShootParameter.ProjectileCount:
                    if (value is int count)
                        projectileCount = count;
                    else
                        DebugManager.LogError<ConeShotPattern>("O valor para ProjectileCount precisa ser do tipo int.");
                    break;

                case EnumShootParameter.ConeAngle:
                    if (value is float angle)
                        coneAngle = angle;
                    else
                        DebugManager.LogError<ConeShotPattern>("O valor para ConeAngle precisa ser do tipo float.");
                    break;

                default:
                    DebugManager.LogWarning<ConeShotPattern>($"Parâmetro '{parameter}' não suportado pelo ConeShotPattern.");
                    break;
            }
        }

        protected override object GetParameter(EnumShootParameter parameterName)
        {
            return parameterName switch
            {
                EnumShootParameter.ProjectileCount => projectileCount,
                EnumShootParameter.ConeAngle => coneAngle,
                _ => base.GetParameter(parameterName)
            };
        }

    }
}
