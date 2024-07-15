using System;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.MovementStates;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.WallsManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesMovement : MonoBehaviour
    {
        protected float _moveVelocity;
        private enum Directions { None, Forward, Back, Up, Right, Down, Left, Free }
        [SerializeField] private Directions startDirection;
        [SerializeField] private Vector3 freeVectorDirection;

        private IMove _startState;
        
        private Vector3 _directionVector;
        protected EnemiesMaster _enemiesMaster;
        protected EnemiesScriptables _enemiesScriptables;
        protected EnemiesAnimation _enemiesAnimation;
        protected GamePlayManager _gamePlayManagers;

        #region Unity Methods

        private void Awake()
        {
            _directionVector = SetDirection(startDirection);
        }

        private void OnEnable()
        {
            SetInitialReferences();
            _gamePlayManagers.EventGameRestart += ResetMovement;
        }

        private void Start()
        {
            _startState = new MoveStateHold(this);
            if (_enemiesScriptables.GetMoveApproach != 0) _startState = new MoveStatePatrol(this);
            if (_enemiesScriptables.GetMoveApproach == 0 && _enemiesScriptables.moveVelocity != 0) _startState = new MoveStateMove(this);

            ChangeState(_startState);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other == null || !_enemiesMaster.ObjectIsReady || (_enemiesScriptables.ignoreWalls && _enemiesScriptables.ignoreEnemies) ) return;
            var enemies = _enemiesScriptables.ignoreEnemies ? null : other.GetComponentInParent<EnemiesMaster>();
            var wall = _enemiesScriptables.ignoreWalls ? null : other.GetComponentInParent<WallMaster>();
            if (enemies == null && wall == null) return;
            _enemiesAnimation.AnimationFlip();
            _directionVector *= -1;
        }

        private void Update()
        {
            if (!_enemiesMaster.ObjectIsReady) return;
            GetActualState?.UpdateState(transform, _directionVector);
        }

        private void OnDisable()
        {
            _gamePlayManagers.EventGameRestart -= ResetMovement;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _gamePlayManagers = GamePlayManager.instance;
            _enemiesMaster = GetComponent<EnemiesMaster>();
            _enemiesAnimation = GetComponent<EnemiesAnimation>();
            _enemiesScriptables = _enemiesMaster.GetEnemySettings;
            _moveVelocity = _enemiesScriptables.moveVelocity;
        }

        public bool ShouldBeMove => _moveVelocity != 0 && _enemiesMaster.ObjectIsReady;
        public EnemiesScriptables GetEnemySettings => _enemiesScriptables;
        
        private void SetVelocity(float defaultSpeed)
        {
            _moveVelocity = defaultSpeed;
        }
        public float GetVelocity => _moveVelocity;

        private void ResetMovement()
        {
            _directionVector = SetDirection(startDirection);
            SetVelocity(_enemiesScriptables.moveVelocity);
            ChangeState(_startState);
        }
        

        #region StateMachine
        
        internal void ChangeState(IMove newState)
        {
            if (GetActualState == newState) return;
            GetActualState?.ExitState();

            GetActualState = newState;
            GetActualState?.EnterState();
        }

        private IMove GetActualState { get; set; }

        #endregion

        #region Functions Auxiliares

        private Vector3 SetDirection(Directions dir)
        {
            return dir switch
            {
                Directions.Up => Vector3.up,
                Directions.Right => Vector3.right,
                Directions.Down => Vector3.down,
                Directions.Left => Vector3.left,
                Directions.Back => Vector3.back,
                Directions.Forward => Vector3.forward,
                Directions.None => Vector3.zero,
                Directions.Free => freeVectorDirection,
                _ => Vector3.zero
            };
        }

        #endregion

        #region Gizmos
#if UNITY_EDITOR
        protected Vector2 _gizmoRadius;
        [Header("Gizmos Settings")]
        public Color gizmoColor = new Color(0, 0, 250, 150);
        protected virtual void OnDrawGizmos()
        {
            var em = GetComponent<EnemiesMaster>();
            _gizmoRadius = em.GetEnemySettings.approachMovement;
        }

        private void OnDrawGizmosSelected()
        {
            if (_gizmoRadius == Vector2.zero) return;
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