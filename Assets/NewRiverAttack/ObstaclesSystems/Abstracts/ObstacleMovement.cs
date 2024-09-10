using GD.MinMaxSlider;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public class ObstacleMovement : MonoBehaviour
    {
        [Header("Movement Settings")] [Range(0, 50)]
        public float moveVelocity;
        [Header("Movement with Patrol")] [MinMaxSlider(0f, 100f)]
        public Vector2 approachMovement;
        
        protected float MoveVelocity;

        protected enum Directions { None, Forward, Back, Up, Right, Down, Left, Free }
        [SerializeField] protected Directions startDirection;
        [SerializeField] protected Vector3 freeVectorDirection;

        protected IMove StartState;

        protected Vector3 DirectionVector;
        protected ObstacleMaster ObstacleMaster;

        private GamePlayManager _gamePlayManagers;

        public T GetObjectScriptable<T>() where T : class
        {
            return ObstacleMaster.objectDefault as T;
        }
        
        public bool ShouldBeMove => MoveVelocity != 0 && ObstacleMaster.ObjectIsReady;

        #region Unity Methods
        private void Awake()
        {
            DirectionVector = SetDirection(startDirection);
        }
        private void OnEnable()
        {
            SetInitialReferences();
            _gamePlayManagers.EventGameRestart += ResetMovement;
            _gamePlayManagers.EventGameReload += ReloadMovement;
        }
        private void Update()
        {
            if (!ObstacleMaster.ObjectIsReady) return;
            GetActualState?.UpdateState(transform, DirectionVector);
        }
        private void OnDisable()
        {
            _gamePlayManagers.EventGameRestart -= ResetMovement;
            _gamePlayManagers.EventGameReload -= ReloadMovement;
        }
        

        #endregion
        protected virtual void SetInitialReferences()
        {
            _gamePlayManagers = GamePlayManager.Instance;
            ObstacleMaster = GetComponent<ObstacleMaster>();
        }
        protected virtual void ReloadMovement(){}
        protected void SetVelocity(float defaultSpeed)
        {
            MoveVelocity = defaultSpeed;
        }
        public float GetVelocity => MoveVelocity;

        protected virtual void ResetMovement()
        {
            DirectionVector = SetDirection(startDirection);
            ChangeState(StartState);
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

        protected Vector3 SetDirection(Directions dir)
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

        public float GetMoveApproach
        {
            get
            {
                return approachMovement.x switch
                {
                    0 when approachMovement.y > 0 => approachMovement.y,
                    > 0 => Random.Range(approachMovement.x, approachMovement.y),
                    _ => 0
                };
            }
        }

        #endregion
        
        #region Gizmos
#if UNITY_EDITOR
        protected Vector2 GizmoRadius;
        [Header("Gizmos Settings")]
        public Color gizmoColor = new Color(0, 0, 250, 150);
        
        private void OnDrawGizmosSelected()
        {
            if (GizmoRadius == Vector2.zero) return;
            var position = transform.position;
            if (GizmoRadius is { x: 0, y: > 0 })
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireSphere(center: position, GizmoRadius.y);
            }
            if (GizmoRadius.x == 0) return;
            Gizmos.color = gizmoColor + new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, GizmoRadius.x);
            Gizmos.color = gizmoColor - new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, GizmoRadius.y);
        }
#endif
        #endregion
    }
}