using System;
using ImmersiveGames.GameStateSystem.GameStates;
using ImmersiveGames.GameStateSystem.Interfaces;
using NewRiverAttack.StateManagers.States;
using UnityEngine;

namespace ImmersiveGames.GameStateSystem
{
    public class TesteChangeState : MonoBehaviour
    {
        private GameStateManager _gameStateManager;
        private void Awake()
        {
            // Initialize _stateManager apenas se ainda não foi inicializado
            if (_gameStateManager != null) return;
            _gameStateManager = new GameStateManager();

            // adicione os estados ao StateManager
            _gameStateManager.AddState(new GameStatesMenu("MenuInicial"));
            _gameStateManager.AddState(new GameStatesRunning("GamePlay"));
            
            _gameStateManager.ChangeState("MenuInicial");
            _ = GameSceneManager.instance.LoadingSceneAsync(_gameStateManager.GetCurrentState as IGameStateScene, _gameStateManager.GetPreviousState as IGameStateScene);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                _gameStateManager.ChangeState("MenuInicial");
            }
            if (Input.GetKey(KeyCode.S))
            {
                _gameStateManager.ChangeState("GamePlay");
            }
        }
    }
}