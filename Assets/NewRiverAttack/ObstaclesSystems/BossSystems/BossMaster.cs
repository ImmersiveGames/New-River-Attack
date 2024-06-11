using DG.Tweening;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public sealed class BossMaster : EnemiesMaster
    {
        public Ease enterAnimation;
        
        private PlayerMaster _playerMaster;
        
        public delegate void BossMasterGeneralHandler();
        public event BossMasterGeneralHandler EventBossShowUp;

        private void Start()
        {
            GamePlayBossManager.instance.SetBoss(this);
        }
        

        internal void OnEventBossShowUp()
        {
            EventBossShowUp?.Invoke();
        }
    }
}