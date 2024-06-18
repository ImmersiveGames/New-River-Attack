using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.BossSystems.Strategies;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveNorthBehavior : Behavior
    {
        private readonly BossBehavior _bossBehavior;

        public MoveNorthBehavior(IBehavior[] subBehaviors, BossBehavior bossBehavior)
            : base("MoveNorthBehavior", subBehaviors,
                new DefaultChangeBehaviorStrategy(),
                new DefaultUpdateStrategy(),
                new DefaultFinalizeStrategy())
        {
            _bossBehavior = bossBehavior;
        }

        public override async Task UpdateAsync(CancellationToken token)
        {
            await base.UpdateAsync(token).ConfigureAwait(false);
        }
    }
}