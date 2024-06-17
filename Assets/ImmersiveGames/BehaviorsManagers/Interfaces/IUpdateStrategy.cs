using System.Threading;
using System.Threading.Tasks;

namespace ImmersiveGames.BehaviorsManagers.Interfaces
{
    public interface IUpdateStrategy
    {
        Task UpdateAsync(IBehavior currentBehavior, BehaviorManager manager, CancellationToken cancellationToken);
    }
}