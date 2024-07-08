using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.ObstaclesSystems.ShootStates;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public sealed class EnemiesShoot : ObjectShoot
    {
        private bool _isShoot;
        
        private IShoot _startState;
        private EnemiesMaster _enemiesMaster;
        private EnemiesScriptables _enemies;
        private GamePlayManager _gamePlayManagers;
        #region Unity Methods
        private void OnEnable()
        {
            SetInitialReferences();
            _enemiesMaster.EventObstacleChangeSkin += UpdateCadenceShoot;
            _gamePlayManagers.EventGameRestart -= ResetMovement;
        }

        private void OnBecameInvisible()
        {
            Invoke(nameof(StopShoot), 1f);
        }

        private void OnBecameVisible()
        {
            StartShoot();
        }
        
        private void Start()
        {
            if (_enemies.GetShootApproach != 0) 
                _startState = new ShootStatePatrol(this, false);
            if (_enemies.GetShootApproach == 0) 
                _startState = new ShootStateShoot(this);
            
            ChangeState(_startState);
        }

        private void Update()
        {
            if (!_enemiesMaster.ObjectIsReady) return;
            GetActualState?.UpdateState(transform);
        }

        private void OnDisable()
        {
            _enemiesMaster.EventObstacleChangeSkin -= UpdateCadenceShoot;
            _gamePlayManagers.EventGameRestart -= ResetMovement;
        }

        #endregion

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _isShoot = false;
            _gamePlayManagers = GamePlayManager.instance;
            _enemiesMaster = GetComponent<EnemiesMaster>();
            _enemies = _enemiesMaster.objectDefault as EnemiesScriptables;
            _shootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
        }

        internal void UpdateCadenceShoot()
        {
            CadenceShoot = _enemies.cadenceShoot;
            _shootSpawnPoint = GetComponentInChildren<ShootSpawnPoint>();
        }

        public override void SetDataBullet(ObjectMaster objectMaster)
        {
            var bulletSpeed = 0f;
            var damage = 0;

            var enemiesMaster = objectMaster as EnemiesMaster;
            if (enemiesMaster != null)
            {
                var enemy = enemiesMaster.objectDefault as EnemiesScriptables;
                if (enemy != null)
                {
                    bulletSpeed = enemy.speedShoot;
                    damage = enemy.damageShoot;
                }
            }
            MakeBullet(enemiesMaster, bulletSpeed, damage, bulletLifeTime, Vector3.forward);
        }

       
        #region StateMachine
        
        internal void ChangeState(IShoot newState)
        {
            if (GetActualState == newState) return;
            GetActualState?.ExitState();

            GetActualState = newState;
            GetActualState?.EnterState(_enemiesMaster);
        }
        private void ResetMovement()
        {
            ChangeState(_startState);
        }
        private IShoot GetActualState { get; set; }
        public bool ShouldBeShoot => _isShoot && _enemiesMaster.ObjectIsReady;

        private void StartShoot()
        {
            _isShoot = true;
        }

        public void StopShoot()
        {
            _isShoot = false;
        }

        #endregion
        
        #region Gizmos
#if UNITY_EDITOR
        private Vector2 _gizmoRadius;
        [Header("Gizmos Settings")]
        public Color gizmoColor = new Color(255, 0, 0, 150);
        private void OnDrawGizmos()
        {
            var em = GetComponent<EnemiesMaster>();
            _gizmoRadius = em.GetEnemySettings.approachShoot;
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            if (_gizmoRadius is { x: 0, y: > 0 })
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireSphere(center: position, _gizmoRadius.y);
            }
            if (_gizmoRadius.x == 0) return;
            Gizmos.color = gizmoColor + new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, _gizmoRadius.x);
            Gizmos.color = gizmoColor - new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, _gizmoRadius.y);
        }
#endif
        #endregion
    }
}