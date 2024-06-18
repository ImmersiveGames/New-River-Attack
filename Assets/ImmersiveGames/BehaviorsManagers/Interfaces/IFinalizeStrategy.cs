using System.Threading;
using System.Threading.Tasks;

namespace ImmersiveGames.BehaviorsManagers.Interfaces
{
    public interface IFinalizeStrategy
    {
        Task FinalizeAsync(IBehavior behavior, CancellationToken cancellationToken);
        void FinalizeSubBehavior(IBehavior behavior);
    }
}