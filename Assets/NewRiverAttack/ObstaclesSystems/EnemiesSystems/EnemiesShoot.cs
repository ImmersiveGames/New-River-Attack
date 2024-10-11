using UnityEngine;
using ImmersiveGames.FiniteStateMachine;
using NewRiverAttack.BulletsManagers.Interface;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.ObstaclesSystems.ShootStates;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesShoot : ObjectShoot
    {
        private StateMachine _stateMachine;
        private EnemiesMaster _enemiesMaster;
        private EnemiesScriptable _enemiesScriptable;
        private Transform _target;
        private bool _isVisible;

        protected override void Awake()
        {
            base.Awake();
            _enemiesMaster = GetComponent<EnemiesMaster>();
            _enemiesScriptable = _enemiesMaster.GetEnemySettings;
            
            InitializeStateMachine();
        }

        private void OnEnable()
        {
            _enemiesMaster.EventObstacleChangeSkin += UpdateSpawnPoint;
        }

        private void OnDisable()
        {
            _enemiesMaster.EventObstacleChangeSkin -= UpdateSpawnPoint;
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        protected void OnBecameVisible()
        {
            _isVisible = true;
        }

        protected void OnBecameInvisible()
        {
            _isVisible = false;
        }

        public override float GetCadenceShoot => _enemiesScriptable.cadenceShoot;

        public override BulletSpawnData CreateBulletData(Vector3 direction, Vector3 position)
        {
            return new BulletSpawnData(
                _enemiesMaster,
                direction,
                position,
                _enemiesScriptable.damageShoot,
                _enemiesScriptable.speedShoot,
                _enemiesScriptable.timeoutDestroy,
                false
            );
        }

        public bool ShouldBeReady => _isVisible && _enemiesMaster.ShouldBeReady;

        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }

        public override void ResetShoot()
        {
            _target = null;
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();

            var patrolState = new PatrolState(this);
            var shootState = new ShootState(this);

            _stateMachine.AddTransition(patrolState, shootState, () => _target != null && ShouldBeReady);
            _stateMachine.AddTransition(shootState, patrolState, () => _target == null || !ShouldBeReady);

            _stateMachine.SetState(patrolState);
        }
    }
}
