using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;

namespace ImmersiveGames.BehaviorsManagers
{
    public abstract class Behavior : IBehavior
    {
        private const double MinimumDistance = 10;
        private const int DebugDelay = 2000;

        protected Behavior(IBehavior[] subBehaviors, string identifier = "")
        {
            Name = GetType().Name;
            if (identifier != "")
            {
                Name += "_" + identifier;
            }
            SubBehaviors = subBehaviors;
        }

        public string Name { get; }
        public IBehavior[] SubBehaviors { get; }

        public bool Initialized { get; set; }
        public bool Finalized { get; set; }

        public virtual async Task EnterAsync(CancellationToken token)
        {
            await Task.Delay(DebugDelay, token).ConfigureAwait(false);
            DebugManager.Log<Behavior>($"Enter {GetType().Name}.");
        }

        public virtual async void UpdateAsync(CancellationToken token)
        {
            await Task.Delay(DebugDelay, token).ConfigureAwait(false);
            DebugManager.Log<Behavior>($"Update {GetType().Name}.");
        }

        public virtual async Task ExitAsync(CancellationToken token)
        {
            await Task.Delay(DebugDelay, token).ConfigureAwait(false);
            DebugManager.Log<Behavior>($"Exit {GetType().Name}.");
        }
        
    }
}