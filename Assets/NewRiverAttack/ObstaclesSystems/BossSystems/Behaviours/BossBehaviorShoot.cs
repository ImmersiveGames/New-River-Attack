using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
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
            enableTargeting = true;
            _bossMaster = GetComponent<BossMaster>();
        }

        private void Start()
        {
            var player = PlayersManager.Instance.GetPlayerMaster(0);
            if (player != null)
            {
                SetTarget(player.transform);
            }

            UpdateSpawnPoint();
        }

        public string NodeName => $"BossShoot_{idNode}";
        public int NodeID => idNode;

        // Função de execução do tiro para o Behavior Tree
        public Func<NodeState> GetNodeFunction() => ExecuteShooting;

        private NodeState ExecuteShooting()
        {
            // Usa TryShoot para lidar com o cooldown e decidir o estado do nó
            var shotExecuted = shootPattern.TryShoot(ExecuteShootPattern);
            return shotExecuted ? NodeState.Success : // Disparo efetuado com sucesso
                NodeState.Running; // Ainda no cooldown, continua rodando
        }

        public void OnEnter()
        {
            if (shootPattern is IResettablePattern resettablePattern)
            {
                resettablePattern.ResetPattern(this);
            }
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
