using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public sealed class PlayerMaster : ObjectMaster
    {
        [SerializeField] internal PlayerSettings getPlayerSettings;
        [Header("Player Destroy Settings")]
        [SerializeField]
        bool isPlayerDead;
        [SerializeField] float timeoutReSpawn;
        [SerializeField]
        GameObject deadParticlePrefab;
        [SerializeField]
        float timeoutDestroyExplosion;
        [Header("Shake Camera")]
        [SerializeField]
        float shakeIntensity;
        [SerializeField]
        float shakeTime;

        public enum MovementStatus { None, Paused, Accelerate, Reduce }
        [Header("Movement")]
        [SerializeField] internal MovementStatus playerMovementStatus;

        [SerializeField]
        internal List<LogPlayerCollectables> collectableList;

        internal PlayersInputActions playersInputActions;
        internal bool inEffectArea;
        [SerializeField]
        internal bool inPowerUp;
        internal int nextScoreForLive;

        Animator m_Animator;
        GamePlaySettings m_GamePlaySettings;
        GamePlayManager m_GamePlayManager;
        GameManager m_GameManager;
        GameSettings m_GameSettings;
        #region Delagetes
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventPlayerMasterHit;
        public event GeneralEventHandler EventPlayerMasterRespawn;

        public delegate void ControllerEventHandler(Vector2 dir);
        public event ControllerEventHandler EventPlayerMasterControllerMovement;
        #endregion

        #region UNITYMETHOD
        void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_GamePlayManager = GamePlayManager.instance;
            m_GameSettings = m_GamePlayManager.getGameSettings;
            m_GameManager = GameManager.instance;
            m_GamePlaySettings = m_GamePlayManager.gamePlaySettings;
            playersInputActions = new PlayersInputActions();
            playersInputActions.Enable();
        }

        void OnEnable()
        {
            m_GamePlayManager.EventStartRapidFire += () => inPowerUp = true;
            m_GamePlayManager.EventEndRapidFire += () => inPowerUp = false;
        }

        void Start()
        {
            PlayerStartSetup();
            MyDebugStart();
        }
        void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<WallsMaster>() && !other.GetComponentInParent<EnemiesMaster>() && !other.GetComponent<BulletEnemy>()) return;
            if (other.GetComponentInParent<CollectiblesMaster>() != null) return;
            if (m_GamePlayManager.getGodMode) return;
            LogGamePlay(other);
            //return;
            OnEventPlayerMasterHit();
        }
  #endregion

        public bool shouldPlayerBeReady { get { return isPlayerDead == false && playerMovementStatus != MovementStatus.Paused; } }

        public void SetPlayerSettingsToPlayMaster(PlayerSettings playerSettings)
        {
            getPlayerSettings = playerSettings;
        }
        public bool CanCollect()
        {
            return getPlayerSettings.bombs < GameSettings.instance.maxBombs;
        }

        public Animator GetPlayerAnimator()
        {
            return m_Animator;
        }

        void PlayerStartSetup()
        {
            inEffectArea = false;
            getPlayerSettings.spawnPosition = transform.position;
            getPlayerSettings.spawnPosition.z = 0;
            isPlayerDead = false;
            getPlayerSettings.score = 0;
            m_GamePlayManager.OnEventUpdateScore(getPlayerSettings.score);
            getPlayerSettings.distance = 0;
            int distInt = Mathf.FloorToInt(getPlayerSettings.distance);
            m_GamePlayManager.OnEventUpdateDistance(distInt);
            //getPlayerSettings.wealth = 0;
            m_GamePlayManager.OnEventUpdateRefugees(getPlayerSettings.wealth);
            getPlayerSettings.bombs = m_GameSettings.startBombs;
            m_GamePlayManager.OnEventUpdateBombs(getPlayerSettings.bombs);
            getPlayerSettings.actualFuel = m_GameSettings.startFuel;
            getPlayerSettings.lives = m_GameSettings.startLives;
            m_GamePlayManager.OnEventUpdateLives(getPlayerSettings.lives);
        }

        void KillPlayer()
        {
            m_GamePlayManager.playerDead = isPlayerDead = true;
            playerMovementStatus = MovementStatus.Paused;
            CameraShake.instance.ShakeCamera(shakeIntensity,shakeTime);
            m_GamePlayManager.OnEventEnemiesMasterKillPlayer();

            Tools.ToggleChildren(transform, false);
            var go = Instantiate(deadParticlePrefab, transform);
            Destroy(go, timeoutDestroyExplosion);
        }

        void ChangeGameOver()
        {
            m_GameManager.ChangeState(new GameStateGameOver());
        }

        void TryRespawn()
        {
            if (getPlayerSettings.lives <= 0)
            {
                Invoke(nameof(ChangeGameOver), timeoutReSpawn / 2);
                return;
            }
            Invoke(nameof(Reposition), timeoutReSpawn / 2);
        }

        void Reposition()
        {
            //Debug.Log("Respawn Player");
            var transform1 = transform;
            transform1.position = getPlayerSettings.spawnPosition;
            transform1.rotation = getPlayerSettings.spawnRotation;
            m_GamePlayManager.OnEventReSpawnEnemiesMaster();
            Tools.ToggleChildren(transform1);
            getPlayerSettings.actualFuel = m_GameSettings.startFuel;
            OnEventPlayerMasterRespawn();
            Invoke(nameof(ReSpawn), timeoutReSpawn);
        }
        void ReSpawn()
        {
            m_GamePlayManager.OnEventActivateEnemiesMaster();
            isPlayerDead = false;
            playerMovementStatus = MovementStatus.None;
        }



        void LogGamePlay(Component component)
        {
            var walls = component.GetComponentInParent<WallsMaster>();
            var enemies = component.GetComponentInParent<ObstacleMaster>();
            var bullet = component.GetComponent<BulletEnemy>();

            if (walls != null)
            {
                m_GamePlaySettings.playerDieWall += 1;
            }
            if (bullet != null)
            {
                m_GamePlaySettings.playerDieBullet += 1;
            }
            if (enemies != null && enemies is CollectiblesMaster or EffectAreaMaster)
            {
                GamePlayManager.AddResultList(m_GamePlaySettings.hitEnemiesResultsList, getPlayerSettings, enemies.enemy, 1, CollisionType.Collected);
            }
            else if (enemies != null)
            {
                GamePlayManager.AddResultList(m_GamePlaySettings.hitEnemiesResultsList, getPlayerSettings, enemies.enemy, 1, CollisionType.Collider);
            }
        }

        #region Calls
        internal void OnEventPlayerMasterHit()
        {
            KillPlayer();
            EventPlayerMasterHit?.Invoke();
            TryRespawn();
        }
        void OnEventPlayerMasterRespawn()
        {
            m_GamePlayManager.playerDead = false;
            EventPlayerMasterRespawn?.Invoke();
        }

        internal void OnEventPlayerMasterControllerMovement(Vector2 dir)
        {
            EventPlayerMasterControllerMovement?.Invoke(dir);
        }
  #endregion

        void MyDebugStart()
        {
            if (!m_GameManager.debugMode) return;
            transform.position = Vector3.zero;
        }
    }
}
