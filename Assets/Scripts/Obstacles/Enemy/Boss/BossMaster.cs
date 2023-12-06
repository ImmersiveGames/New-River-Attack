using UnityEngine;
namespace RiverAttack
{
    public class BossMaster: ObstacleMaster
    {
        [SerializeField] int bossHp;
        [SerializeField] int bossCycles;
        EnemiesBossScriptable m_BossScriptable;

        #region Events
        protected internal event GeneralEventHandler EventBossHit;
  #endregion
        internal override void Awake()
        {
            base.Awake();
            m_BossScriptable = enemy as EnemiesBossScriptable;
            bossHp = m_BossScriptable!.maxHp;
            bossCycles = m_BossScriptable!.maxCycles;
        }

        internal override void OnEnable()
        {
            base.OnEnable();
            
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
