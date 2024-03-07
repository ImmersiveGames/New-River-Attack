using System.Threading.Tasks;
using ImmersiveGames.InputManager;
using RiverAttack;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManager
{
    public abstract class GameState : IState
    {
        protected GameState(string currentStateName)
        {
            stateName = currentStateName;
        }
        #region IState Interface
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

        #region Scene Change

        public virtual bool requiresSceneLoad => false;
        public virtual string sceneName => null;
        public virtual LoadSceneMode loadMode => LoadSceneMode.Single;
        public virtual bool unLoadAdditiveScene => true;

        #endregion

        #region Transitions

        public virtual ITransition inTransition => null;
        public virtual ITransition outTransition => null;

        #endregion

        #region InputSystem

        private static PlayersInputActions _inputActions;
        private static ActionManager _actionManager;
        protected virtual GameActionMaps stateInputActionMap => GameActionMaps.UiControls;
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
            ChangeInputMap(stateInputActionMap);
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