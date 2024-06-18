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
            }

            // Atualizar subcomportamentos, se houver
            if (currentBehavior.SubBehaviors.Length > 0)
            {
                var currentSubBehavior = currentBehavior.SubBehaviors[currentBehavior.CurrentSubBehaviorIndex];

                if (currentSubBehavior.Initialized && !currentSubBehavior.Finalized)
                {
                    await currentSubBehavior.UpdateAsync(cancellationToken).ConfigureAwait(false);
                }
                else if (currentSubBehavior.Finalized && currentBehavior.CurrentSubBehaviorIndex < currentBehavior.SubBehaviors.Length - 1)
                {
                    currentBehavior.CurrentSubBehaviorIndex++;
                    currentBehavior.SubBehaviors[currentBehavior.CurrentSubBehaviorIndex].Initialized = true;
                }
            }

            if (currentBehavior.Initialized && currentBehavior.Finalized)
            {
                // Chamar ExitAsync para finalizar o comportamento atual e seus subcomportamentos
                await currentBehavior.ExitAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}