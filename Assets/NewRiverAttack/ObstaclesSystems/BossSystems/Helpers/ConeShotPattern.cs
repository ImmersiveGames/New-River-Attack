using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers.Interfaces;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class ConeShotPattern : IShootPattern
    {
        private readonly int _projectileCount;
        private readonly float _coneAngle;
        private readonly float _cooldown;
        private readonly CooldownSystem _cooldownSystem;

        public ConeShotPattern(int projectileCount, float coneAngle, float cooldown)
        {
            _projectileCount = projectileCount;
            _coneAngle = coneAngle;
            _cooldown = cooldown;
            _cooldownSystem = new CooldownSystem();
        }

        public bool CanShoot() => _cooldownSystem.IsCooldownComplete(_cooldown);

        public void Execute(Transform spawnPoint, ObjectShoot shooter)
        {
            // Verifica o cooldown apenas no início do disparo do cone completo
            if (!CanShoot()) return;

            // Verificar se há um alvo
            var target = GamePlayManager.Instance.GetPlayerMaster(0)?.transform;
            if (target == null) return;

            // Direção principal em direção ao alvo
            Vector3 directionToTarget = (target.position - spawnPoint.position).normalized;

            // Cálculo do ângulo do cone e passo entre os projéteis
            float angleStep = _coneAngle / Mathf.Max(_projectileCount - 1, 1);
            float startAngle = -_coneAngle / 2;

            // Disparar múltiplos projéteis em um cone
            for (int i = 0; i < _projectileCount; i++)
            {
                float currentAngle = startAngle + (i * angleStep);  // Ângulo de cada projétil

                // Rotaciona a direção do projétil para criar o padrão de cone
                Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
                Vector3 shootDirection = rotation * directionToTarget;  // Aplica a rotação ao vetor de direção

                // Criar e disparar o projétil
                var bulletData = shooter.CreateBulletData(shootDirection, spawnPoint.position);
                shooter.Fire(bulletData, spawnPoint);
            }

            // Atualiza o cooldown **após** todos os projéteis terem sido disparados
            _cooldownSystem.UpdateLastActionTime();
        }
    }
}
