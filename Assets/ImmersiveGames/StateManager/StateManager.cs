using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGames
{
    public class StateManager
    {
        private readonly Dictionary<string, IState> _states = new Dictionary<string, IState>();
        private static IState _currentState;
        private static IState _previousState;

        public void AddState(IState state)
        {
            _states[state.stateName] = state;
        }

        public async Task ChangeStateAsync(string stateName)
        {
            if (!_states.TryGetValue(stateName, out var nextState))
            {
                Debug.LogError($"Estado não encontrado: {stateName}");
                return;
            }

            if (nextState == _currentState)
            {
                Debug.Log($"Já está no estado: {stateName}");
                return;
            }

            if (_currentState != null)
            {
                await _currentState.ExitAsync().ConfigureAwait(false);
            }

            _previousState = _currentState;
            _currentState = nextState;

            await Task.Delay(2000).ConfigureAwait(true);

            // Adicionamos uma condição para verificar se há uma transição de cena necessária
            if (_currentState.requiresSceneLoad && !string.IsNullOrEmpty(_currentState.sceneName))
            {
                // Agora, esperamos que a transição de cena seja concluída antes de prosseguir
                await SceneChangeManager.StartSceneTransitionAsync(_currentState, _previousState?.sceneName, _currentState.loadMode, _currentState.unLoadAdditiveScene).ConfigureAwait(false);
            }

            await _currentState.EnterAsync(_previousState).ConfigureAwait(false);

            Debug.Log($"Mudou para o estado: {stateName}");
        }

        public static IState GetCurrentState()
        {
            return _currentState;
        }

        public IState GetPreviousState()
        {
            return _previousState;
        }
    }
}