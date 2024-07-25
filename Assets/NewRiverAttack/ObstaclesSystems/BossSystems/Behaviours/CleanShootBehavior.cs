using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.Utils;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class CleanShootBehavior : Behavior
    {
        private BossBehavior BossBehavior { get; }
        private readonly BehaviorManager _behaviorManager;
        private PlayerMaster PlayerMaster { get; }
        private readonly object[] _dataShoot;
        
        private readonly BossCleanShoot _bossCleanShoot;
        public CleanShootBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors,params object[] data) : base(subBehaviors)
        {
            _behaviorManager = behaviorManager;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = behaviorManager.BossBehavior.PlayerMaster;
            _bossCleanShoot = BossBehavior.GetComponent<BossCleanShoot>();
            _dataShoot = data;
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                var maxShoot = (int)(_dataShoot[0] ?? 5);
             
                if (_bossCleanShoot != null) 
                    _bossCleanShoot.SetShoots(maxShoot);
                _bossCleanShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossCleanShoot.UpdateCadenceShoot();
                _bossCleanShoot.StartShoot();
            }).ConfigureAwait(false);

        }
        
        public override async void UpdateAsync(CancellationToken token)
        {
            base.UpdateAsync(token);
            await UnityMainThreadDispatcher.EnqueueAsync(async () =>
            {
                if (_bossCleanShoot && _bossCleanShoot.ShouldBeShoot)
                {
                    _bossCleanShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
                }
            }).ConfigureAwait(false);
            /*Debug.Log($"Update Shoot Finalize: {Finalized}");
            if (!_bossCleanShoot.EndCycle) return;
            await Task.Delay(100, token).ConfigureAwait(false);
            Finalized = true;
            await NextBehavior(_behaviorManager.SubBehaviorManager).ConfigureAwait(false);*/
            
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            _bossCleanShoot.StopShoot();
            await base.ExitAsync(token).ConfigureAwait(false);

        }
    }
}