using ImmersiveGames.DebugManagers;
using ImmersiveGames.StateManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.StateManagers;
using NewRiverAttack.StateManagers.States;
using UnityEngine;
namespace NewRiverAttack.GameManagers
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Debug Settings")]
        public bool enableDebugs;
        [Header("State Settings")]
        public StatesNames startState = StatesNames.GameStateMenuInitial;

        [Header("Game Modes Settings")] 
        public GamePlayModes gamePlayMode;

        public LevelData classicModeLevels;
        public LevelListData missionModeLevels;
        internal LevelData ActiveLevel;
        internal int ActiveIndex;
        
        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            DebugManager.SetGlobalDebugState(enableDebugs);

            // Initialize _stateManager apenas se ainda não foi inicializado
            if (StateManager != null) return;
            StateManager = new StateManager();

            // adicione os estados ao StateManager
            StateManager.AddState(new GameStateMenuInitial());
            StateManager.AddState(new GameStatePlay());
            StateManager.AddState(new GameStateGameOver());
            StateManager.AddState(new GameStateBriefingRoom());
            StateManager.AddState(new GameStateHub());
            StateManager.AddState(new GameStateEndGame());
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
