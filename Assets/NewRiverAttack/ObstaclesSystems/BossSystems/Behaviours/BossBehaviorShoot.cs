using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorShoot : ObjectShoot, INodeFunctionProvider
    {
        private BossMaster _bossMaster;

        [Header("Node Reference")]
        public int idNode;  // Identificador do nó para uso na árvore de comportamento

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
            SetTarget(PlayersManager.Instance.GetPlayerMaster(0).transform);
        }

        public string NodeName => $"BossShoot_{idNode}";
        public int NodeID => idNode;

        // Retorna a função de execução do tiro para o nó de comportamento
        public Func<NodeState> GetNodeFunction() => ExecuteShooting;

        // Função de execução de tiro, usada pelo Behavior Tree
        private NodeState ExecuteShooting()
        {
            if (!shootPattern.CanShoot())
            {
                return NodeState.Running; // Retorna Running enquanto o cooldown estiver ativo
            }

            ExecuteShootPattern(); // Executa o padrão de tiro configurado
            ShootSound();
            return NodeState.Success; // Retorna Success após o tiro
        }

        // Define os dados específicos do projétil do boss
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
