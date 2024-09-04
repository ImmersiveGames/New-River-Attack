using System.Collections.Generic;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager;
using ImmersiveGames.StateManagers.Interfaces;

namespace ImmersiveGames.StateManagers
{
    public class StateManager
    {
        private readonly Dictionary<string, IState> _states = new Dictionary<string, IState>();
        private static IState _currentState;
        private static IState _previousState;

        public void AddState(IState state)
        {
            _states[state.StateName] = state;
        }

        public void ForceChangeState(string stateName)
        {
            if (!_states.TryGetValue(stateName, out var nextState))
            {
                DebugManager.LogError<StateManager>($"Estado não encontrado: {stateName}");
                return;
            }
            AudioManager.instance.PlayBGM(nextState);
            _currentState.ExitAsync(nextState);
            _previousState = _currentState;
            _currentState = nextState;
            _currentState.EnterAsync(_previousState);
        }

        public async Task ChangeStateAsync(string stateName)
        {
            if (!_states.TryGetValue(stateName, out var nextState))
            {
                DebugManager.LogError<StateManager>($"Estado não encontrado: {stateName}");
                return;
            }
            if (nextState == _currentState)
            {
                DebugManager.Log<StateManager>($"Já está no estado: {stateName}");
                return;
            }
            AudioManager.instance.PlayBGM(nextState);

            if (_currentState != null)
            {
                await _currentState.ExitAsync(nextState).ConfigureAwait(false);
            }

            _previousState = _currentState;
            _currentState = nextState;

            if (_currentState.RequiresSceneLoad && !string.IsNullOrEmpty(_currentState.SceneName))
            {
                // Agora, esperamos que a transição de cena seja concluída antes de prosseguir
                await SceneChangeManager.StartSceneTransitionAsync(_currentState, _previousState?.SceneName, _currentState.LoadMode, _currentState.UnLoadAdditiveScene).ConfigureAwait(false);
            }

            await _currentState.EnterAsync(_previousState).ConfigureAwait(false);

            DebugManager.Log<StateManager>($"Mudou para o estado: {stateName}");
        }

        public IState GetCurrentState()
        {
            return _currentState;
        }

        public IState GetPreviousState()
        {
            return _previousState;
        }
    }
}
