using System.Threading;
using System.Threading.Tasks;
using NewRiverAttack.ObstaclesSystems.BossSystems;

namespace ImmersiveGames.BehaviorsManagers.Interfaces
{
    public interface IChangeBehaviorStrategy
    {
        Task ChangeBehaviorAsync(IBehavior currentBehavior, BossBehavior bossBehavior,  CancellationTokenSource cancellationTokenSource);
    }
}