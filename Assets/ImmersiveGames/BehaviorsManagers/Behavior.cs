using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;

namespace ImmersiveGames.BehaviorsManagers
{
    public abstract class Behavior : IBehavior
    {
        private const double MinimumDistance = 10;
        protected Behavior(string name, IBehavior[] subBehaviors)
        {
            Name = name;
            SubBehaviors = subBehaviors;
        }

        public string Name { get; }
        public IBehavior[] SubBehaviors { get; }

        public bool Initialized { get; set; }
        public bool Finalized { get; set; }

        public virtual Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Enter Behavior {Name}");
            Finalized = false;
            Initialized = false;
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Update Behavior {Name}");
            return Task.CompletedTask;
        }

        public virtual Task ExitAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Exit Behavior {Name}");
            Finalized = true;
            Initialized = false;
            return Task.CompletedTask;
        }
        
    }
}