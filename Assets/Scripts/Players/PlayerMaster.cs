using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class PlayerMaster : ObjectMaster
    {
        [SerializeField] internal PlayerSettings getPlayerSettings;
        [Header("Player Destroy Settings")]
        [SerializeField]
        internal bool isPlayerDead;

        private bool m_Invulnerability;
        [SerializeField] private float timeoutReSpawn;
        [SerializeField] private GameObject deadParticlePrefab;
        [SerializeField] private float timeoutDestroyExplosion;
        [Header("Shake Camera")]
        [SerializeField]
        private float shakeIntensity;
        [SerializeField] private float shakeTime;

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
        private GamePlayingLog m_GamePlayingLog;
        private GamePlayManager m_GamePlayManager;
        private GameManager m_GameManager;
        private GameSettings m_GameSettings;
        
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

        private void Awake()
        {
            //m_Animator = GetComponent<Animator>();
            m_GamePlayManager = GamePlayManager.instance;
            m_GameSettings = GamePlayManager.getGameSettings;
            m_GameManager = GameManager.instance;
            m_GamePlayingLog = m_GamePlayManager.gamePlayingLog;
            playersInputActions = m_GameManager.inputSystem;
            /*playersInputActions.Player.Enable();
            playersInputActions.UI_Controlls.Disable();*/
        }

        private void OnEnable()
        {
            m_GamePlayManager.EventStartRapidFire += () => inPowerUp = true;
            m_GamePlayManager.EventEndRapidFire += () => inPowerUp = false;
            OnEventPlayerMasterUpdateSkin();
        }

        private void Start()
        {
            PlayerStartSetup();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<PlayerMaster>() || other.GetComponent<BulletPlayer>()
                || other.GetComponent<BulletPlayerBomb>() || other.GetComponentInParent<CollectiblesMaster>() != null
                || other.GetComponentInParent<EffectAreaAnimator>() || other.GetComponent<LevelCheck>()) return;
            
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

        private void PlayerStartSetup()
        {
            getPlayerSettings.spawnPosition = transform.position;
            getPlayerSettings.cadenceShootPowerUp = 0;
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
                if (getPlayerSettings.lives <= 1)
                    getPlayerSettings.lives = 3;
                m_GamePlayManager.OnEventUpdateLives(getPlayerSettings.lives);
                return;
            }
            getPlayerSettings.lives = m_GameSettings.startLives;
            m_GamePlayManager.OnEventUpdateLives(getPlayerSettings.lives);
        }

        private void KillPlayer()
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

        private void ChangeGameOver()
        {
            m_GameManager.ChangeState(new GameStateGameOver());
        }

        private void TryRespawn()
        {
            if (getPlayerSettings.lives <= 0)
            {
                Invoke(nameof(ChangeGameOver), timeoutReSpawn / 2);
                return;
            }
            Invoke(nameof(Reposition), timeoutReSpawn / 2);
        }

        private void Reposition()
        {
            //Debug.Log("Respawn Player");
            var transform1 = transform;
            transform1.position = m_GamePlayManager.bossFight ? transform1.position : getPlayerSettings.spawnPosition;
            transform1.rotation = m_GamePlayManager.bossFight ? transform1.rotation : getPlayerSettings.spawnRotation;
            m_GamePlayManager.OnEventReSpawnEnemiesMaster();
            Tools.ToggleChildren(transform1);
            getPlayerSettings.actualFuel = m_GameSettings.startFuel;
            getPlayerSettings.cadenceShootPowerUp = 0;
            OnEventPlayerMasterRespawn();
            var timeToRespawn = m_GamePlayManager.bossFight ? timeoutReSpawn/2 : timeoutReSpawn;
            
            Invoke(nameof(ReSpawn), timeToRespawn);
        }

        private void ReSpawn()
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

        private void LogGamePlay(Component component)
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
                GamePlayManager.AddResultList(m_GamePlayingLog.GetEnemiesResult(), getPlayerSettings, enemies.enemy, 1, CollisionType.Collected);
            }
            else if (enemies != null)
            {
                GameSteamManager.AddStat("stat_CrashPlayer",1,false);
                GamePlayManager.AddResultList(m_GamePlayingLog.GetEnemiesResult(), getPlayerSettings, enemies.enemy, 1, CollisionType.Collider);
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

        private void OnEventPlayerMasterRespawn()
        {
            m_GamePlayManager.playerDead = false;
            EventPlayerMasterRespawn?.Invoke();
        }

        internal void OnEventPlayerMasterControllerMovement(Vector2 dir)
        {
            EventPlayerMasterControllerMovement?.Invoke(dir);
        }

        private void OnEventPlayerMasterBossHit(bool active)
        {
            EventPlayerMasterBossHit?.Invoke(active);
        }
        internal void OnEventPlayerMasterUpdateSkin()
        {
            EventPlayerMasterUpdateSkin?.Invoke();
        }
  #endregion
        
    }
}
