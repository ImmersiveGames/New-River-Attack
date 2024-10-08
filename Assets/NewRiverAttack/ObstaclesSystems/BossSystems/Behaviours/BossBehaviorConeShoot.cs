using System;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorConeShoot : ObjectShoot, INodeFunctionProvider
    {
        private BossMaster _bossMaster;

        [Header("Node Reference")]
        public int idNode;

        [Header("Bullet Settings")]
        [SerializeField] private int bulletDamage = 1;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private float bulletLifetime = 5f;

        [Header("Cone Shoot Settings")]
        [SerializeField] private int projectileCount = 5;
        [SerializeField] private float coneAngle = 45f;

        [Header("Cooldown Settings")]
        [SerializeField] private float shootCooldown = 1.5f;
        
        private ConeShotPattern _shootPattern;
        public override float GetCadenceShoot => shootCooldown;

        protected override void Awake()
        {
            base.Awake();
            _bossMaster = GetComponent<BossMaster>();
            _shootPattern = new ConeShotPattern(projectileCount, coneAngle, shootCooldown);
            SetShootPattern(_shootPattern);
        }

        public string NodeName => $"BossConeShoot_{idNode}";
        public int NodeID => idNode;

        public Func<NodeState> GetNodeFunction() => ExecuteShooting;
        private NodeState ExecuteShooting()
        {
            if (!_shootPattern.CanShoot())
            {
                return NodeState.Running; // Enquanto o cooldown estiver ativo, retorna Running
            }

            ExecuteShootPattern(); // Executa o padrão de tiro
            return NodeState.Success; // Retorna Success após o tiro
        }

        public override BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position)
        {
            return new BulletSpawnData(
                _bossMaster,
                direction,
                position,
                bulletDamage,
                bulletSpeed,
                bulletLifetime,
                false
            );
        }
    }
}