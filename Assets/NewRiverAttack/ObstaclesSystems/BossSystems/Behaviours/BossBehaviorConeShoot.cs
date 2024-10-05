using System;
using UnityEngine;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.Tags;
using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorConeShoot : ObjectShoot, INodeFunctionProvider
    {
        private BossMaster _bossMaster;

        [Header("Node Reference")]
        public int idNode;

        [Header("Bullet Settings")]
        [SerializeField] private int bulletDamage = 1;     // Dano da bala
        [SerializeField] private float bulletSpeed = 20f;  // Velocidade da bala
        [SerializeField] private float bulletLifetime = 5f; // Tempo de vida da bala

        [Header("Cone Shoot Settings")]
        [SerializeField] private int projectileCount = 5;   // Quantidade de projéteis
        [SerializeField] private float coneAngle = 45f;     // Ângulo do cone

        [Header("Cooldown Settings")]
        [SerializeField] private float shootCooldown = 1.5f; // Tempo de cooldown entre tiros
        private float _lastShootTime;

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            _bossMaster = GetComponent<BossMaster>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _bossMaster.EventObstacleChangeSkin += UpdateCadenceShoot;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _bossMaster.EventObstacleChangeSkin -= UpdateCadenceShoot;
        }

        #endregion

        #region INodeFunctionProvider Implementation

        public string NodeName => $"BossConeShoot_{idNode}";
        public int NodeID => idNode;
        public Func<NodeState> GetNodeFunction()
        {
            return ShootAction;
        }

        #endregion

        #region Shooting Logic

        private NodeState ShootAction()
        {
            if (Time.time - _lastShootTime < shootCooldown)
            {
                return NodeState.Running;
            }

            if (!Aim()) return NodeState.Failure;

            // Disparar múltiplos projéteis em forma de cone
            ShootInCone();

            _lastShootTime = Time.time;

            return NodeState.Success;
        }

        private bool Aim()
        {
            var target = AlwaysTarget();
            if (target == null) return false;

            var directionToTarget = (target.position - SpawnPoint.position).normalized;
            SpawnPoint.rotation = Quaternion.LookRotation(directionToTarget);

            return true;
        }

        // Função para disparar os projéteis em um formato de cone
        private void ShootInCone()
        {
            float angleStep = coneAngle / (projectileCount - 1);
            float startAngle = -coneAngle / 2;

            for (int i = 0; i < projectileCount; i++)
            {
                float currentAngle = startAngle + (i * angleStep);
                Vector3 shootDirection = Quaternion.Euler(0, currentAngle, 0) * SpawnPoint.forward;

                var bulletData = CreateBulletData(shootDirection);
                base.Fire(bulletData);
            }
        }

        #endregion

        #region Override de Métodos Herdados de ObjectShoot

        protected override float GetCadenceShoot()
        {
            return shootCooldown;
        }

        // Criação dos dados da bala
        protected override BulletSpawnData CreateBulletData(Vector3 direction)
        {
            return new BulletSpawnData(_bossMaster, direction, bulletDamage, bulletSpeed, bulletLifetime, false);
        }

        public override void ResetShoot()
        {
            // Reseta o comportamento de tiro se necessário
        }

        private void UpdateCadenceShoot()
        {
            var shootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
            SpawnPoint = shootSpawnPoint != null ? shootSpawnPoint.transform : transform;
        }

        #endregion
    }
}
