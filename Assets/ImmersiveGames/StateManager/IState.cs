using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ImmersiveGames
{
    public interface IState
    {
        string stateName { get; }
        bool stateInitialized { get; }
        ITransition inTransition => null;
        ITransition outTransition => null;
        Task EnterAsync(IState previousState);
        void UpdateState();
        Task ExitAsync();

        #region LoadScene

        bool requiresSceneLoad => false;
        string sceneName => null;

        LoadSceneMode loadMode => LoadSceneMode.Single;

        bool unLoadAdditiveScene => false;

        #endregion
    }
}