using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGames
{
    public class GameManagerTest : MonoBehaviour
    {
        private readonly StateManager _stateManager = new StateManager();
        private async void Start()
        {
            // Adicione os estados ao StateManager
            DontDestroyOnLoad(this);
            _stateManager.AddState(new GameStatePlay());
            _stateManager.AddState(new GameStatePause());
            
            // Inicia no estado de jogo
            await _stateManager.ChangeStateAsync("GameStatePlay").ConfigureAwait(false);

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
            if(StateManager.GetCurrentState().stateInitialized)
                StateManager.GetCurrentState().UpdateState();
        }
    }
}