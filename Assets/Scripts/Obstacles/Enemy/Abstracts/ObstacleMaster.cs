using System;
using Steamworks.Data;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public abstract class ObstacleMaster : ObjectMaster
    {
        public EnemiesScriptable enemy;
        [Header("Enemy Destroy Settings")]
        [SerializeField] internal bool isFinishLevel;
        [SerializeField]
        GameObject deadParticlePrefab; 
        [SerializeField]
        protected float timeoutDestroyExplosion;
        public bool isDestroyed;
        [SerializeField]
        internal bool isActive;
        internal Collider[] myColliders;
        Vector3 m_ObjectStartPosition;
        Quaternion m_ObjectStartRotate;
        Vector3 m_ObjectStartScale;
        protected PlayerMaster playerMaster;
        protected GamePlayManager gamePlayManager;
        protected GamePlayingLog gamePlayingLog;
        public EnemiesSetDifficulty.EnemyDifficult actualDifficultName;
        
        

        #region Events
        public delegate void GeneralEventHandler();
        protected internal event GeneralEventHandler EventObstacleMasterHit;
        public delegate void PlayerSettingsEventHandler(PlayerSettings playerSettings);
        protected internal event PlayerSettingsEventHandler EventObstacleScore;
        public delegate void MovementEventHandler(bool active);
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
            //ComponentToKill(other.GetComponent<BulletBoss>(), CollisionType.None);
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
            gamePlayingLog = gamePlayManager.gamePlayingLog;
        }
        public bool shouldObstacleBeReady
        {
            get { return isDestroyed == false && isActive; }
        }
        void StartObstacle()
        {
            isDestroyed = false;
            isActive = true;
            var objTransform = transform;
            objTransform.position = m_ObjectStartPosition;
            objTransform.rotation = m_ObjectStartRotate;
            objTransform.localScale = m_ObjectStartScale;
        }

        protected virtual void ComponentToKill(Component other, CollisionType collisionType)
        {
            if (other == null) return;
            playerMaster = WhoHit(other);
            OnEventObstacleMasterHit();
            OnEventObstacleScore(playerMaster.getPlayerSettings);
            ShouldSavePoint(playerMaster.getPlayerSettings);
            GamePlayManager.AddResultList(gamePlayingLog.hitEnemiesResultsList, playerMaster.getPlayerSettings, enemy, 1, collisionType);
            KillStatsHandle(collisionType);
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
            if (!enemy.canRespawn || gamePlayManager.bossFight) return;
            StartObstacle();
            Tools.ToggleChildren(transform);
        }

        void ForceRespawn()
        {
            Tools.ToggleChildren(transform);
            StartObstacle();
        }

        void ActiveObject()
        {
            isActive = true;
        }
        void DeactivateObject()
        {
            isActive = false;
        }
        protected void ShouldSavePoint(PlayerSettings playerSettings)
        {
            if (!enemy.isCheckInPoint)
                return;
            if(GameManager.instance.gameModes == GameManager.GameModes.Classic)
                GameSteamManager.AddState("stat_FinishCPath", 1, false);
            GameSteamManager.StoreStats();
            var transform1 = transform;
            var position = transform1.position;
            playerSettings.spawnPosition.z = position.z;
            playerSettings.spawnPosition.x = position.x;
            gamePlayManager.OnEventBuildPathUpdate(position.z);
        }

        void ShouldFinishGame()
        {
            if (!isFinishLevel) return;
            GameSteamManager.StoreStats();
            gamePlayManager.readyToFinish = true;
        }
        void KillStatsHandle(CollisionType collisionType)
        {
            switch (enemy.enemyType)
            {
                case EnemiesTypes.Others:
                    break;
                case EnemiesTypes.Ship:
                    GameSteamManager.AddState("statBeatShip", 1, false);
                    if(collisionType == CollisionType.Collider)
                        GameSteamManager.UnlockAchievement("ACH_CRASH_PLAYER_SHIP");
                    break;
                case EnemiesTypes.Helicopter:
                    GameSteamManager.AddState("statBeatHeli", 1, false);
                    if(collisionType == CollisionType.Collider)
                        GameSteamManager.UnlockAchievement("ACH_CRASH_PLAYER_HELI");
                    break;
                case EnemiesTypes.Hovercraft:
                    GameSteamManager.AddState("statBeatHover", 1, false);
                    break;
                case EnemiesTypes.Drone:
                    GameSteamManager.AddState("statBeatDrones", 1, false);
                    if(collisionType == CollisionType.Collider)
                        GameSteamManager.UnlockAchievement("ACH_CRASH_PLAYER_DRONE");
                    break;
                case EnemiesTypes.Tower:
                    GameSteamManager.AddState("statBeatTower", 1, false);
                    break;
                case EnemiesTypes.Jet:
                    GameSteamManager.AddState("statBeatJet", 1, false);
                    break;
                case EnemiesTypes.Tanks:
                    break;
                case EnemiesTypes.Bridges:
                    GameSteamManager.AddState("statBeatBridge", 1, false);
                    if(collisionType == CollisionType.Collider)
                        GameSteamManager.UnlockAchievement("ACH_CRASH_PLAYER_BRIDGE");
                    break;
                case EnemiesTypes.Submarine:
                    break;
                case EnemiesTypes.GasStation:
                    break;
                case EnemiesTypes.Refugee:
                    break;
                case EnemiesTypes.Collectable:
                    break;
                case EnemiesTypes.Decoration:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            GameSteamManager.StoreStats();
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
        internal void OnEventObstacleMovement(bool active)
        {
            EventObstacleMovement?.Invoke(active);
        }
  #endregion

    }
}
