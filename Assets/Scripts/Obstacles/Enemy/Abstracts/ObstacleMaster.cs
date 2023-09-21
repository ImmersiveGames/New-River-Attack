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

        protected bool isActive;
        Vector3 m_ObjectStartPosition;
        Quaternion m_ObjectStartRotate;
        Vector3 m_ObjectStartScale;
        protected PlayerMaster m_PlayerMaster;
        protected GamePlayManager gamePlayManager;
        protected GamePlaySettings gamePlaySettings;

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
            if(!enemy.canRespawn)
                gamePlayManager.EventEnemiesMasterForceRespawn += ForceRespawn;
            StartObstacle();
        }
        internal virtual void OnTriggerEnter(Collider other)
        {
            if (other == null || !shouldObstacleBeReady || !enemy.canDestruct) return;
            ComponentToKill(other.GetComponent<BulletPlayer>(), CollisionType.Shoot);
            ComponentToKill(other.GetComponent<BulletPlayerBomb>(), CollisionType.Bomb);
        }
        internal virtual void OnDisable()
        {
            //isActive = false;
        }
        internal virtual void OnDestroy()
        {
            gamePlayManager.EventActivateEnemiesMaster -= ActiveObject;
            gamePlayManager.EventDeactivateEnemiesMaster -= DeactivateObject;
            gamePlayManager.EventReSpawnEnemiesMaster -= TryRespawn;
            if(!enemy.canRespawn)
                gamePlayManager.EventEnemiesMasterForceRespawn -= ForceRespawn;
        }
  #endregion
        protected virtual void SetInitialReferences()
        {
            gamePlayManager = GamePlayManager.instance;
            gamePlaySettings = gamePlayManager.gamePlaySettings;
        }
        public virtual bool shouldObstacleBeReady
        {
            get { return isDestroyed == false && isActive; }
        }
        protected virtual void StartObstacle()
        {
            isDestroyed = false;
            isActive = true;
            var objTransform = transform;
            objTransform.position = m_ObjectStartPosition;
            objTransform.rotation = m_ObjectStartRotate;
            objTransform.localScale = m_ObjectStartScale;
        }

        protected void ComponentToKill(Component other, CollisionType collisionType)
        {
            if (other == null) return;
            m_PlayerMaster = WhoHit(other);
            OnEventObstacleMasterHit();
            OnEventObstacleScore(m_PlayerMaster.getPlayerSettings);
            ShouldSavePoint(m_PlayerMaster.getPlayerSettings);
            GamePlayManager.AddResultList(gamePlaySettings.hitEnemiesResultsList, m_PlayerMaster.getPlayerSettings, enemy, 1, collisionType);
            ShouldFinishGame();
        }
        internal static PlayerMaster WhoHit(Component other)
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
            isActive = false;
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

        void ForceRespawn()
        {
            StartObstacle();
            Tools.ToggleChildren(transform);
        }

        protected virtual void ActiveObject()
        {
            isActive = true;
        }
        protected virtual void DeactivateObject()
        {
            isActive = false;
        }
        protected void ShouldSavePoint(PlayerSettings playerSettings)
        {
            if (!enemy.isCheckInPoint)
                return;
            var transform1 = transform;
            var position = transform1.position;
            playerSettings.spawnPosition.z = position.z;
            playerSettings.spawnPosition.x = position.x;
            gamePlayManager.OnEventBuildPathUpdate(position.z);
        }

        void ShouldFinishGame()
        {
            if (!enemy.isFinishLevel) return;
            gamePlayManager.readyToFinish = true;
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

    }
}
