using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.Utils;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class EmergeBehavior : Behavior
    {
        private BossBehavior BossBehavior { get; }
        private readonly BehaviorManager _behaviorManager;
        public EmergeBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(subBehaviors)
        {
            _behaviorManager = behaviorManager;
            BossBehavior = behaviorManager.BossBehavior;
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            var animationTime = 0;
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                if (BossBehavior.BossMaster.IsEmerge) return;
                BossBehavior.BossMaster.IsEmerge = true;
                animationTime = (int)BossBehavior.GetComponent<BossAnimation>().GetSubmergeTime();
                BossBehavior.BossMaster.OnEventBossEmerge();

            }).ConfigureAwait(false);
            await Task.Delay(animationTime * 1000, token).ConfigureAwait(false);
            Finalized = true;
        }
        
        public override Task ExitAsync(CancellationToken token)
        {
            _ = _behaviorManager.ChangeBehaviorAsync(EnumNameBehavior.MoveNorthBehavior.ToString());
            return Task.CompletedTask;
        }
    }
}