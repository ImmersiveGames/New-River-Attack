using System.Threading.Tasks;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.GameStateSystem.Interfaces
{
    public interface IGameState
    {
        string StateName { get; }
        void EnterState(IGameState previousState);  // Executa transição de entrada, como fade in
        void ExitState(IGameState nextState);   // Executa transição de saída, como fade out
    }

    public interface IGameStateScene
    {
        Task EnterScene();
        Task ExitScene();
        string SceneName { get; }
        bool UnLoadAdditiveScene => false;
        LoadSceneMode LoadMode => LoadSceneMode.Single;
    }

    public interface IGameStateTransition
    {
        ITransition InTransition => null;
        ITransition OutTransition => null;
    }
}