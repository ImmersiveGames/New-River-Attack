using ImmersiveGames.DebugManagers;
using ImmersiveGames.GamePlayManagers;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
using ImmersiveGames.StateManagers.States;
using ImmersiveGames.Utils;
using UnityEngine;

namespace ImmersiveGames
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Debug Settings")]
        public bool enableDebugs;
        [Header("State Settings")]
        public StatesNames startState = StatesNames.GameStateMenuInicial;

        [Header("Game Modes Settings")] 
        public GamePlayModes gamePlayMode;
        

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            DebugManager.SetGlobalDebugState(enableDebugs);

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
        private async void OnEnable()
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
