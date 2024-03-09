using System.Threading.Tasks;

namespace ImmersiveGames.ScenesManager.Transitions
{
    public interface ITransition
    {
        Task InTransitionAsync();
        Task OutTransitionAsync();
    }
}