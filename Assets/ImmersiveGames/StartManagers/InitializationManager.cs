using ImmersiveGames.SaveManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.StateManager.States;
using UnityEngine;

namespace ImmersiveGames
{
    public class InitializationManager : MonoBehaviour
    {
        // Transforme _stateManager em uma propriedade estática

        [SerializeField] protected GameOptionsSave gameOptionsSave;
        public DebugManager.DebugLevel debugLevel;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            DebugManager.debugLevel = debugLevel;

            // Inicialize _stateManager apenas se ainda não foi inicializado
            if (StateManager != null) return;
            StateManager = new StateManager.StateManager();

            // Adicione os estados ao StateManager
            StateManager.AddState(new GameStateMenuInicial());
            StateManager.AddState(new GameStatePlay());
            StateManager.AddState(new GameStatePause());
            StateManager.AddState(new GameStateBriefingRoom());
            
        }
        private async void Start()
        {
            await StateManager.ChangeStateAsync("GameStateMenuInicial").ConfigureAwait(false);
        }

        private void Update()
        {
            if (StateManager.GetCurrentState().stateInitialized)
                StateManager.GetCurrentState().UpdateState();
        }

        // Adicione uma propriedade estática para acessar _stateManager de qualquer lugar
        public static StateManager.StateManager StateManager { get; private set; }
    }
}
