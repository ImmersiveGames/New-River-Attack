using System;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public class EnemiesMovement : ObstacleDetectApproach
    {
        [SerializeField] bool ignoreWalls;
        [SerializeField] bool ignoreEnemies;
        [Header("Movement Settings")]
        [SerializeField]
        protected internal float moveVelocity;
        [Header("Movement with Animation Curve")]
        [SerializeField]
        protected internal float animationDuration;
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
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldEnemyBeReady || m_EnemiesMaster.isDestroyed || !m_Renderer.isVisible)
                return;
            switch (shouldBeMoving)
            {
                case true when shouldBeApproach && !m_Target:
                    m_Target = null;
                    ChangeState(m_StateMovePatrol);
                    m_Target = m_StateMovePatrol.target;
                    break;
                case true:
                    ChangeState(m_StateMove);
                    break;
                case false:
                    m_Target = null;
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
                !m_Renderer.isVisible || m_ActualState != m_StateMove || m_InCollision) return;
            
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
            base.SetInitialReferences();
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
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
        
        /*
        [Header("Enemy Settings")]
        [SerializeField]
        public bool hasAnimationFlip;
        
        [SerializeField] internal float moveVelocity;
        public enum Directions { None, Up, Right, Down, Left, Forward, Backward, Free }
        [SerializeField]
        protected internal Directions directions;
        protected internal Directions startDirection;
        [SerializeField]
        protected internal Vector3 moveFreeDirection;
        protected internal Vector3 facingDirection;
        [Header("Motion with Animation Curve")]
        [SerializeField]
        protected internal float animationDuration;
        [SerializeField]
        protected internal AnimationCurve animationCurve;
        
        #region IMove
        readonly StateMove m_StateMove = new StateMove();
        readonly StateMoveHold m_StateMoveHold = new StateMoveHold();
        readonly StateMovePatrol m_StateMovePatrol = new StateMovePatrol();
        IMove m_ActualState;
        #endregion
        bool m_AlreadyCollided;
        Transform m_Target;
        
        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemyDifficult;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_EnemiesMaster.EventDestroyEnemy += StopMove;
            m_GamePlayManager.EventEnemyDestroyPlayer += StopMove;
        }
        void Start()
        {
            ChangeState(m_StateMoveHold);
        }
        void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldEnemyBeReady || m_EnemiesMaster.isDestroyed || !m_Renderer.isVisible)
                return;
            switch (shouldBeMoving)
            {
                case true when shouldBeMoveByApproach && !m_Target:
                    m_Target = null;
                    ChangeState(m_StateMovePatrol);
                    m_Target = m_StateMovePatrol.target;
                    break;
                case true:
                    ChangeState(m_StateMove);
                    break;
                case false:
                    m_Target = null;
                    ChangeState(m_StateMoveHold);
                    break;
            }
            m_ActualState.UpdateState();
        }
        void OnTriggerEnter(Collider other)
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldEnemyBeReady || m_EnemiesMaster.isDestroyed || !m_Renderer.isVisible)
                return;
            if (m_ActualState != m_StateMove) return;
            if(m_AlreadyCollided || (other.GetComponentInParent<WallsMaster>() && m_EnemiesMaster.ignoreWall) || 
               (other.GetComponentInParent<EnemiesMaster>() && m_EnemiesMaster.ignoreEnemies) || 
               other.GetComponentInParent<PlayerMaster>()) return;
            m_AlreadyCollided = true;
            Debug.Log("Face Antes: "+ facingDirection);
            facingDirection = m_StateMove.FlipMe();
            Debug.Log("Face Depois: "+ facingDirection);
            if (hasAnimationFlip) m_EnemiesMaster.CallEventEnemiesMasterFlipEnemies(facingDirection);
            m_AlreadyCollided = false;
        }
        void OnDisable()
        {
            m_EnemiesMaster.EventDestroyEnemy -= StopMove;
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
            m_GamePlayManager = GamePlayManager.instance;
            startDirection = directions;


            // Setar definições do movimento ???


        }
        void ChangeState(IMove newState)
        {
            if (m_ActualState == newState) return;
            m_ActualState?.ExitState();

            m_ActualState = newState;
            m_ActualState?.EnterState(m_EnemiesMaster, this);
        }
        bool shouldBeMoving
        {
            get
            {
                return moveVelocity > 0 && (directions != Directions.None || startDirection != Directions.None);
            }
        }
        bool shouldBeMoveByApproach
        {
            get
            {
                return playerApproachRadius != 0 || playerApproachRadiusRandom != Vector2.zero;
            }
        }
        
        void StopMove()
        {
            directions = Directions.None;
            ChangeState(m_StateMoveHold);
        }

        /*
        bool m_AlreadyCollided;

        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemyDifficult;
        
        
        #region UNITYMETHODS
        void Awake()
        {
            startDirection = directions;
            obstacleMovementState = MovementState.Paused;
        }
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventResetEnemies += ResetMovement;
        }
        void Start()
        {
            playerApproachRadius = SetPlayerApproachRadius();

            if (m_EnemiesMaster == null) return;
            LogInitialMovementStatus(m_EnemiesMaster.enemy);
            
            m_EnemiesMaster.canMove = canMove = playerApproachRadius == 0;
            
            if (m_EnemiesMaster.enemy == null || m_EnemiesMaster.enemy.enemiesSetDifficultyListSo == null) return;
            DifficultUpdates();
            
            if (playerApproachRadius != 0) InvokeRepeating(nameof(HasPlayerApproach), 0, timeToCheck);
            
        }
        void OnTriggerEnter(Collider other)
        {
            if(m_AlreadyCollided || (other.GetComponentInParent<WallsMaster>() && m_EnemiesMaster.ignoreWall) || 
               (other.GetComponentInParent<EnemiesMaster>() && m_EnemiesMaster.ignoreEnemies) || 
            other.GetComponentInParent<PlayerMaster>()) return;

            FlipMe();
        }
        void LateUpdate()
        {
            if (!canMove || !GamePlayManager.instance.shouldBePlayingGame || directions == Directions.None || m_EnemiesMaster.isDestroyed) return;
            
            m_AlreadyCollided = false;
            
            if (m_EnemiesMaster != null && m_EnemyDifficult.enemyDifficult != m_EnemiesMaster.getDifficultName)
                DifficultUpdates();

            facingDirection = SetDirection(directions);
            m_EnemiesMaster.CallEventEnemiesMasterMovement(facingDirection);
            if (animationCurve != null && animationDuration > 0)
            {
                MoveCurveAnimation(facingDirection,moveVelocity,animationDuration, animationCurve);
            }
            else
            {
                Move(facingDirection, moveVelocity);
            }
        }
        void OnDisable()
        {
            m_GamePlayManager.EventResetEnemies -= ResetMovement;
        }
  #endregion
        void SetInitialReferences()
        {
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
            m_GamePlayManager = GamePlayManager.instance;
            
        }
        protected void HasPlayerApproach()
        {
            m_EnemiesMaster.canMove = canMove = FindTarget<PlayerMaster>(GameManager.instance.layerPlayer);
        }
        void DifficultUpdates()
        {
            m_EnemyDifficult = m_EnemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(m_EnemiesMaster.getDifficultName);
            moveVelocity = m_EnemiesMaster.enemy.velocity * m_EnemyDifficult.multiplyEnemiesSpeedy;
            
            if (canMoveByApproach == false || (m_EnemiesMaster.enemy.radiusToApproach == 0 || playerApproachRadius == 0)) return;
            if(m_EnemiesMaster.enemy.radiusToApproach != 0 && m_EnemyDifficult.multiplyPlayerDistanceRadius != 0)
                playerApproachRadius = m_EnemiesMaster.enemy.radiusToApproach * m_EnemyDifficult.multiplyPlayerDistanceRadius;
        }
        
        void FlipMe()
        {
            m_AlreadyCollided = true;
            switch (directions)
            {
                case Directions.Up:
                    directions = Directions.Down;
                    break;
                case Directions.Right:
                    directions = Directions.Left;
                    break;
                case Directions.Down:
                    directions = Directions.Up;
                    break;
                case Directions.Left:
                    directions = Directions.Right;
                    break;
                case Directions.Forward:
                    directions = Directions.Backward;
                    break;
                case Directions.Backward:
                    directions = Directions.Forward;
                    break;
                case Directions.None:
                case Directions.Free:
                default:
                    moveFreeDirection *= -1;
                    break;
            }
            facingDirection = SetDirection(directions);
            if (hasAnimationFlip) m_EnemiesMaster.CallEventEnemiesMasterFlipEnemies(facingDirection);
        }
        void LogInitialMovementStatus(EnemiesScriptable enemy)
        {
            enemy.velocity = moveVelocity;
            enemy.radiusToApproach = playerApproachRadius;
        }
        */
    }
}
