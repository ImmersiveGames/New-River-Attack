using System;
using UnityEngine;
namespace RiverAttack
{
    public class BossMaster: ObstacleMaster
    {
        const float HEIGHT_Y = 0.3f;
        [Header("Boss Fight")]
        int m_BossHp;
        int m_BossCycles;

        internal BattleBossSubState actualPosition;

        internal Transform targetPlayer;
        [SerializeField] internal float distanceTarget = 20.0f;

        EnemiesBossScriptable m_BossScriptable;


        #region Events
        public delegate void BossCyclesHandler(Transform spawnPosition);

        protected internal event BossCyclesHandler EventSmokeSpawn;
        protected internal event GeneralEventHandler EventBossHit;
        protected internal event GeneralEventHandler EventBossEmerge;
        protected internal event GeneralEventHandler EventBossSubmerge;
        protected internal event GeneralEventHandler EventBossDeath;
  #endregion
        internal override void Awake()
        {
            base.Awake();
            m_BossScriptable = enemy as EnemiesBossScriptable;
            m_BossHp = m_BossScriptable!.maxHp;
            m_BossCycles = m_BossScriptable!.maxCycles;
            GamePlayManager.instance.bossMaster = this;
            actualPosition = BattleBossSubState.Top;
        }

        void Start()
        {
            targetPlayer = PlayerManager.instance.GetTransformFirstPlayer();
        }

        internal override void OnTriggerEnter(Collider other)
        {
            //Debug.Log($" Coliders: {other}, {shouldObstacleBeReady}, {enemy.canDestruct}");
            if (other == null || !shouldObstacleBeReady || !enemy.canDestruct) return;
            if (!other.GetComponent<Bullets>() || other.GetComponent<BulletBoss>()) return;
            
            ComponentToKill(other.GetComponent<BulletPlayer>(), CollisionType.Shoot);
            ComponentToKill(other.GetComponent<BulletPlayerBomb>(), CollisionType.Bomb);
            //GamePlayManager.instance.OnEventOtherEnemiesKillPlayer();
        }

        public bool shouldBeBossBattle
        {
            get
            {
                return shouldObstacleBeReady || !GamePlayManager.instance.bossFightPause;
            }
        }

        protected override void ComponentToKill(Component other, CollisionType collisionType)
        {
            if (other == null) return;
            playerMaster = WhoHit(other);
            Bullets bullet = null;
            switch (collisionType)
            {
                case CollisionType.Shoot:
                    bullet = other.GetComponent<BulletPlayer>();
                    break;
                case CollisionType.Bomb:
                    bullet = other.GetComponent<BulletPlayerBomb>();
                    break;
                case CollisionType.Collider:
                    break;
                case CollisionType.Collected:
                    break;
                case CollisionType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(collisionType), collisionType, null);
            }
            if (bullet == null) return;
            
            DamageBoss(bullet.powerFire);

            //TODO: Organizar o esquema de ciclos e HP do Boss.
            //Debug.Log($" Coliders: {other}, {bullet}, {enemy.canDestruct}");
            //OnEventObstacleMasterHit(); <= Efetivamente destroi o obstaculo
            //OnEventObstacleScore(playerMaster.getPlayerSettings); <= Envia para A HUD os resulfados
            //ShouldSavePoint(playerMaster.getPlayerSettings); ,= Verifica se salva a posição do player
            //GamePlayManager.AddResultList(gamePlayingLog.hitEnemiesResultsList, playerMaster.getPlayerSettings, enemy, 1, collisionType);
            //ShouldFinishGame(); //<= Verifica se o jogo terminsou
        }

        public void MoveBoss(BattleBossSubState positionBoss)
        {
            actualPosition = positionBoss;
            var transform1 = transform;
            var targetPosition = targetPlayer.position;
            transform1.position = positionBoss switch
            {
                BattleBossSubState.Top => new Vector3(targetPosition.x, HEIGHT_Y, targetPosition.z + distanceTarget),
                BattleBossSubState.Base => new Vector3(targetPosition.x, HEIGHT_Y, GamePlayManager.LimitZBottom + 5f),
                BattleBossSubState.Left => new Vector3(targetPosition.x - distanceTarget, HEIGHT_Y, GamePlayManager.LimitZBottom + (targetPosition.z / 2)),
                BattleBossSubState.Right => new Vector3(targetPosition.x + distanceTarget, HEIGHT_Y, GamePlayManager.LimitZBottom + (targetPosition.z / 2)),
                BattleBossSubState.Dead => transform1.position,
                _ => throw new ArgumentOutOfRangeException(nameof(positionBoss), positionBoss, null)
            };
            Invoke(nameof(OnEventBossEmerge), 2f);

        }
        

        internal BossMissileShoot GetBossMissileShoot()
        {
            return GetComponent<BossMissileShoot>();
        }
        internal BossMinesShoot GetBossMines()
        {
            return GetComponent<BossMinesShoot>();
        }
        internal BossGasStationDrop GetBossGasStationDrop()
        {
            return GetComponent<BossGasStationDrop>();
        }

        void DamageBoss(int damage)
        {
            OnEventBossHit();
            m_BossHp -= damage;
            Debug.Log($"Cycles: {m_BossCycles} - HP:{m_BossHp} - State {GameManager.instance.currentGameState}");
            
            if (m_BossHp > 0) return; // Enquanto tiver HP
            if (m_BossCycles <= 0)
            {
                isDestroyed = true;
                GameAudioManager.instance.StopBGM(LevelTypes.Boss);
                OnEventBossDeath();
                gamePlayManager.readyToFinish = true;
                Invoke(nameof(BeatGame),7f);
                return;
            }
            m_BossCycles--;
            var transformPoint = GetComponentInChildren<BossSmokePoints>().FindChildWithoutChild();
            if (transformPoint != default)
            {
                OnEventSmokeSpawn(transformPoint);
            }
            
            //Recarrega o ciclo
            m_BossHp = m_BossScriptable!.maxHp;
            var gameSubState = GameManager.instance.currentGameState as GameStatePlayGameBoss;
            gameSubState?.FinishBehavior();

            //Checar as mudanças de ciclo
        }

        void BeatGame()
        {
            GameManager.instance.ChangeState(new GameStateEndGame(), "EndGameCredits");
        }

        internal void BossInvulnerability(bool active)
        {
            myColliders ??= GetComponentsInChildren<Collider>();
            foreach (var bossCollider in myColliders)
            {
                bossCollider.enabled = !active;
            }
        }
        protected virtual void OnEventBossHit()
        {
            EventBossHit?.Invoke();
        }
        internal void OnEventBossEmerge()
        {
            EventBossEmerge?.Invoke();
        }
        internal void OnEventBossSubmerge()
        {
            EventBossSubmerge?.Invoke();
        }
        protected virtual void OnEventSmokeSpawn(Transform spawnPosition)
        {
            EventSmokeSpawn?.Invoke(spawnPosition);
        }
        protected virtual void OnEventBossDeath()
        {
            EventBossDeath?.Invoke();
        }
    }
}
