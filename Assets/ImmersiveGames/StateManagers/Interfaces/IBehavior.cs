using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGames.StateManagers.Interfaces
{
    public interface IBehavior
    {
        string Name { get; }
        bool Initialized { get; set; }
        bool Finalization { get; set; }
        Task EnterAsync(IBehavior previousBehavior, MonoBehaviour monoBehaviour, CancellationToken token);
        Task UpdateAsync(CancellationToken token);
        Task ExitAsync(IBehavior nextBehavior, CancellationToken token);
        Task TransitionToAsync(IBehavior nextBehavior, MonoBehaviour monoBehaviour, CancellationToken token);
        void Pause();
        void Resume();
        void Stop();
    }
}