using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class TesteSubs : Behavior
    {
        public TesteSubs(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(nameof(TesteSubs), subBehaviors)
        {
            
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            
            await Task.Delay(100).ConfigureAwait(false);

            Initialized = true;

        }
    }
    public class TesteSubs2 : Behavior
    {
        public TesteSubs2(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(nameof(TesteSubs2), subBehaviors)
        {
        }
        public override async Task EnterAsync(CancellationToken token)
        { await base.EnterAsync(token).ConfigureAwait(false);
            
            await Task.Delay(100).ConfigureAwait(false);

            Initialized = true;

        }
    }
}