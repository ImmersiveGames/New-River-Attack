﻿using System.Collections.Generic;
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

        public void SetShoots(int numMissile, float angle, float cadence, int repeat)
        {
            numMissiles = numMissile;
            angleCones = angle <= 15 ? 15 : angle;
            timesRepeat = repeat <= 0 ? 3 : repeat;
            CadenceShoot = cadence <= 0 ? CadenceShoot : cadence;
        }
        

        protected override void Fire()
        {
            var directions = ConeDirections(numMissiles, angleCones);
            foreach (var t in directions)
            {
                base.Fire();
                var bulletData = Bullet.GetComponent<Bullets>().GetBulletData;
                if (bulletData != null)
                    bulletData.BulletDirection = t;
            }
        }

        private IEnumerable<Vector3> ConeDirections(int numMissile, float angleCone)
        {
            var directions = new Vector3[numMissile];

            // Obtém a direção do eixo Z do SpawnPoint
            var forwardDirection = SpawnPoint.forward;

            // Calcula o ângulo central do cone no plano XZ
            var centerAngle = Mathf.Atan2(forwardDirection.x, forwardDirection.z) * Mathf.Rad2Deg;
            var initialAngle = centerAngle - angleCone / 2;
            var angleIncrease = angleCone / (numMissile - 1);

            // Converte o ângulo central para radianos e cria a rotação
            var rotation = Quaternion.Euler(0, -centerAngle, 0); // Corrige a rotação para o ângulo central

            for (var i = 0; i < numMissile; i++)
            {
                var angle = initialAngle + angleIncrease * i;
                var deg2Rad = angle * Mathf.Deg2Rad;

                // Calcula a direção no plano XZ
                var direction = new Vector3(Mathf.Sin(deg2Rad), 0f, Mathf.Cos(deg2Rad));

                // Rotaciona a direção para alinhar com a direção do SpawnPoint
                direction = rotation * direction;

                directions[i] = direction;
            }

            return directions;
        }
    }
}
