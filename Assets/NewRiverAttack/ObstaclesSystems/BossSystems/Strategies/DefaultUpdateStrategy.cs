using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Strategies
{
    public class DefaultUpdateStrategy : IUpdateStrategy
    {
        public async Task UpdateAsync(IBehavior currentBehavior, BehaviorManager manager, CancellationToken cancellationToken)
        {
            if (currentBehavior.Initialized && !currentBehavior.Finalized)
            {
                await currentBehavior.UpdateAsync(cancellationToken).ConfigureAwait(false);

                // Atualizar sub comportamentos, se houver
                if (currentBehavior.SubBehaviorManager != null && currentBehavior.SubBehaviorManager.CurrentBehavior.Initialized && !currentBehavior.SubBehaviorManager.CurrentBehavior.Finalized)
                {
                    await currentBehavior.SubBehaviorManager.UpdateAsync().ConfigureAwait(false);
                }
            }

            if (currentBehavior.Initialized && currentBehavior.Finalized)
            {
                // Chamar ExitAsync para finalizar o comportamento atual e seus sub comportamentos
                await currentBehavior.ExitAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}