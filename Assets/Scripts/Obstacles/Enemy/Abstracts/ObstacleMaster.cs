using UnityEngine;
using UnityEngine.Serialization;
using Utils;
namespace RiverAttack
{
    public abstract class ObstacleMaster : ObjectMaster
    {
        public EnemiesScriptable enemy;
        [Header("Enemy Destroy Settings")]
        [SerializeField]
        GameObject deadParticlePrefab;
        [FormerlySerializedAs("timeoutDestroy")]
        [SerializeField]
        float timeoutDestroyExplosion;
        public bool isDestroyed;

        bool m_IsActive;
        Vector3 m_ObjectStartPosition;
        Quaternion m_ObjectStartRotate;
        Vector3 m_ObjectStartScale;
        protected GamePlayManager gamePlayManager;
        GamePlaySettings m_GamePlaySettings;

        #region Events
        public delegate void GeneralEventHandler();
        protected internal event GeneralEventHandler EventObstacleMasterHit;
        public delegate void PlayerSettingsEventHandler(PlayerSettings playerSettings);
        protected internal event PlayerSettingsEventHandler EventObstacleScore;
        public delegate void MovementEventHandler(Vector3 dir);
        protected internal event MovementEventHandler EventObstacleMovement;
  #endregion

        #region UNITYMETHODS
        internal virtual void Awake()
        {
            var objTransform = transform;
            m_ObjectStartPosition = objTransform.position;
            m_ObjectStartRotate = objTransform.rotation;
            m_ObjectStartScale = objTransform.localScale;
        }
        internal virtual void OnEnable()
        {
            SetInitialReferences();
            gamePlayManager.EventActivateEnemiesMaster += ActiveObject;
            gamePlayManager.EventDeactivateEnemiesMaster += DeactivateObject;
            gamePlayManager.EventReSpawnEnemiesMaster += TryRespawn;
            StartObstacle();
        }
        internal virtual void OnTriggerEnter(Collider other)
        {
            if (other == null || !shouldObstacleBeReady || !enemy.canDestruct) return;
            ComponentToKill(other.GetComponent<BulletPlayer>(), CollisionType.Shoot);
        }
        void OnDisable()
        {
            m_IsActive = false;
        }
        internal virtual void OnDestroy()
        {
            gamePlayManager.EventActivateEnemiesMaster -= ActiveObject;
            gamePlayManager.EventDeactivateEnemiesMaster -= DeactivateObject;
            gamePlayManager.EventReSpawnEnemiesMaster -= TryRespawn;
        }
  #endregion
        protected virtual void SetInitialReferences()
        {
            gamePlayManager = GamePlayManager.instance;
            m_GamePlaySettings = gamePlayManager.gamePlaySettings;
        }
        public virtual bool shouldObstacleBeReady
        {
            get { return isDestroyed == false && m_IsActive; }
        }
        protected virtual void StartObstacle()
        {
            isDestroyed = false;
            m_IsActive = false;
            var objTransform = transform;
            objTransform.position = m_ObjectStartPosition;
            objTransform.rotation = m_ObjectStartRotate;
            objTransform.localScale = m_ObjectStartScale;
        }

        protected void ComponentToKill(Component other, CollisionType collisionType)
        {
            if (other == null) return;
            var playerMaster = WhoHit(other);
            OnEventObstacleMasterHit();
            OnEventObstacleScore(playerMaster.getPlayerSettings);
            ShouldSavePoint(playerMaster.getPlayerSettings);
            GamePlayManager.AddResultList(m_GamePlaySettings.hitEnemiesResultsList, playerMaster.getPlayerSettings, enemy,1, collisionType);
        }
        static PlayerMaster WhoHit(Component other)
        {
            return other switch
            {
                Bullets bullet => bullet.ownerShoot as PlayerMaster,
                PlayerMaster player => player,
                _ => null
            };
        }

        protected virtual void DestroyObstacle()
        {
            isDestroyed = true;
            m_IsActive = false;
            Tools.ToggleChildren(transform, false);
            var explosion = Instantiate(deadParticlePrefab, transform);
            Destroy(explosion, timeoutDestroyExplosion);
        }
        void TryRespawn()
        {
            if (!enemy.canRespawn) return;
            StartObstacle();
            Tools.ToggleChildren(transform);
        }

        protected virtual void ActiveObject()
        {
            m_IsActive = true;
        }
        protected virtual void DeactivateObject()
        {
            m_IsActive = false;
        }
        void ShouldSavePoint(PlayerSettings playerSettings)
        {
            if (enemy.isCheckInPoint)
                playerSettings.spawnPosition.z = transform.position.z;
        }

        #region Calls
        void OnEventObstacleMasterHit()
        {
            DestroyObstacle();
            EventObstacleMasterHit?.Invoke();
        }
        void OnEventObstacleScore(PlayerSettings playerSettings)
        {
            EventObstacleScore?.Invoke(playerSettings);
        }
        protected internal virtual void OnEventObstacleMovement(Vector3 dir)
        {
            EventObstacleMovement?.Invoke(dir);
        }
  #endregion
        /*const float DESTROY_DELAY = 0.1f;
        
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
    #endregion*/
        
    } 
}

