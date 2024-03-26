using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.States;
using UnityEngine;

namespace ImmersiveGames.DialogManagers.Test
{
    public class TestBriefingRoom: MonoBehaviour
    {
        private async void Start()
        {
            if (InitializationManager.StateManager == null)
            {
                var stateManager = new StateManager();
                // Adicione os estados ao StateManager
                stateManager.AddState(new GameStateBriefingRoom());
                await stateManager.ChangeStateAsync("GameStateBriefingRoom").ConfigureAwait(false);
            }
            
        }
    }
}