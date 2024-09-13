using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public sealed class BossMaster : EnemiesMaster
    {
        public bool IsEmerge { get; set; }

        #region Delagates & Events
        public delegate void BossGenericHandler();
        public event BossGenericHandler EventBossEmerge;
        public event BossGenericHandler EventBossSubmerge;

        #endregion
        
        private void Start()
        {
            GamePlayBossManager.instance.SetBoss(this);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            GamePlayManagerRef.EventGameReload += ReloadBoss;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamePlayManagerRef.EventGameReload -= ReloadBoss;
        }
        
        private void ReloadBoss()
        {
            GamePlayBossManager.instance.SetBoss(this);
            var behaviors = GetComponent<BossBehavior>();
            behaviors.StartBehavior();
            gameObject.transform.localScale = Vector3.one;
        }

        protected override void AttemptKillObstacle(PlayerMaster playerMaster)
        {
            base.AttemptKillObstacle(playerMaster);
            playerMaster.OnEventPlayerMasterStopDecoyFuel(true);
        }

        internal void OnEventBossSubmerge()
        {
            EventBossSubmerge?.Invoke();
        }

        internal void OnEventBossEmerge()
        {
            EventBossEmerge?.Invoke();
        }
    }
}