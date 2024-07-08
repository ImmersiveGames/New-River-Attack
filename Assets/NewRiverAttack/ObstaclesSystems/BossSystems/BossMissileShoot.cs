using System.Collections.Generic;
using ImmersiveGames.BulletsManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossMissileShoot : BossShoot
    {
        [SerializeField] private int numMissiles = 5;
        [SerializeField] private float angleCones = 90;

        private void Awake()
        {
            poolName = $"Pool ({nameof(BossMissileShoot)})";
            angleCones = angleCones <= 15 ? 15 : angleCones;
        }

        public void SetMissiles(int numMissile, float angle)
        {
            numMissiles = numMissile;
            angleCones = angle <= 15 ? 15 : angle;
        }


        protected override void Fire()
        {
            var bossPosition = SpawnPoint.transform.position;
            var directions = ConeDirections(bossPosition, numMissiles, angleCones);

            foreach (var t in directions)
            {
                base.Fire();
                var bulletData = Bullet.GetComponent<Bullets>().GetBulletData;
                bulletData.BulletDirection = t;
                //Debug.Log($"Firing bullet with direction: {t.normalized}");
                //Debug.Log($"SpawnPoint Position: {_spawnPoint.position}, Rotation: {_spawnPoint.rotation}");
            }
        }

        private static IEnumerable<Vector3> ConeDirections(Vector3 targetDirection, int numMissile, float angleCone)
        {
            var directions = new Vector3[numMissile];

            var centerAngle = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
            var initialAngle = centerAngle - angleCone / 2;
            var angleIncrease = angleCone / (numMissile - 1);

            for (var i = 0; i < numMissile; i++)
            {
                var angle = initialAngle + angleIncrease * i;
                var deg2Rad = angle * Mathf.Deg2Rad;
                var finalDirection = new Vector3(Mathf.Cos(deg2Rad), 0f, Mathf.Sin(deg2Rad));
                directions[i] = finalDirection;
                //Debug.Log($"Direction {i}: {finalDirection} with angle {angle}");
            }

            return directions;
        }
    }
}