using System;
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
        private bool _isPaused;
        public IBehavior[] SubBehaviors { get; }
        public BehaviorManager SubBehaviorManager { get; set; }
        protected IBehavior NextBehavior = null;
        public IChangeBehaviorStrategy ChangeBehaviorStrategy { get; }
        public IUpdateStrategy UpdateStrategy { get; }
        private IFinalizeStrategy FinalizeStrategy { get; }
        public int CurrentSubBehaviorIndex { get; set; }

        protected Behavior(string name, IBehavior[] subBehaviors, IChangeBehaviorStrategy changeBehaviorStrategy = null, IUpdateStrategy updateStrategy = null, IFinalizeStrategy finalizeStrategy = null)
        {
            Name = name;
            ChangeBehaviorStrategy = changeBehaviorStrategy ?? new DefaultChangeBehaviorStrategy();
            UpdateStrategy = updateStrategy ?? new DefaultUpdateStrategy();
            FinalizeStrategy = finalizeStrategy ?? new DefaultFinalizeStrategy();
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
            if (_isPaused || token.IsCancellationRequested) return;
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
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        public void Stop()
        {
            _isPaused = false;
        }

        // Método para finalizar explicitamente o sub comportamento
        public void FinalizeSubBehavior()
        {
            FinalizeStrategy.FinalizeSubBehavior(this);
        }

        // Método para finalizar explicitamente o comportamento e todos os seus sub comportamentos
        public async Task FinalizeAsync(CancellationToken cancellationToken)
        {
            await FinalizeStrategy.FinalizeAsync(this, cancellationToken).ConfigureAwait(false);
        }
    }
}
