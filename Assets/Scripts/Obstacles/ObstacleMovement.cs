using UnityEngine;
namespace RiverAttack
{
    public abstract class ObstacleMovement : MonoBehaviour
    {
        public bool canMove;
        /*[SerializeField]
        DifficultyList enemiesDifficultyList;*/
        [SerializeField]
        protected float moveSpeed;
        public enum Directions { None, Up, Right, Down, Left, Forward, Backward, Free }
        [SerializeField]
        protected Directions directions;
        protected Vector3 faceDirection;
        [SerializeField]
        protected Vector3 freeDirection;
        [SerializeField]
        protected AnimationCurve animationCurve;

    #region Variable Private
        Directions m_StartDirection;
        Vector3 m_StartFreeDirection;
        // ReSharper disable once InconsistentNaming
        private protected Vector3 m_EnemyMovement;
        GamePlayManager m_GamePlayManager;
        protected EnemiesMaster enemyMaster;
        protected EnemiesScriptable enemy;
    #endregion
    #region Unity METHODS
        private void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventResetEnemies += ResetMovement;
        }
        private void Update()
        {
            MoveEnemy();
        }
        private void OnDisable()
        {
            m_GamePlayManager.EventResetEnemies -= ResetMovement;
        }
    #endregion

        int m_HighScorePlayer;
        private float enemySpeedy { get { return (enemy.enemiesDifficulty.GetDifficult(m_HighScorePlayer).multiplySpeedy > 0) ? movementSpeed * enemy.enemiesDifficulty.GetDifficult(m_HighScorePlayer).multiplySpeedy : movementSpeed; } }
        public Directions moveDirection { get { return directions; } set { directions = value; } }
        public Vector3 moveFree { get { return freeDirection; } set { freeDirection = value; } }
        public float movementSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
        public AnimationCurve curveMovement { get { return animationCurve; } set { animationCurve = value; } }

        

        private void SetInitialReferences()
        {
            m_StartDirection = directions;
            m_GamePlayManager = GamePlayManager.instance;
            m_HighScorePlayer = m_GamePlayManager.HighScorePlayers();
            enemyMaster = GetComponent<EnemiesMaster>();
            enemy = enemyMaster.enemy;
            m_StartFreeDirection = freeDirection;
            SetDirection(directions);
        }

        

        protected virtual void MoveEnemy()
        {
            //Debug.Log("Canmove: "+ canMove + " Move dir: " + moveDirection + " Gameover?: " + gameManager.isGameOver + " Pause: " + gameManager.isPaused + " Destroy: " + enemyMaster.IsDestroyed);
            if (!canMove || !m_GamePlayManager.shouldBePlayingGame || directions == Directions.None || enemyMaster.isDestroyed)
                return;
            m_EnemyMovement = faceDirection * (enemySpeedy * Time.deltaTime);
            transform.position += m_EnemyMovement;
            if (animationCurve.length > 1)
                transform.position = MoveInCurveAnimation();
        }

        private Vector3 MoveInCurveAnimation()
        {
            float st, st2, modPos;
            var position = transform.position;
            switch (m_StartDirection)
            {
                case Directions.Up or Directions.Down:
                    st = enemyMaster.enemyStartPosition.y - 2;
                    st2 = enemyMaster.enemyStartPosition.y + 2;
                    modPos = Mathf.Lerp(st, st2, animationCurve.Evaluate(Time.time));
                    return new Vector3(position.x, modPos, position.z);
                case Directions.Right or Directions.Left:
                    st = enemyMaster.enemyStartPosition.x - 2;
                    st2 = enemyMaster.enemyStartPosition.x + 2;
                    modPos = Mathf.Lerp(st, st2, animationCurve.Evaluate(Time.time));
                    return new Vector3(modPos, position.y, position.z);
                default:
                    return new Vector3(position.x, position.y, position.z);
            }
        }

        private void SetDirection(Directions dir)
        {
            switch (dir)
            {
                case Directions.Up:
                    faceDirection = Vector3.up;
                    return;
                case Directions.Right:
                    faceDirection = Vector3.right;
                    return;
                case Directions.Down:
                    faceDirection = Vector3.down;
                    return;
                case Directions.Left:
                    faceDirection = Vector3.left;
                    return;
                case Directions.Backward:
                    faceDirection = Vector3.back;
                    return;
                case Directions.Forward:
                    faceDirection = Vector3.forward;
                    return;
                case Directions.None:
                    faceDirection = Vector3.zero;
                    return;
                case Directions.Free:
                    faceDirection = freeDirection;
                    return;
                default:
                    faceDirection = Vector3.zero;
                    break;
            }
        }

        private void ResetMovement()
        {
            m_EnemyMovement = Vector3.zero;
            directions = m_StartDirection;
            freeDirection = m_StartFreeDirection;
            SetDirection(directions);
        }
    }
}
