using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers.Interfaces;
using NewRiverAttack.LevelBuilder;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManagers
{
    public abstract class GameState : IState
    {
        protected GameState(string currentStateName)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            StateName = currentStateName;
        }
        #region IState Interface
        
        public string StateName { get;}
        public bool StateInitialized { get; set; }

        private bool _stateFinalization;
        public bool StateFinalization { get; private set; }

        public async Task EnterAsync(IState previousState)
        {
            if (!StateInitialized)
            {
                StateInitialized = true;
            }
            DebugManager.Log<GameState>($"O {previousState} requer transição? {RequiresSceneLoad}");
            if (previousState == null || RequiresSceneLoad)
            {
                var transitionOut = OutTransition;

                if (transitionOut != null)
                {
                    DebugManager.Log<GameState>($"O {previousState} ira transitar usando: {transitionOut}");
                    await transitionOut.OutTransitionAsync().ConfigureAwait(false);
                }
            }
            
            await OnEnter(previousState).ConfigureAwait(false);
            StateFinalization = true;
        }

        public abstract void UpdateState();

        public async Task ExitAsync(IState nextState)
        {
            StateFinalization = false;
            await OnExit().ConfigureAwait(false);
            if (nextState.RequiresSceneLoad && RequiresSceneLoad)
            {
                var transitionIn = InTransition;
                if (transitionIn != null)
                {
                    DebugManager.Log<GameState>($"Saida: In: {transitionIn}");
                    await transitionIn.InTransitionAsync().ConfigureAwait(false);
                }
            }
        }

        #endregion

        #region Scene Change

        public abstract bool RequiresSceneLoad { get; }
        public abstract string SceneName  { get; }
        public abstract LoadSceneMode LoadMode { get; }
        public abstract bool UnLoadAdditiveScene { get; }

        #endregion

        #region Transitions

        public abstract ITransition InTransition { get; }
        public abstract ITransition OutTransition { get; }
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