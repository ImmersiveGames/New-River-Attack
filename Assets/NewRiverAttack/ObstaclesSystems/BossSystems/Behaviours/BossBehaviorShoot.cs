using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Helpers.Interfaces;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorShoot : ObjectShoot, INodeFunctionProvider
    {
        private BossMaster _bossMaster;

        [Header("Node Reference")]
        public int idNode;

        [Header("Bullet Settings")]
        [SerializeField] private int bulletDamage = 1;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private float bulletLifetime = 5f;

        protected override void Awake()
        {
            base.Awake();
            _bossMaster = GetComponent<BossMaster>();
        }

        private void Start()
        {
            SetTarget(null);
            var player = PlayersManager.Instance.GetPlayerMaster(0);
            if (player != null && enableTargeting)
            {
                SetTarget(player.transform);
            }

            UpdateSpawnPoint();
        }

        private void OnDisable()
        {
            if (shootPattern is IResettablePattern resettablePattern)
            {
                resettablePattern.ExitPattern(this);
            }
        }

        public string NodeName => $"BossShoot_{idNode}";
        public int NodeID => idNode;

        // Função de execução do tiro para o Behavior Tree
        public Func<NodeState> GetNodeFunction() => ExecuteShooting;

        private NodeState ExecuteShooting()
        {
            if (Time.realtimeSinceStartup < LastShootTime + cooldown) return NodeState.Running;
            UpdateSpawnPoint();
            if (enableTargeting && target != null)
            {
                TargetingSystem.AimAtTarget(SpawnPoint, target);
            }

            if (shootPattern == null || SpawnPoint == null) return NodeState.Failure;
            shootPattern?.Execute(SpawnPoint, this);
            LastShootTime = Time.realtimeSinceStartup; // Atualiza o cooldown

            return NodeState.Success;
        }

        public void OnEnter()
        {
            UpdateSpawnPoint();
            if (shootPattern is IResettablePattern resettablePattern)
            {
                resettablePattern.ResetPattern(this);
            }
        }
        public void OnExit()
        {
            if (shootPattern is IResettablePattern resettablePattern)
            {
                resettablePattern.ExitPattern(this);
            }
        }
        public override ISpawnData CreateBulletData(Vector3 direction, Vector3 position)
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
