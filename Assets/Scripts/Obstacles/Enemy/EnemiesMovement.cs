using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public class EnemiesMovement : ObstacleMovement
    {
        [Header("Enemy Settings")]
        [SerializeField] bool hasAnimationFlip;
        bool m_AlreadyCollided;

        GamePlayManager m_GamePlayManager;
        EnemiesMaster m_EnemiesMaster;
        EnemiesSetDifficulty m_EnemyDifficult;
        
        
        #region UNITYMETHODS
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

        float SetPlayerApproachRadius()
        {
            if (playerApproachRadiusRandom != Vector2.zero)
                playerApproachRadius = randomRangeDetect;
           
            return playerApproachRadius;
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
        
    }
}
