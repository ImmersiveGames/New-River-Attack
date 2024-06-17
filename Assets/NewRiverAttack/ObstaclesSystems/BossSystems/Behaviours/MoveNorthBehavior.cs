using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Strategies;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveNorthBehavior : Behavior
    {
        private readonly BossBehavior _bossBehavior;
        private int _nextSubBehaviorIndex = 0;

        public MoveNorthBehavior(IBehavior[] subBehaviors, BossBehavior bossBehavior)
            : base("MoveNorthBehavior", subBehaviors,
                new DefaultChangeBehaviorStrategy(),
                new DefaultUpdateStrategy()
            )
        {
            _bossBehavior = bossBehavior;
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<MoveNorthBehavior>("Move North Behavior Entered");
        }

        public override async Task UpdateAsync(CancellationToken token)
        {
            await base.UpdateAsync(token).ConfigureAwait(false);

            // Verificar se há sub comportamentos para gerenciar
            if (SubBehaviors is { Length: > 0 })
            {
                if (_nextSubBehaviorIndex >= SubBehaviors.Length)
                {
                    // Todos os sub comportamentos foram executados, finalizar o comportamento principal
                    await FinalizeAsync().ConfigureAwait(false);
                }
                else
                {
                    // Obter o próximo sub comportamento
                    var currentSubBehavior = SubBehaviors[_nextSubBehaviorIndex];

                    // Verificar se o sub comportamento precisa ser finalizado
                    if (!currentSubBehavior.Finalized)
                    {
                        await _bossBehavior.GetBehaviorManager.FinalizeSubBehaviorAsync(currentSubBehavior).ConfigureAwait(false);
                    }
                    else
                    {
                        // Avançar para o próximo sub comportamento
                        _nextSubBehaviorIndex++;
                    }
                }
            }
            else
            {
                // Não há sub comportamentos, finalizar o comportamento principal
                await FinalizeAsync().ConfigureAwait(false);
            }
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            DebugManager.Log<MoveNorthBehavior>("Move North Behavior Exited");
        }
    }
}