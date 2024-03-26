using ImmersiveGames.DebugManagers;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.States;
using UnityEngine;

namespace ImmersiveGames
{
    public class InitializationManager : MonoBehaviour
    {
        public DebugManager.DebugLevel debugLevel;

        public StatesNames startState = StatesNames.GameStateMenuInicial;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            DebugManager.debugLevel = debugLevel;

            // Inicialize _stateManager apenas se ainda não foi inicializado
            if (StateManager != null) return;
            StateManager = new StateManager();

            // Adicione os estados ao StateManager
            StateManager.AddState(new GameStateMenuInicial());
            StateManager.AddState(new GameStatePlay());
            StateManager.AddState(new GameStatePause());
            StateManager.AddState(new GameStateBriefingRoom());
            StateManager.AddState(new GameStateOpenGame());
            
        }
        private async void Start()
        {
            await StateManager.ChangeStateAsync(startState.ToString()).ConfigureAwait(false);
        }

        private void Update()
        {
            if (StateManager.GetCurrentState().StateInitialized)
                StateManager.GetCurrentState().UpdateState();
        }

        // Adicione uma propriedade estática para acessar _stateManager de qualquer lugar
        public static StateManager StateManager { get; private set; }
    }
}
