using UnityEngine;

namespace ImmersiveGames.Tests
{
    public class TestChangeStates: MonoBehaviour
    {
        private async void Update()
        {
            // Troca de estado ao pressionar tecla P para pausar e R para retomar
            if (Input.GetKeyDown(KeyCode.P))
            {
                await GameManager.StateManager.ChangeStateAsync("GameStatePause").ConfigureAwait(false);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                await GameManager.StateManager.ChangeStateAsync("GameStatePlay").ConfigureAwait(false);
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                await GameManager.StateManager.ChangeStateAsync("GameStateMenuInicial").ConfigureAwait(false);
            }
        }
    }
}