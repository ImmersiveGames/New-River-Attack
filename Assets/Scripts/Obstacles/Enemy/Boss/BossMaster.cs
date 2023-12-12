using System;
using UnityEngine;
namespace RiverAttack
{
    public class BossMaster: ObstacleMaster
    {
        const float HEIGHT_Y = 0.3f;
        
        [SerializeField] int bossHp;
        [SerializeField] int bossCycles;
        EnemiesBossScriptable m_BossScriptable;
        internal bool invulnerability;

        internal BattleBossSubState actualPosition;

        internal Transform targetPlayer;
        [SerializeField] internal float distanceTarget = 20.0f;
        
        #region Events
        protected internal event GeneralEventHandler EventBossHit;
        protected internal event GeneralEventHandler EventBossEmerge;
  #endregion
        internal override void Awake()
        {
            base.Awake();
            m_BossScriptable = enemy as EnemiesBossScriptable;
            bossHp = m_BossScriptable!.maxHp;
            bossCycles = m_BossScriptable!.maxCycles;
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
            if (other.GetComponent<Bullets>())
            {
                ComponentToKill(other.GetComponent<BulletPlayer>(), CollisionType.Shoot);
                ComponentToKill(other.GetComponent<BulletPlayerBomb>(), CollisionType.Bomb);
            }
            
            //GamePlayManager.instance.OnEventOtherEnemiesKillPlayer();
        }

        protected override void ComponentToKill(Component other, CollisionType collisionType)
        {
            Debug.Log($" Coliders: {other}, {shouldObstacleBeReady}, {enemy.canDestruct}");
            Debug.Log($" ColiderType: {collisionType}");
            if (other == null) return;
            playerMaster = WhoHit(other);
            if (other.GetComponent<Bullets>())
            {
                var bullet = other.GetComponent<BulletPlayer>();
                var bomb = other.GetComponent<BulletPlayerBomb>();
                DamageBoss(bullet.powerFire);
            }
            //TODO: Organizar o esquema de ciclos e HP do Boss.
            
            //OnEventObstacleMasterHit(); <= Efetivamente destroi o obstaculo
            //OnEventObstacleScore(playerMaster.getPlayerSettings); <= Envia para A HUD os resulfados
            //ShouldSavePoint(playerMaster.getPlayerSettings); ,= Verifica se salva a posição do player
            //GamePlayManager.AddResultList(gamePlayingLog.hitEnemiesResultsList, playerMaster.getPlayerSettings, enemy, 1, collisionType);
            ShouldFinishGame(); //<= Verifica se o jogo terminsou
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
                _ => throw new ArgumentOutOfRangeException(nameof(positionBoss), positionBoss, null)
            };
            Invoke(nameof(OnEventBossEmerge), 2f);
        }

        void DamageBoss(int damage)
        {
            bossHp -= damage;
            Debug.Log($"Acertou um tiro? {damage}");
            OnEventBossHit();
            //Checar as mudanças de ciclo
        }
        protected virtual void OnEventBossHit()
        {
            EventBossHit?.Invoke();
        }
        public void OnEventBossEmerge()
        {
            EventBossEmerge?.Invoke();
        }
    }
}
