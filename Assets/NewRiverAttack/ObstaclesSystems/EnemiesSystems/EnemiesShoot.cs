using NewRiverAttack.BulletsManagers.Interface;
using ImmersiveGames.FiniteStateMachine;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.ObstaclesSystems.ShootStates;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesShoot : ObjectShoot
    {
        private StateMachine _stateMachine;
        private EnemiesMaster _enemiesMaster;
        private EnemiesScriptable _enemiesScriptable;
        private Transform _target;

        // Variável para controlar se o inimigo está visível na tela
        private bool _isVisible;
        public bool ShouldBeShoot => _isVisible && _enemiesMaster.ObjectIsReady;

        #region UnityMethods

        protected override void OnEnable()
        {
            base.OnEnable();
            _enemiesMaster.EventObstacleChangeSkin += UpdateCadenceShoot;
        }

        private void Update()
        {
            _stateMachine.Tick(); // Atualiza o estado da FSM
        }

        private void OnBecameVisible()
        {
            _isVisible = true; // Inimigo está visível, deve atirar
        }

        private void OnBecameInvisible()
        {
            _isVisible = false; // Inimigo saiu da tela, para de atirar
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _enemiesMaster.EventObstacleChangeSkin -= UpdateCadenceShoot;
        }

        #endregion

        #region Configuração e Inicialização

        // Inicializa a FSM e define os estados e transições
        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();

            var patrolState = new PatrolState(this, _enemiesScriptable);
            var shootState = new ShootState(this);

            // Transição de Patrulha para Atirar
            _stateMachine.AddTransition(patrolState, shootState, patrolState.IsTargetInRange);

            // Transição garantida para o estado de atirar
            _stateMachine.AddAnyTransition(shootState, () => _target != null);

            // Define o estado inicial com base no GetShootApproach
            _stateMachine.SetState(_enemiesScriptable.GetShootApproach == 0 ? shootState : patrolState);
        }

        // Configura as referências iniciais e chama a inicialização da FSM
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _enemiesMaster = GetComponent<EnemiesMaster>();
            _enemiesScriptable = _enemiesMaster.GetEnemySettings;

            UpdateCadenceShoot();
            InitializeStateMachine();
        }

        // Reseta o estado de tiro e reinicializa a FSM
        public override void ResetShoot()
        {
            // Reseta variáveis internas e visibilidade
            _isVisible = false;
            _target = null;

            InitializeStateMachine(); // Reconfigura a FSM
            UpdateCadenceShoot(); // Atualiza o ponto de disparo
        }

        #endregion

        #region Controle de Tiro e Pool

        // Atualiza o ponto de spawn e outras referências
        private void UpdateCadenceShoot()
        {
            var shootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
            SpawnPoint = shootSpawnPoint != null ? shootSpawnPoint.transform : transform;
        }

        // Define a cadência de tiro específica para o inimigo
        protected override float GetCadenceShoot()
        {
            return _enemiesScriptable.cadenceShoot;
        }

        // Criação dos dados da bala para o inimigo
        protected override BulletSpawnData CreateBulletData(Vector3 direction)
        {
            return new BulletSpawnData
            (
                _enemiesMaster,
                direction, // SpawnPoint.forward Direção sempre no SpawnPoint
                _enemiesScriptable.damageShoot,
                _enemiesScriptable.speedShoot,
                _enemiesScriptable.timeoutDestroy,
                false
            );
        }

        #endregion

        #region Gizmos

#if UNITY_EDITOR

        [Header("Gizmos Settings")] public Color gizmoColor = new Color(255, 0, 0, 150); // Cor padrão do gizmo
        private Vector2 _gizmoRadius;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;
            var em = GetComponent<EnemiesMaster>();
            if (em == null || em.GetEnemySettings == null) return;

            _gizmoRadius = em.GetEnemySettings.approachShoot;
        }

        private void OnDrawGizmosSelected()
        {
            if (_gizmoRadius == Vector2.zero) return; // Sem área definida

            var position = transform.position;

            Gizmos.color = gizmoColor - new Color(0.2f, 0.2f, 0.2f); // Cor para o máximo
            Gizmos.DrawWireSphere(center: position, _gizmoRadius.y);

            if (!(_gizmoRadius.x > 0)) return;
            Gizmos.color = gizmoColor + new Color(0.2f, 0.2f, 0.2f); // Cor para o mínimo
            Gizmos.DrawWireSphere(center: position, _gizmoRadius.x);
        }
#endif

        #endregion

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}
