using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace RiverAttack
{
    public class EnemiesMovement : ObstacleDetectApproach
    {
        [Header("Movement Settings")]
        [SerializeField] bool ignoreWalls;
        [SerializeField] bool ignoreEnemies;
        [SerializeField] internal float moveVelocity;
        public enum Directions { None, Forward, Back, Up, Right, Down, Left, Free }
        [Header("Movement Directions")]
        [SerializeField] Directions startDirection;
        Vector3 m_VectorDirection;
        public Vector3 freeVectorDirection;
        [Header("Movement with Animation Curve")]
        [SerializeField] internal float animationDuration;
        [SerializeField] internal AnimationCurve animationCurve;
   
        IMove m_ActualState;
        EnemiesMaster m_EnemiesMaster;
        GamePlayManager m_GamePlayManager;
        bool m_InCollision;

        #region UNITYMETHODS
        void Awake()
        {
            m_VectorDirection = SetDirection(startDirection);
        }
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventReSpawnEnemiesMaster += ResetEnemyMovement;
        }
        void Start()
        {
            ChangeState(new StateMoveHold(this, m_EnemiesMaster));
        }
        void OnTriggerEnter(Collider other)
        {
            if (m_ActualState is not StateMove) return;
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldObstacleBeReady || m_EnemiesMaster.isDestroyed || !meshRenderer.isVisible)
                return;
            if(m_InCollision || (other.GetComponentInParent<WallsMaster>() && ignoreWalls) || (other.GetComponentInParent<EnemiesMaster>() && ignoreEnemies)) 
                return;
            if (!other.GetComponentInParent<EnemiesMaster>() && !other.GetComponentInParent<WallsMaster>()) return;
            
            m_InCollision = true;
            m_VectorDirection *= -1;
            m_EnemiesMaster.OnEventObjectMasterFlipEnemies(true);
        }
        void OnTriggerExit(Collider other)
        {
            if (m_ActualState is not StateMove) return;
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldObstacleBeReady || m_EnemiesMaster.isDestroyed || !meshRenderer.isVisible)
                return;
            if(!m_InCollision || (other.GetComponentInParent<WallsMaster>() && ignoreWalls) || (other.GetComponentInParent<EnemiesMaster>() && ignoreEnemies)) 
                return;
            if (!other.GetComponentInParent<EnemiesMaster>() && !other.GetComponentInParent<WallsMaster>()) return;
            m_InCollision = false;
        }
        void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_EnemiesMaster.shouldObstacleBeReady || m_EnemiesMaster.isDestroyed || !meshRenderer.isVisible)
                return;
            m_ActualState.UpdateState(transform, m_VectorDirection);
        }

        void OnDisable()
        {
            
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_GamePlayManager = GamePlayManager.instance;
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
        }
        internal bool shouldBeMoving { get { return m_EnemiesMaster.isActive && moveVelocity > 0; } }
        internal void ChangeState(IMove newState)
        {
            if (m_ActualState == newState) return;
            m_ActualState?.ExitState();

            m_ActualState = newState;
            m_ActualState?.EnterState();
        }
        
        void ResetEnemyMovement()
        {
            m_VectorDirection = SetDirection(startDirection);
            m_InCollision = false;
            target = null;
            ChangeState(new StateMoveHold(this, m_EnemiesMaster));
        }

        Vector3 SetDirection(Directions dir)
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

        #region Gizmos
        void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            
            if (playerApproachRadius <= 0 && playerApproachRadiusRandom.y <= 0) return;
            float newPlayerApproachRadius = playerApproachRadiusRandom != Vector2.zero ? Random.Range(playerApproachRadiusRandom.x, playerApproachRadiusRandom.y) : playerApproachRadius;
            var enemiesMaster = GetComponent<EnemiesMaster>();
            if (!enemiesMaster.enemy && !enemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            var difficult = enemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(enemiesMaster.actualDifficultName);
            float realApproachRadius = newPlayerApproachRadius * difficult.multiplyPlayerDistanceRadiusToMove;
            var position = transform.position;

            // Código que será executado apenas no Editor
            if (playerApproachRadiusRandom == Vector2.zero)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireSphere(center: position, realApproachRadius);
            }
            if(playerApproachRadiusRandom == Vector2.zero) return;
            Gizmos.color = gizmoColor + new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, playerApproachRadiusRandom.x * difficult.multiplyPlayerDistanceRadiusToMove);
            Gizmos.color = gizmoColor - new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, playerApproachRadiusRandom.y * difficult.multiplyPlayerDistanceRadiusToMove);
#endif
        }
  #endregion

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
