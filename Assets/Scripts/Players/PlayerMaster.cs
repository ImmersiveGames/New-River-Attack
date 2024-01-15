using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        bool m_Invulnerability;
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
        //internal bool inEffectArea;
        [SerializeField]
        internal bool inPowerUp;
        internal int nextScoreForLive;
        

        //Animator m_Animator;
        GamePlayingLog m_GamePlayingLog;
        GamePlayManager m_GamePlayManager;
        GameManager m_GameManager;
        GameSettings m_GameSettings;
        #region Delagetes
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventPlayerMasterHit;
        public event GeneralEventHandler EventPlayerMasterUpdateSkin;
        public event GeneralEventHandler EventPlayerMasterRespawn;
        public delegate void BoolEventHandler(bool active);
        public event BoolEventHandler EventPlayerMasterBossHit;
        public delegate void ControllerEventHandler(Vector2 dir);
        public event ControllerEventHandler EventPlayerMasterControllerMovement;
        #endregion

        #region UNITYMETHOD
        void Awake()
        {
            //m_Animator = GetComponent<Animator>();
            m_GamePlayManager = GamePlayManager.instance;
            m_GameSettings = GamePlayManager.getGameSettings;
            m_GameManager = GameManager.instance;
            m_GamePlayingLog = m_GamePlayManager.gamePlayingLog;
            playersInputActions = m_GamePlayManager.inputSystem;
            /*playersInputActions.Player.Enable();
            playersInputActions.UI_Controlls.Disable();*/
        }

        void OnEnable()
        {
            m_GamePlayManager.EventStartRapidFire += () => inPowerUp = true;
            m_GamePlayManager.EventEndRapidFire += () => inPowerUp = false;
            OnEventPlayerMasterUpdateSkin();
        }

        void Start()
        {
            PlayerStartSetup();
            MyDebugStart();
        }
        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"coll: {other.GetComponent<LevelFinish>()}");
            if (other.GetComponentInParent<PlayerMaster>() || other.GetComponent<BulletPlayer>()
                || other.GetComponent<BulletPlayerBomb>() || other.GetComponentInParent<CollectiblesMaster>() != null
                || other.GetComponentInParent<EffectAreaAnimator>() || other.GetComponent<LevelFinish>()
                || other.GetComponent<LevelChangeBGM>()) return;
            
            if (m_GamePlayManager.getGodMode || m_Invulnerability) return;
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

        /*public Animator GetPlayerAnimator()
        {
            return m_Animator;
        }*/

        void PlayerStartSetup()
        {
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
            // não reiniciar as vidas no modo missão
            if (m_GameManager.gameModes == GameManager.GameModes.Mission)
            {
                if (getPlayerSettings.lives == 0)
                    getPlayerSettings.lives = 3;
                m_GamePlayManager.OnEventUpdateLives(getPlayerSettings.lives);
                return;
            }
            getPlayerSettings.lives = m_GameSettings.startLives;
            m_GamePlayManager.OnEventUpdateLives(getPlayerSettings.lives);
        }

        void KillPlayer()
        {
            m_GamePlayManager.playerDead = !m_GamePlayManager.bossFight;
            isPlayerDead = !m_GamePlayManager.bossFight;
            playerMovementStatus = m_GamePlayManager.bossFight ? MovementStatus.None : MovementStatus.Paused;
            m_Invulnerability = true;
            //TODO: Ativar animação de invencibilidade do jogador
            CameraShake.ShakeCamera(shakeIntensity,shakeTime);
            m_GamePlayManager.OnEventEnemiesMasterKillPlayer();
            if (m_GamePlayManager.bossFight)
            {
                OnEventPlayerMasterBossHit(true);
                return;
            }
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
            transform1.position = m_GamePlayManager.bossFight ? transform1.position : getPlayerSettings.spawnPosition;
            transform1.rotation = m_GamePlayManager.bossFight ? transform1.rotation : getPlayerSettings.spawnRotation;
            m_GamePlayManager.OnEventReSpawnEnemiesMaster();
            Tools.ToggleChildren(transform1);
            getPlayerSettings.actualFuel = m_GameSettings.startFuel;
            OnEventPlayerMasterRespawn();
            float timeToRespawn = m_GamePlayManager.bossFight ? timeoutReSpawn/2 : timeoutReSpawn;
            Invoke(nameof(ReSpawn), timeToRespawn);
        }
        void ReSpawn()
        {
            m_GamePlayManager.OnEventActivateEnemiesMaster();
            if (m_GamePlayManager.bossFight)
            {
                OnEventPlayerMasterBossHit(false);
            }
            isPlayerDead = false;
            playerMovementStatus = MovementStatus.None;
            m_Invulnerability = false;
        }

        void LogGamePlay(Component component)
        {
            var walls = component.GetComponentInParent<WallsMaster>();
            var enemies = component.GetComponentInParent<ObstacleMaster>();
            var bullet = component.GetComponent<BulletEnemy>();
            
            if (walls != null)
            {
                m_GamePlayingLog.playerDieWall += 1;
                GameSteamManager.AddStat("stat_CrashPlayer",1,false);
                GameSteamManager.UnlockAchievement("ACH_CRASH_PLAYER_WALL");
            }
            if (bullet != null)
            {
                m_GamePlayingLog.playerDieBullet += 1;
            }
            if (enemies != null && enemies is CollectiblesMaster or EffectAreaMaster)
            {
                GamePlayManager.AddResultList(m_GamePlayingLog.hitEnemiesResultsList, getPlayerSettings, enemies.enemy, 1, CollisionType.Collected);
            }
            else if (enemies != null)
            {
                GameSteamManager.AddStat("stat_CrashPlayer",1,false);
                GamePlayManager.AddResultList(m_GamePlayingLog.hitEnemiesResultsList, getPlayerSettings, enemies.enemy, 1, CollisionType.Collider);
            }
            GameSteamManager.StoreStats();
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
        void OnEventPlayerMasterBossHit(bool active)
        {
            EventPlayerMasterBossHit?.Invoke(active);
        }
        internal void OnEventPlayerMasterUpdateSkin()
        {
            EventPlayerMasterUpdateSkin?.Invoke();
        }
  #endregion

        void MyDebugStart()
        {
            if (!m_GameManager.debugMode) return;
            transform.position = Vector3.zero;
        }
        
    }
}
