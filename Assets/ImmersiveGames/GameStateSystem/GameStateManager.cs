using System.Collections.Generic;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.GameStateSystem.Interfaces;
using ImmersiveGames.ScenesManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.GameStateSystem
{
    public class GameStateManager
    {
        private readonly Dictionary<string, IGameState> _states = new Dictionary<string, IGameState>();
        private static IGameState _currentState;
        private static IGameState _previousState;

        public void AddState(IGameState state)
        {
            _states[state.StateName] = state;
        }

        public void ChangeState(string stateName)
        {
            
            if (!_states.TryGetValue(stateName, out var nextState))
            {
                DebugManager.LogError<GameStateManager>($"Estado não encontrado: {stateName}");
                return;
            }

            if (nextState == _currentState)
            {
                DebugManager.Log<GameStateManager>($"Já está no estado: {stateName}");
                return;
            }
            
            // Sair do estado atual
            _currentState?.ExitState(nextState);
           
            // Atualizar o estado
            _previousState = _currentState;
            _currentState = nextState;

            // Entrar no novo estado
            _currentState?.EnterState(_previousState);
            DebugManager.Log<GameStateManager>($"Mudou para o estado: {stateName}");
        }

        public IGameState GetCurrentState => _currentState;
        public IGameState GetPreviousState => _previousState;
    }
}
