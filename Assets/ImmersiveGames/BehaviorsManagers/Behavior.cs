using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Strategies;

namespace ImmersiveGames.BehaviorsManagers
{
    public abstract class Behavior : IBehavior
    {
        public string Name { get; }
        public bool Initialized { get; set; }
        public bool Finalized { get; set; }
        protected bool IsPaused;
        public IBehavior[] SubBehaviors { get; }
        public BehaviorManager SubBehaviorManager { get; set; }
        public IBehavior NextBehavior { get; set; }
        public IChangeBehaviorStrategy ChangeBehaviorStrategy { get; }
        public IUpdateStrategy UpdateStrategy { get; }
        public int CurrentSubBehaviorIndex { get; set; }

        protected Behavior(string name, IBehavior[] subBehaviors, IChangeBehaviorStrategy changeBehaviorStrategy = null, IUpdateStrategy updateStrategy = null)
        {
            Name = name;
            ChangeBehaviorStrategy = changeBehaviorStrategy ?? new DefaultChangeBehaviorStrategy();
            UpdateStrategy = updateStrategy ?? new DefaultUpdateStrategy();
            SubBehaviors = subBehaviors ?? Array.Empty<IBehavior>();
        }

        public virtual async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Entering Behavior: {Name}");
            if (token.IsCancellationRequested) return;
            await Task.Delay(10, token).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(CancellationToken token)
        {
            if (IsPaused || token.IsCancellationRequested) return;
            DebugManager.Log<Behavior>($"Updating... Behavior: {Name}");
            await Task.Delay(10, token).ConfigureAwait(false);
        
            // Verificar e atualizar sub comportamentos
            if (SubBehaviors is { Length: > 0 })
            {
                var currentSubBehavior = SubBehaviors[CurrentSubBehaviorIndex];

                if (!currentSubBehavior.Finalized)
                {
                    await currentSubBehavior.UpdateAsync(token).ConfigureAwait(false);
                }
            }
        }

        public virtual async Task ExitAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;
            DebugManager.Log<Behavior>($"Exiting Behavior: {Name}");
            await Task.Delay(10, token).ConfigureAwait(false);
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void Stop()
        {
            IsPaused = false;
        }

        // Método para finalizar todos os sub comportamentos
        public virtual async Task FinalizeAllSubBehavior(CancellationToken cancellationToken)
        {
            DebugManager.Log<DefaultFinalizeStrategy>("Call Finalize");

            // Finalizar sub comportamentos
            if (SubBehaviors.Length > 0)
            {
                var subManager = SubBehaviorManager;
                if (subManager != null)
                {
                    foreach (var subBehavior in SubBehaviors)
                    {
                        subBehavior.Finalized = true;
                        await subBehavior.ExitAsync(cancellationToken).ConfigureAwait(false);
                    }
                }
            }
            DebugManager.Log<DefaultFinalizeStrategy>("Finalize");

            // Finalizar o comportamento atual
            Finalized = true;
            await ExitAsync(cancellationToken).ConfigureAwait(false);
        }

        /*// Método para finalizar explicitamente o comportamento e todos os seus sub comportamentos
        public virtual async Task FinalizeAsync(CancellationToken cancellationToken)
        {
            if (SubBehaviors.Length <= 0) return;

            SubBehaviors[CurrentSubBehaviorIndex].Finalized = true;

            // Verificar se todos os sub comportamentos foram finalizados
            var allSubBehaviorsFinalized = SubBehaviors.All(subBehavior => subBehavior.Finalized);

            if (allSubBehaviorsFinalized)
            {
                Finalized = true;
            }
            else
            {
                // Avançar para o próximo sub comportamento
                CurrentSubBehaviorIndex++;
            }
            await ExitAsync(cancellationToken).ConfigureAwait(false);
            //return Task.CompletedTask;
        }*/
    }
}
