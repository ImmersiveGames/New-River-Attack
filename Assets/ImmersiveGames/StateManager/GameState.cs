using System.Threading.Tasks;
using UnityEngine.SceneManagement;


namespace ImmersiveGames
{
    public abstract class GameState : IState
    {
        protected GameState(string currentStateName)
        {
            stateName = currentStateName;
        }

        #region IState

        public string stateName { get; }

        public bool stateInitialized { get; private set; }
        public async Task EnterAsync(IState previousState)
        {
            if (!stateInitialized)
            {
                stateInitialized = true;
            }
            var transitionOut = outTransition;
            
            if (transitionOut != null)
            {
                await transitionOut.OutTransitionAsync().ConfigureAwait(false);
            }
            
            await OnEnter(previousState).ConfigureAwait(false);
        }

        public abstract void UpdateState();

        public async Task ExitAsync()
        {
            await OnExit().ConfigureAwait(false);
            
            var transitionIn = inTransition;
            if (transitionIn != null)
            {
                await transitionIn.InTransitionAsync().ConfigureAwait(false);
            }
        }

        #endregion

        #region SceneChange

        public virtual bool requiresSceneLoad => false;
        public virtual string sceneName => null;

        public virtual LoadSceneMode loadMode => LoadSceneMode.Single;

        public virtual bool unLoadAdditiveScene => true;

        #endregion

        public virtual ITransition inTransition => null;
        public virtual ITransition outTransition => null;
        
        protected virtual async Task OnEnter(IState previousState)
        {
            await Task.Yield();
        }
        protected virtual async Task OnExit()
        {
            await Task.Yield();
        }
    }
}