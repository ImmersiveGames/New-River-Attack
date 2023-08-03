using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public class EnemiesMovement : ObstacleDetectApproach
    {
        
        [Header("Movement Settings")]
        [SerializeField] bool ignoreWalls;
        [SerializeField] bool ignoreEnemies;
        [SerializeField] protected internal float moveVelocity;
        [Header("Movement with Animation Curve")]
        [SerializeField] protected internal float animationDuration;
        [SerializeField] public AnimationCurve animationCurve;
        public enum Directions { None, Forward, Back, Up, Right, Down, Left, Free }
        [Header("Movement Directions")]
        [SerializeField] public Directions startDirection;
        public Vector3 freeVectorDirection;
        Vector3 m_VectorDirection;
        bool m_InCollision;
        Vector3 m_StartPosition;
        Quaternion m_StartRotation;
        
        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemyDifficult;
        
        #region IShoots
        readonly StateMove m_StateMove;
        readonly StateMoveHold m_StateMoveHold;
        readonly StateMovePatrol m_StateMovePatrol;
        IMove m_ActualState;
        public EnemiesMovement()
        {
            m_StateMove = new StateMove(this);
            m_StateMoveHold = new StateMoveHold(this);
            m_StateMovePatrol = new StateMovePatrol(this, null);
        }
    #endregion
        #region UNITY METHODS
        void Awake()
        {
            m_VectorDirection = SetDirection(startDirection);
            var transform1 = transform;
            m_StartPosition = transform1.position;
            m_StartRotation = transform1.rotation;
        }
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventResetEnemies += ResetEnemyMovement;
        }
        void Start()
        {
            ChangeState(m_StateMoveHold);
        }
        void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldEnemyBeReady || m_EnemiesMaster.isDestroyed || !meshRenderer.isVisible)
                return;
            switch (shouldBeMoving)
            {
                case true when shouldBeApproach && !target:
                    target = null;
                    ChangeState(m_StateMovePatrol);
                    target = m_StateMovePatrol.target;
                    break;
                case true:
                    ChangeState(m_StateMove);
                    break;
                case false:
                    target = null;
                    ChangeState(m_StateMoveHold);
                    break;
            }
            m_ActualState.UpdateState(transform, m_VectorDirection);
        }
        void OnTriggerEnter(Collider other)
        {
            if (!m_GamePlayManager.shouldBePlayingGame || 
                !m_EnemiesMaster.shouldEnemyBeReady || 
                m_EnemiesMaster.isDestroyed || 
                !meshRenderer.isVisible || m_ActualState != m_StateMove || m_InCollision) return;
            
            if((other.GetComponentInParent<WallsMaster>() && ignoreWalls) || 
               (other.GetComponentInParent<EnemiesMaster>() && ignoreEnemies) || 
               other.GetComponentInParent<PlayerMaster>()) return;
            
            m_InCollision = true;
            m_VectorDirection *= -1;
            var newDirection = GetDirection(m_VectorDirection);
            m_VectorDirection = SetDirection(newDirection);
            m_EnemiesMaster.CallEventEnemiesMasterFlipEnemies(m_VectorDirection);
            m_InCollision = false;
        }
        void OnDisable()
        {
            m_GamePlayManager.EventResetEnemies -= ResetEnemyMovement;
        }
                
  #endregion
        protected override void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
            base.SetInitialReferences();
        }
        void ChangeState(IMove newState)
        {
            if (m_ActualState == newState) return;
            m_ActualState?.ExitState();

            m_ActualState = newState;
            m_ActualState?.EnterState(m_EnemiesMaster);
        }

        bool shouldBeMoving
        {
            get
            {
                return moveVelocity > 0;
            }
        }

        void ResetEnemyMovement()
        {
            m_VectorDirection = SetDirection(startDirection);
            ChangeState(m_StateMoveHold);
            var transform1 = transform;
            transform1.position = new Vector3(m_StartPosition.x, m_StartPosition.y, m_StartPosition.z);
            transform1.rotation = new Quaternion(m_StartRotation.x, m_StartRotation.y, m_StartRotation.z, m_StartRotation.w);

        }
        public Vector3 SetDirection(Directions dir)
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

        static Directions GetDirection(Vector3 vector3)
        {
            if (vector3 == new Vector3(0, 1, 0))
                return Directions.Up;
            if (vector3 == new Vector3(0, -1, 0))
                return Directions.Down;
            if (vector3 == new Vector3(1, 0, 0))
                return Directions.Right;
            if (vector3 == new Vector3(-1, 0, 0))
                return Directions.Left;
            if (vector3 == new Vector3(0, 0, 1))
                return Directions.Forward;
            if (vector3 == new Vector3(0, 0, -1))
                return Directions.Back;
            return vector3 != Vector3.zero ? Directions.Free : Directions.None;
        }
        
    }
}
