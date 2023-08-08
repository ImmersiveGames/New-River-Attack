using UnityEngine;
namespace RiverAttack
{
    public class ObstacleMaster : ObjectMaster
    {
        const float DESTROY_DELAY = 0.1f;
        public EnemiesScriptable enemy;
        public bool isDestroyed;
        protected internal enum EnemyStatus { Paused, Active }
        [SerializeField]
        protected internal EnemyStatus actualEnemyStatus;

        Vector3 m_ObjectStartPosition;
        
        private protected GamePlayManager gamePlayManager;
        
        #region Delegates
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventDestroyObject;
        public event GeneralEventHandler EventChangeSkin;
        public delegate void MovementEventHandler(Vector3 pos);
        public event MovementEventHandler EventObjectMasterMovement;
        public event MovementEventHandler EventObjectMasterFlipEnemies;
        public delegate void EnemyEventHandler(PlayerMaster playerMaster);
        public event EnemyEventHandler EventPlayerDestroyObject;
        #endregion
        
        #region UNITY METHODS
        protected virtual void Awake()
        { 
            m_ObjectStartPosition = transform.position;
            actualEnemyStatus = EnemyStatus.Paused;
            isDestroyed = false;
        }
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            //Tools.SetLayersRecursively(GameManager.instance.layerEnemies, transform);
            gamePlayManager.EventStartPlayGame += OnInitializeEnemy;
            gamePlayManager.EventResetEnemies += OnInitializeEnemy;
        }
        void Start()
        {
            actualEnemyStatus = EnemyStatus.Active;
        }
        protected virtual void OnDisable()
        {
            gamePlayManager.EventStartPlayGame -= OnInitializeEnemy;
            gamePlayManager.EventResetEnemies -= OnInitializeEnemy;
        }
        #endregion
        
        public bool shouldObstacleBeReady
        {
            get
            {
                return isDestroyed == false && actualEnemyStatus == EnemyStatus.Active;
            }
        }
        
        protected virtual void SetInitialReferences()
        {
            gamePlayManager = GamePlayManager.instance;
        }

        void OnInitializeEnemy()
        {
            if (!enemy.canRespawn && isDestroyed)
                Destroy(gameObject, DESTROY_DELAY);
            else
            {
                Utils.Tools.ToggleChildren(transform);
                transform.position = m_ObjectStartPosition;
                actualEnemyStatus = EnemyStatus.Active;
                isDestroyed = false;
            }
        }
        
        #region Calls
        
        public void CallEventEnemiesMasterMovement(Vector3 pos)
        {
            EventObjectMasterMovement?.Invoke(pos);
        }
        public void CallEventEnemiesMasterFlipEnemies(Vector3 pos)
        {
            EventObjectMasterFlipEnemies?.Invoke(pos);
        }
        public void CallEventDestroyEnemy(PlayerMaster playerMaster)
        {
            actualEnemyStatus = EnemyStatus.Paused;
            EventDestroyObject?.Invoke();
            EventPlayerDestroyObject?.Invoke(playerMaster);
        }
        public void CallEventChangeSkin()
        {
            EventChangeSkin?.Invoke();
        }
    #endregion
    } 
}

