using ImmersiveGames.InputManager;
using ImmersiveGames.SaveManagers;
using ImmersiveGames.StateManager.States;
using RiverAttack;
using UnityEngine;
using GameStatePause = ImmersiveGames.StateManager.States.GameStatePause;

namespace ImmersiveGames
{
    public class InitializationManager : MonoBehaviour
    {
        // Transforme _stateManager em uma propriedade estática

        [SerializeField] protected GameOptionsSave gameOptionsSave;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            // Inicialize _stateManager apenas se ainda não foi inicializado
            if (StateManager != null) return;
            StateManager = new StateManager.StateManager();

            // Adicione os estados ao StateManager
            StateManager.AddState(new GameStateMenuInicial());
            StateManager.AddState(new GameStatePlay());
            StateManager.AddState(new GameStatePause());
                
            // Inicia no estado de jogo e sua cena inicial
            StateManager.ChangeStateAsync("GameStateMenuInicial").ConfigureAwait(false).GetAwaiter().GetResult();
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
