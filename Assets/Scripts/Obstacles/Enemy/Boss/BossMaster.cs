using System;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    public class BossMaster: ObstacleMaster
    {
        [SerializeField] int bossHp;
        [SerializeField] int bossCycles;
        EnemiesBossScriptable m_BossScriptable;

        internal Transform targetPlayer;
        [SerializeField] internal float distanceTarget = 20.0f;

        #region Events
        protected internal event GeneralEventHandler EventBossHit;
  #endregion
        internal override void Awake()
        {
            base.Awake();
            m_BossScriptable = enemy as EnemiesBossScriptable;
            bossHp = m_BossScriptable!.maxHp;
            bossCycles = m_BossScriptable!.maxCycles;
            GamePlayManager.instance.bossMaster = this;
        }

        void Start()
        {
            targetPlayer = PlayerManager.instance.GetTransformFirstPlayer();
        }

        internal override void OnTriggerEnter(Collider other)
        {
            if (other == null || !shouldObstacleBeReady || !enemy.canDestruct) return;
            ComponentToKill(other.GetComponent<BulletPlayer>(), CollisionType.Shoot);
            ComponentToKill(other.GetComponent<BulletPlayerBomb>(), CollisionType.Bomb);
            if (other.GetComponent<BulletPlayer>()) return;
            ComponentToKill(other.GetComponentInParent<PlayerMaster>(), CollisionType.Collider);
            GamePlayManager.instance.OnEventOtherEnemiesKillPlayer();
        }

        protected override void ComponentToKill(Component other, CollisionType collisionType)
        {
            if (other == null) return;
            playerMaster = WhoHit(other);
            if (other is Bullets bullets)
            {
                DamageBoss(bullets.powerFire);
            }
            
            
            //TODO: Organizar o esquema de ciclos e HP do Boss.
            
            
            //OnEventObstacleMasterHit(); <= Efetivamente destroi o obstaculo
            //OnEventObstacleScore(playerMaster.getPlayerSettings); <= Envia para A HUD os resulfados
            //ShouldSavePoint(playerMaster.getPlayerSettings); ,= Verifica se salva a posição do player
            //GamePlayManager.AddResultList(gamePlayingLog.hitEnemiesResultsList, playerMaster.getPlayerSettings, enemy, 1, collisionType);
            ShouldFinishGame(); //<= Verifica se o jogo terminsou
        }

        internal void MoveBoss(BattleBossSubState positionBoss)
        {
            var transform1 = transform;
            var positionAhead = transform1.position;
            var targetPosition = targetPlayer.position;

            switch (positionBoss)
            {

                case BattleBossSubState.Top:
                    transform1.position = new Vector3(0f, 0.3f, targetPosition.z + distanceTarget);
                    break;
                case BattleBossSubState.Base:
                    break;
                case BattleBossSubState.Left:
                    break;
                case BattleBossSubState.Right:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(positionBoss), positionBoss, null);
            }
        }

        void DamageBoss(int damage)
        {
            bossHp -= damage;
            OnEventBossHit();
            //Checar as mudanças de ciclo
        }
        protected virtual void OnEventBossHit()
        {
            EventBossHit?.Invoke();
        }
    }
}
