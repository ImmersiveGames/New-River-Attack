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
        [SerializeField] private GameObject deadParticlePrefab; 
        [SerializeField]
        protected float timeoutDestroyExplosion;
        public bool isDestroyed;
        [SerializeField]
        internal bool isActive;
        internal Collider[] myColliders;
        private Vector3 m_ObjectStartPosition;
        private Quaternion m_ObjectStartRotate;
        private Vector3 m_ObjectStartScale;
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

        private void StartObstacle()
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
            GamePlayManager.AddResultList(gamePlayingLog.GetEnemiesResult(), playerMaster.getPlayerSettings, enemy, 1, collisionType);
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

        private void TryRespawn()
        {
            if (!enemy.canRespawn || gamePlayManager.bossFight) return;
            StartObstacle();
            Tools.ToggleChildren(transform);
        }

        private void ForceRespawn()
        {
            Tools.ToggleChildren(transform);
            StartObstacle();
        }

        private void ActiveObject()
        {
            isActive = true;
        }

        private void DeactivateObject()
        {
            isActive = false;
        }
        protected void ShouldSavePoint(PlayerSettings playerSettings)
        {
            if (!enemy.isCheckInPoint)
                return;
            if(GameManager.instance.gameModes == GameManager.GameModes.Classic)
                GameSteamManager.AddStat("stat_FinishCPath", 1, false);
            GameSteamManager.StoreStats();
            var transform1 = transform;
            var position = transform1.position;
            playerSettings.spawnPosition.z = position.z;
            playerSettings.spawnPosition.x = position.x;
            gamePlayManager.OnEventBuildPathUpdate(position.z);
        }

        private void ShouldFinishGame()
        {
            if (!isFinishLevel) return;
            GameSteamManager.StoreStats();
            gamePlayManager.readyToFinish = true;
        }

        private void KillStatsHandle(CollisionType collisionType)
        {
            switch (enemy.enemyType)
            {
                case EnemiesTypes.Others:
                    break;
                case EnemiesTypes.Ship:
                    GameSteamManager.AddStat("stat_BeatShip", 1, false);
                    if(collisionType == CollisionType.Collider)
                        GameSteamManager.UnlockAchievement("ACH_CRASH_PLAYER_SHIP");
                    break;
                case EnemiesTypes.Helicopter:
                    GameSteamManager.AddStat("stat_BeatHeli", 1, false);
                    if(collisionType == CollisionType.Collider)
                        GameSteamManager.UnlockAchievement("ACH_CRASH_PLAYER_HELI");
                    break;
                case EnemiesTypes.Hovercraft:
                    GameSteamManager.AddStat("stat_BeatHover", 1, false);
                    break;
                case EnemiesTypes.Drone:
                    GameSteamManager.AddStat("stat_BeatDrones", 1, false);
                    if(collisionType == CollisionType.Collider)
                        GameSteamManager.UnlockAchievement("ACH_CRASH_PLAYER_DRONE");
                    break;
                case EnemiesTypes.Tower:
                    GameSteamManager.AddStat("stat_BeatTower", 1, false);
                    break;
                case EnemiesTypes.Jet:
                    GameSteamManager.AddStat("stat_BeatJet", 1, false);
                    break;
                case EnemiesTypes.Tanks:
                    break;
                case EnemiesTypes.Bridges:
                    GameSteamManager.AddStat("stat_BeatBridge", 1, false);
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
                case EnemiesTypes.Secret:
                    GameSteamManager.UnlockAchievement("ACH_FIND_SECRET");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            GameSteamManager.StoreStats();
        }
        #region Calls

        private void OnEventObstacleMasterHit()
        {
            DestroyObstacle();
            EventObstacleMasterHit?.Invoke();
        }

        private void OnEventObstacleScore(PlayerSettings playerSettings)
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
