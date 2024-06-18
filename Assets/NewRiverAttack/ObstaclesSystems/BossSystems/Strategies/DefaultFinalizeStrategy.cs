using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers.Interfaces;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Strategies
{
    public class DefaultFinalizeStrategy : IFinalizeStrategy
    {
        public async Task FinalizeAsync(IBehavior behavior, CancellationToken cancellationToken)
        {
            if (behavior == null) return;

            // Finalizar sub comportamentos
            if (behavior.SubBehaviors.Length > 0)
            {
                var subManager = behavior.SubBehaviorManager;
                if (subManager != null)
                {
                    foreach (var subBehavior in behavior.SubBehaviors)
                    {
                        subBehavior.Finalized = true;
                        await subBehavior.ExitAsync(cancellationToken).ConfigureAwait(false);
                    }
                }
            }

            // Finalizar o comportamento atual
            behavior.Finalized = true;
            await behavior.ExitAsync(cancellationToken).ConfigureAwait(false);
        }

        public void FinalizeSubBehavior(IBehavior behavior)
        {
            if (behavior.SubBehaviors.Length <= 0) return;

            behavior.SubBehaviors[behavior.CurrentSubBehaviorIndex].Finalized = true;

            // Verificar se todos os sub comportamentos foram finalizados
            var allSubBehaviorsFinalized = behavior.SubBehaviors.All(subBehavior => subBehavior.Finalized);

            if (allSubBehaviorsFinalized)
            {
                behavior.Finalized = true;
            }
            else
            {
                // Avançar para o próximo sub comportamento
                behavior.CurrentSubBehaviorIndex++;
            }
        }
    }
}