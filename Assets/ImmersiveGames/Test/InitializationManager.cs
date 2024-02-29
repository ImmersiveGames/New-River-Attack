using System;
using UnityEngine;

namespace ImmersiveGames
{
    public class InitializationManager : MonoBehaviour
    {
        private readonly StateManager _stateManager = new StateManager();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private async void Start()
        {
            // Adicione os estados ao StateManager
            DontDestroyOnLoad(this);
            _stateManager.AddState(new GameStateMenuInicial());
            _stateManager.AddState(new GameStatePlay());
            _stateManager.AddState(new GameStatePause());
            
            // Inicia no estado de jogo e sua scena inicial
            await _stateManager.ChangeStateAsync("GameStateMenuInicial").ConfigureAwait(false);

        }

        private async void Update()
        {
            // Troca de estado ao pressionar tecla P para pausar e R para retomar
            if (Input.GetKeyDown(KeyCode.P))
            {
                await _stateManager.ChangeStateAsync("GameStatePause").ConfigureAwait(false);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                await _stateManager.ChangeStateAsync("GameStatePlay").ConfigureAwait(false);
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                await _stateManager.ChangeStateAsync("GameStateMenuInicial").ConfigureAwait(false);
            }
            if(StateManager.GetCurrentState().stateInitialized)
                StateManager.GetCurrentState().UpdateState();
        }
    }
}