using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorSingleShoot : ObjectShoot, INodeFunctionProvider
    {
        private BossMaster _bossMaster;
        [Header("Node Reference")]
        public int idNode;
        [Header("Bullet Settings")]
        [SerializeField] private int bulletDamage = 1;     // Dano da bala
        [SerializeField] private float bulletSpeed = 20f;  // Velocidade da bala
        [SerializeField] private float bulletLifetime = 5f; // Tempo de vida da bala
        
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

        public string NodeName => $"BossSingleShoot_{idNode}";
        public int NodeID => idNode;
        // Função fornecida para a árvore de comportamento
        public Func<NodeState> GetNodeFunction()
        {
            return ShootAction; // Retorna a função de tiro como ActionNode
        }

        #endregion

        #region Shooting Logic

        // Função acionada pelo Behavior Tree para atirar
        private NodeState ShootAction()
        {
            // Verifica se o cooldown já passou desde o último disparo
            if (Time.time - _lastShootTime < shootCooldown)
            {
                return NodeState.Running; // Continua rodando até o cooldown passar
            }

            // Checa se o alvo está disponível para atirar
            if (!Aim()) return NodeState.Failure;

            // Executa o disparo
            var bulletData = CreateBulletData(SpawnPoint.forward, SpawnPoint.position);
            base.Fire(bulletData, SpawnPoint);
            
            // Atualiza o tempo do último disparo para gerenciar o cooldown
            _lastShootTime = Time.time;

            return NodeState.Success;

        }

        // Função para calcular a direção do alvo e ajustar a rotação, mas não dispara
        private bool Aim()
        {
            var target = AlwaysTarget();
            if (target == null) return false; // Não atira se não houver alvo

            // Calcula a direção do alvo e ajusta a rotação do SpawnPoint
            var directionToTarget = (target.position - SpawnPoint.position).normalized;
            SpawnPoint.rotation = Quaternion.LookRotation(directionToTarget);

            return true;
        }

        #endregion

        #region Override de Métodos Herdados de ObjectShoot

        // Define a cadência de tiro do Boss (herdado de ObjectShoot)
        protected override float GetCadenceShoot()
        {
            return shootCooldown; // Agora usa a variável configurável via inspector
        }
        
        private void UpdateCadenceShoot()
        {
            var shootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
            SpawnPoint = shootSpawnPoint != null ? shootSpawnPoint.transform : transform;
        }

        // Criação dos dados do projétil (herdado de ObjectShoot)
        protected override BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position)
        {
            // Cria os dados da bala, substituindo os números mágicos por variáveis configuráveis
            return new BulletSpawnData(_bossMaster, direction, position, bulletDamage, bulletSpeed, bulletLifetime, false);
        }

        // Método para resetar o tiro (herdado de ObjectShoot)
        public override void ResetShoot()
        {
            // Aqui você pode adicionar lógica para resetar qualquer coisa adicional se necessário
        }

        #endregion
    }
}
