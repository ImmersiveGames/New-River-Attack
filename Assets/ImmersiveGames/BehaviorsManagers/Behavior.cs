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
        public string Name { get; protected set; }
        public bool Initialized { get; set; }
        public bool Finalized { get; set; }
        private bool _isPaused;
        public IBehavior[] SubBehaviors { get; }
        protected IBehavior NextBehavior = null;
        public IChangeBehaviorStrategy ChangeBehaviorStrategy { get; }
        public IUpdateStrategy UpdateStrategy { get; }
        public BehaviorManager SubBehaviorManager { get; set; } // Adicionado

        protected Behavior(string name, IBehavior[] subBehaviors, IChangeBehaviorStrategy changeBehaviorStrategy, IUpdateStrategy updateStrategy)
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
            if (_isPaused || token.IsCancellationRequested) return;
            DebugManager.Log<Behavior>($"Updating... Behavior: {Name}");
            await Task.Delay(10, token).ConfigureAwait(false);

            if (SubBehaviorManager != null)
            {
                await SubBehaviorManager.UpdateAsync().ConfigureAwait(false);
            }
        }

        public virtual async Task ExitAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;
            DebugManager.Log<Behavior>($"Exiting Behavior: {Name}");
            await Task.Delay(10, token).ConfigureAwait(false);
            Initialized = false;
            Finalized = false;

            if (SubBehaviorManager != null)
            {
                await SubBehaviorManager.UpdateAsync().ConfigureAwait(false);
            }
        }
        
        public virtual async Task FinalizeAsync()
        {
            Finalized = true;

            if (SubBehaviors is { Length: > 0 })
            {
                foreach (var subBehavior in SubBehaviors)
                {
                    await subBehavior.FinalizeAsync().ConfigureAwait(false);
                }
            }
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
    }
}
