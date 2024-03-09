using System.Threading.Tasks;
using ImmersiveGames.InputManager;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.Utils;
using RiverAttack;
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

        #region InputSystem

        private static PlayersInputActions _inputActions;
        private static ActionManager _actionManager;
        protected abstract GameActionMaps stateInputActionMap { get; }

        private static void ChangeInputMap(GameActionMaps actionMap)
        {
            // Inicializa o PlayersInputActions
            _inputActions = InputManagerInitializer.InputActions;
            _inputActions.Enable();

            // Inicializa o ActionManager com a instância de PlayersInputActions
            _actionManager = InputManagerInitializer.ActionManager;
            _actionManager.ActivateActionMap(actionMap);
        }

        #endregion
        
        #region Enter and Exit Methods

        protected virtual async Task OnEnter(IState previousState)
        {
            // Custom logic to be executed on entering the state
            await MainThreadTaskExecutor.instance.RunOnMainThreadAsync(() =>
            {
                ChangeInputMap(stateInputActionMap);
                return Task.CompletedTask;
            }).ConfigureAwait(false);
            //
            await Task.Yield();
        }

        protected virtual async Task OnExit()
        {
            // Custom logic to be executed on exiting the state
            // Certifique-se de desativar o PlayersInputActions ao destruir o objeto
            _inputActions?.Disable();

            // Certifique-se de desativar o ActionManager ao destruir o objeto
            _actionManager = null;
            await Task.Yield();
        }

        #endregion
    }
}