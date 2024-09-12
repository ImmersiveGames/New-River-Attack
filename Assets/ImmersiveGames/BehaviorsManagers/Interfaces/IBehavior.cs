using System.Threading;
using System.Threading.Tasks;

namespace ImmersiveGames.BehaviorsManagers.Interfaces
{
    public interface IBehavior
    {
        string Name { get; }
        IBehavior[] SubBehaviors { get; }
        bool Initialized { get; set; }
        bool Finalized { get; set; }

        Task EnterAsync(CancellationToken token);
        void UpdateAsync(CancellationToken token);
        Task ExitAsync(CancellationToken token);
        
    }
}