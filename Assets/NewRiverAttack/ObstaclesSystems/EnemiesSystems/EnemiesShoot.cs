using ImmersiveGames.FiniteStateMachine;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.ObstaclesSystems.ShootStates;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesShoot : ObjectShoot
    {
        private StateMachine _stateMachine;
        private IState _startState;
        private EnemiesMaster _enemiesMaster;
        private bool _isVisible; // Checagem de visibilidade
        private EnemiesScriptable _enemiesScriptable;

        protected override void Awake()
        {
            base.Awake();
            _enemiesMaster = GetComponent<EnemiesMaster>();
            _enemiesScriptable = _enemiesMaster.GetEnemySettings;
            InitializeStateMachine();
        }

        private void OnEnable()
        {
            _enemiesMaster.EventObjectChangeSkin += UpdateSpawnPoint;
            if (_stateMachine != null)
            {
                ResetBehavior();
            }
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void OnDisable()
        {
            _enemiesMaster.EventObjectChangeSkin -= UpdateSpawnPoint;
        }

        public bool ShootIsReady => _isVisible && _enemiesMaster.ObjectIsReady;
        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();
            var shootState = new ShootState(this);
            var patrolState = new PatrolState(this);

            // Transição para o estado de tiro apenas se o inimigo tiver um alvo e estiver visível
            _stateMachine.AddTransition(patrolState, shootState, () => target != null);
            _startState = _enemiesScriptable.GetShootApproach != 0 ? patrolState : shootState;
            ResetBehavior();
        }

        private void ResetBehavior()
        {
            target = null;
            _stateMachine.SetState(_startState);
        }

        // Atualiza o estado de visibilidade
        private void OnBecameVisible() => _isVisible = true;
        private void OnBecameInvisible() => _isVisible = false;

        // Cria os dados do projétil específico para o inimigo
        public override ISpawnData CreateBulletData(Vector3 direction, Vector3 position)
        {
            return new BulletSpawnData(
                _enemiesMaster,
                direction,
                position,
                _enemiesMaster.GetEnemySettings.damageShoot,
                _enemiesMaster.GetEnemySettings.speedShoot,
                _enemiesMaster.GetEnemySettings.timeoutDestroy,
                false
            );
        }
    }
}
