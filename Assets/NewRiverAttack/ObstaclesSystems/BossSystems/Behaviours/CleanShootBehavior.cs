using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.Utils;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public abstract class CleanShootBehavior : Behavior
    {
        private BossBehavior BossBehavior { get; }
        private PlayerMaster PlayerMaster { get; }
        private readonly object[] _dataShoot;
        
        private readonly BossCleanShoot _bossCleanShoot;
        // Parametro 0 = cadência dos misseis
        // Parametro 1 = cycles
        protected CleanShootBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors,params object[] data) : base(subBehaviors)
        {
            BossBehavior = behaviorManager.BossBehavior;
            //PlayerMaster = behaviorManager.BossBehavior.PlayerMaster;
            _bossCleanShoot = BossBehavior.GetComponent<BossCleanShoot>();
            _dataShoot = data;
        }
        
        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            await Task.Delay(100, token).ConfigureAwait(false);
            await MainThreadDispatcher.EnqueueAsync(() =>
            {
                //Debug.Log($"DATA: 0:{_dataShoot[0]}, 1:{_dataShoot[1]}");
                var cadence = (float)(_dataShoot[0] ?? 1f);
                var repeat = (int)(_dataShoot[1] ?? 10);
                
                if (_bossCleanShoot == null) return;
                _bossCleanShoot.SetShoots(cadence,repeat);
                //_bossCleanShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossCleanShoot.UpdateCadenceShoot();
                _bossCleanShoot.StartShoot();
            }).ConfigureAwait(false);

        }
        
        public override async void UpdateAsync(CancellationToken token)
        {
            base.UpdateAsync(token);
            await MainThreadDispatcher.EnqueueAsync(async () =>
            {
                if (_bossCleanShoot && _bossCleanShoot.ShouldBeShoot && !Finalized)
                {
                    //_bossCleanShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
                }
                
                //EndCycle
                if (_bossCleanShoot.timesRepeat > 0 && Finalized) return;
                await Task.Delay(100, token).ConfigureAwait(false);
                Finalized = true;
            }).ConfigureAwait(false);
            
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            _bossCleanShoot.StopShoot();
            await base.ExitAsync(token).ConfigureAwait(false);

        }
    }
}