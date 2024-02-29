using System.Threading.Tasks;

namespace ImmersiveGames
{
    public interface ITransition
    {
        Task InTransitionAsync();
        Task OutTransitionAsync();
    }
}