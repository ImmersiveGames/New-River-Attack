using System;
using Unity.VisualScripting;
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
        GamePlayManager m_GamePlayManager;
        GamePlaySettings m_GamePlaySettings;

        #region Events
        protected delegate void GeneralEventHandler();
        protected event GeneralEventHandler EventObstacleMasterHit;
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
            m_GamePlayManager.EventActivateEnemiesMaster += ActiveObject;
            m_GamePlayManager.EventDeactivateEnemiesMaster += DeactivateObject;
            StartObstacle();
        }
        internal virtual void OnTriggerEnter(Collider other)
        {
            if (!shouldObstacleBeReady) return;
            var bullet = other.GetComponent<Bullets>();
            if (bullet)
            {
                KillByBullets(bullet);
            }
        }
        internal virtual void OnDisable()
        {
            m_IsActive = false;
        }
        internal virtual void OnDestroy()
        {
            m_GamePlayManager.EventActivateEnemiesMaster -= ActiveObject;
            m_GamePlayManager.EventDeactivateEnemiesMaster -= DeactivateObject;
        }
  #endregion
        protected virtual void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlaySettings = m_GamePlayManager.gamePlaySettings;
        }
        protected virtual bool shouldObstacleBeReady
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

        protected virtual void KillByBullets(Component bullets)
        {
            if (!enemy.canDestruct) return;
            var playerMaster = WhoHit(bullets);
            OnEventObstacleMasterHit();
            m_GamePlayManager.AddResultList(m_GamePlaySettings.hitEnemiesResultsList, playerMaster.getPlayerSettings, enemy,1, CollisionType.Shoot);
        }
        protected abstract void HitThis(Collider collision);
        static PlayerMaster WhoHit(Component other)
        {
            return other switch
            {
                Bullets bullet => bullet.ownerShoot as PlayerMaster,
                PlayerMaster player => player,
                _ => null
            };
        }

        protected virtual void KillObstacle()
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

        }

        protected virtual void ActiveObject()
        {
            m_IsActive = true;
        }
        protected virtual void DeactivateObject()
        {
            m_IsActive = false;
        }


        #region Calls
        protected virtual void OnEventObstacleMasterHit()
        {
            KillObstacle();
            EventObstacleMasterHit?.Invoke();
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

