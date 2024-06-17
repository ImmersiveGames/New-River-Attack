using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Strategies
{
    public class DefaultChangeBehaviorStrategy : IChangeBehaviorStrategy
    {
        public async Task ChangeBehaviorAsync(IBehavior currentBehavior, BossBehavior bossBehavior,
            CancellationTokenSource cancellationTokenSource)
        {
            if (currentBehavior != null)
            {
                await currentBehavior.EnterAsync(cancellationTokenSource.Token).ConfigureAwait(false);

                if (currentBehavior.SubBehaviors.Length > 0)
                {
                    var subBehaviorManager = new BehaviorManager(bossBehavior);
                    subBehaviorManager.AddBehavior(currentBehavior.SubBehaviors);
                    await subBehaviorManager.ChangeBehaviorAsync(currentBehavior.SubBehaviors[0].Name).ConfigureAwait(false);
                    currentBehavior.SubBehaviorManager = subBehaviorManager;
                }
                else
                {
                    currentBehavior.SubBehaviorManager = null;
                }

                currentBehavior.Initialized = true;
            }
        }
    }
}