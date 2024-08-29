using System.Collections.Generic;
using ImmersiveGames;
using NewRiverAttack.SteamGameManagers;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class PlayerMasterOld : ObjectMasterOLD
    {
        [SerializeField] internal PlayerSettings getPlayerSettings;
        [Header("Player Destroy Settings")]
        [SerializeField]
        internal bool isPlayerDead;

        private bool _invulnerability;
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
        [SerializeField]
        internal bool inEffectArea;
        [SerializeField]
        internal bool inPowerUp;
        internal int nextScoreForLive;
        

        //Animator m_Animator;
        private GamePlayingLog _gamePlayingLog;
        private GamePlayManager _gamePlayManager;
        private GameManager _gameManager;
        private GameSettings _gameSettings;
        
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
            _gamePlayManager = GamePlayManager.instance;
            _gameSettings = GamePlayManager.getGameSettings;
            _gameManager = GameManager.instance;
            _gamePlayingLog = _gamePlayManager.gamePlayingLog;
            playersInputActions = _gameManager.inputSystem;
            /*playersInputActions.Player.Enable();
            playersInputActions.UI_Controlls.Disable();*/
        }

        private void OnEnable()
        {
            _gamePlayManager.EventStartRapidFire += () => inPowerUp = true;
            _gamePlayManager.EventEndRapidFire += () => inPowerUp = false;
            OnEventPlayerMasterUpdateSkin();
        }

        private void Start()
        {
            PlayerStartSetup();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<PlayerMasterOld>() || other.GetComponent<BulletPlayer>()
                || other.GetComponent<BulletPlayerBomb>() || other.GetComponentInParent<CollectiblesMasterOld>() != null
                || other.GetComponentInParent<EffectAreaAnimator>() || other.GetComponent<LevelCheck>()) return;
            
            if (_gamePlayManager.getGodMode || _invulnerability) return;
            LogGamePlay(other);
            //return;
            OnEventPlayerMasterHit();
        }
  #endregion

        public bool ShouldPlayerBeReady => isPlayerDead == false && playerMovementStatus != MovementStatus.Paused;

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
            _gamePlayManager.OnEventUpdateScore(getPlayerSettings.score);
            getPlayerSettings.distance = 0;
            var distInt = Mathf.FloorToInt(getPlayerSettings.distance);
            _gamePlayManager.OnEventUpdateDistance(distInt);
            //getPlayerSettings.wealth = 0;
            _gamePlayManager.OnEventUpdateRefugees(getPlayerSettings.wealth);
            getPlayerSettings.bombs = _gameSettings.startBombs;
            _gamePlayManager.OnEventUpdateBombs(getPlayerSettings.bombs);
            getPlayerSettings.actualFuel = _gameSettings.startFuel;
            // não reiniciar as vidas no modo missão
            if (_gameManager.gameModes == GameManager.GameModes.Mission)
            {
                if (getPlayerSettings.lives <= 1)
                    getPlayerSettings.lives = 3;
                _gamePlayManager.OnEventUpdateLives(getPlayerSettings.lives);
                return;
            }
            getPlayerSettings.lives = _gameSettings.startLives;
            _gamePlayManager.OnEventUpdateLives(getPlayerSettings.lives);
            inEffectArea = false;
            inPowerUp = false;
        }

        private void KillPlayer()
        {
            _gamePlayManager.playerDead = !_gamePlayManager.bossFight;
            isPlayerDead = !_gamePlayManager.bossFight;
            playerMovementStatus = _gamePlayManager.bossFight ? MovementStatus.None : MovementStatus.Paused;
            _invulnerability = true;
            //TODO: Ativar animação de invencibilidade do jogador
            CameraShake.ShakeCamera(shakeIntensity,shakeTime);
            _gamePlayManager.OnEventEnemiesMasterKillPlayer();
            inEffectArea = false;
            inPowerUp = false;
            if (_gamePlayManager.bossFight)
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
            _gameManager.ChangeState(new GameStateGameOver());
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
            transform1.position = _gamePlayManager.bossFight ? transform1.position : getPlayerSettings.spawnPosition;
            transform1.rotation = _gamePlayManager.bossFight ? transform1.rotation : getPlayerSettings.spawnRotation;
            _gamePlayManager.OnEventReSpawnEnemiesMaster();
            Tools.ToggleChildren(transform1);
            getPlayerSettings.actualFuel = _gameSettings.startFuel;
            getPlayerSettings.cadenceShootPowerUp = 0;
            OnEventPlayerMasterRespawn();
            var timeToRespawn = _gamePlayManager.bossFight ? timeoutReSpawn/2 : timeoutReSpawn;
            
            Invoke(nameof(ReSpawn), timeToRespawn);
        }

        private void ReSpawn()
        {
            _gamePlayManager.OnEventActivateEnemiesMaster();
            if (_gamePlayManager.bossFight)
            {
                OnEventPlayerMasterBossHit(false);
            }
            isPlayerDead = false;
            playerMovementStatus = MovementStatus.None;
            _invulnerability = false;
        }

        private void LogGamePlay(Component component)
        {
            var walls = component.GetComponentInParent<WallsMaster>();
            var enemies = component.GetComponentInParent<ObstacleMasterOld>();
            var bullet = component.GetComponent<BulletEnemy>();
            
            if (walls != null)
            {
                _gamePlayingLog.playerDieWall += 1;
                //SteamGameManager.AddStat("stat_CrashPlayer",1,false);
                //SteamGameManager.UnlockAchievement("ACH_CRASH_PLAYER_WALL");
            }
            if (bullet != null)
            {
                _gamePlayingLog.playerDieBullet += 1;
            }
            if (enemies != null && enemies is CollectiblesMasterOld or EffectAreaMasterOld)
            {
                GamePlayManager.AddResultList(_gamePlayingLog.GetEnemiesResult(), getPlayerSettings, enemies.enemy, 1, CollisionType.Collected);
            }
            else if (enemies != null)
            {
                //SteamGameManager.AddStat("stat_CrashPlayer",1,false);
                GamePlayManager.AddResultList(_gamePlayingLog.GetEnemiesResult(), getPlayerSettings, enemies.enemy, 1, CollisionType.Collider);
            }
            //SteamGameManager.StoreStats();
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
            _gamePlayManager.playerDead = false;
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
