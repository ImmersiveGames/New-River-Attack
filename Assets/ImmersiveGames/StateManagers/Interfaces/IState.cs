using System.Threading.Tasks;
using ImmersiveGames.ScenesManager.Transitions;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManagers.Interfaces
{
    public interface IState
    {
        string StateName { get; }
        bool StateInitialized { get; set; }
        bool StateFinalization { get; }
        ITransition InTransition => null;
        ITransition OutTransition => null;
        Task EnterAsync(IState previousState);
        void UpdateState();
        Task ExitAsync(IState nextState);

        #region LoadScene

        bool RequiresSceneLoad => false;
        string SceneName => null;

        LoadSceneMode LoadMode => LoadSceneMode.Single;

        bool UnLoadAdditiveScene => false;

        #endregion
    }
}