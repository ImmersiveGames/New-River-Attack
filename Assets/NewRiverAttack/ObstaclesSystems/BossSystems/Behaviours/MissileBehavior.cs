using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MissileBehavior : Behavior
    {
        private BossBehavior BossBehavior { get; }
        private readonly BossShoot _bossShoot;
        private PlayerMaster PlayerMaster { get; }
        public MissileBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(nameof(MissileBehavior), subBehaviors)
        {
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
            _bossShoot = BossBehavior.GetComponent<BossMissileShoot>();
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                _bossShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossShoot.UpdateCadenceShoot();
                _bossShoot.StartShoot();
            }).ConfigureAwait(false);
            
            Initialized = true;

        }
        
        public override async Task UpdateAsync(CancellationToken token)
        {
            await base.UpdateAsync(token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                if (_bossShoot && _bossShoot.ShouldBeShoot)
                {
                    _bossShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
                }
            }).ConfigureAwait(false);
            
        }
        public override Task ExitAsync(CancellationToken token)
        {
            _bossShoot.StopShoot();
            return base.ExitAsync(token);
        }
        
    }
}