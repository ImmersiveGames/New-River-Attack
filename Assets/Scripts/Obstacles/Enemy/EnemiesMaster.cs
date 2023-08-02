using UnityEngine;

namespace RiverAttack
{
    public class EnemiesMaster : ObjectMaster
    {
        public EnemiesScriptable enemy;
        public bool isDestroyed;
        public bool goalLevel;

        protected internal enum EnemyStatus { Paused, Active }
        [SerializeField]
        protected internal EnemyStatus actualEnemyStatus;
        public bool shouldEnemyBeReady
        {
            get
            {
                return isDestroyed == false && actualEnemyStatus == EnemyStatus.Active;
            }
        }
        [SerializeField]
        EnemiesSetDifficulty.EnemyDifficult actualDifficultName;
        const float DESTROY_DELAY = 0.1f;
        Vector3 enemyStartPosition
        {
            get;
            set;
        }

        public EnemiesSetDifficulty.EnemyDifficult getDifficultName
        {
            get
            {
                return actualDifficultName;
            }
            private set
            {
                actualDifficultName = value;
            }
        }

        private protected GamePlayManager gamePlayManager;

        #region Delegates
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventDestroyEnemy;
        public event GeneralEventHandler EventChangeSkin;
        public delegate void MovementEventHandler(Vector3 pos);
        public event MovementEventHandler EventEnemiesMasterMovement;
        public event MovementEventHandler EventEnemiesMasterFlipEnemies;
        public delegate void EnemyEventHandler(PlayerMaster playerMaster);
        public event EnemyEventHandler EventPlayerDestroyEnemy;
  #endregion

        #region UNITY METHODS
        void Awake()
        {
            //gameObject.name = enemy.name;

            enemyStartPosition = transform.position;
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
        
        protected virtual void SetInitialReferences()
        {
            gamePlayManager = GamePlayManager.instance;
            int novoLayer = Mathf.RoundToInt(Mathf.Log(GameManager.instance.layerEnemies.value, 2));
            gameObject.layer = novoLayer;
        }

        void OnInitializeEnemy()
        {
            if (!enemy.canRespawn && isDestroyed)
                Destroy(gameObject, DESTROY_DELAY);
            else
            {
                Utils.Tools.ToggleChildren(transform);
                transform.position = enemyStartPosition;
                actualEnemyStatus = EnemyStatus.Active;
                isDestroyed = false;
            }
        }

    #region Calls
        
        public void CallEventEnemiesMasterMovement(Vector3 pos)
        {
            EventEnemiesMasterMovement?.Invoke(pos);
        }
        public void CallEventEnemiesMasterFlipEnemies(Vector3 pos)
        {
            EventEnemiesMasterFlipEnemies?.Invoke(pos);
        }
        public void CallEventDestroyEnemy(PlayerMaster playerMaster)
        {
            actualEnemyStatus = EnemyStatus.Paused;
            EventDestroyEnemy?.Invoke();
            EventPlayerDestroyEnemy?.Invoke(playerMaster);
        }
        public void CallEventChangeSkin()
        {
            EventChangeSkin?.Invoke();
        }
    #endregion
    }
}
