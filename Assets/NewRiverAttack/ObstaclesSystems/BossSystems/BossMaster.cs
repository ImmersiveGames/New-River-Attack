using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;

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