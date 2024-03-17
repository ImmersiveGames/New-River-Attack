using System.Threading.Tasks;
using ImmersiveGames.ScenesManager.Transitions;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManager
{
    public abstract class GameState : IState
    {
        protected GameState(string currentStateName)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            stateName = currentStateName;
        }
        #region IState Interface
        
        public string stateName { get; }
        public bool stateInitialized { get; set; }

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

        #region Scene Change

        public abstract bool requiresSceneLoad { get; }
        public abstract string sceneName  { get; }
        public abstract LoadSceneMode loadMode { get; }
        public abstract bool unLoadAdditiveScene { get; }

        #endregion

        #region Transitions

        public abstract ITransition inTransition { get; }
        public abstract ITransition outTransition { get; }
        #endregion
        
        
        #region Enter and Exit Methods

        protected virtual async Task OnEnter(IState previousState)
        {
            // Custom logic to be executed on entering the state
            
            await Task.Yield();
        }

        protected virtual async Task OnExit()
        {
            // Custom logic to be executed on exiting the state
            await Task.Yield();
        }

        #endregion
    }
}