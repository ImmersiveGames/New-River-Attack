using System.Threading.Tasks;
using ImmersiveGames.InputManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManager.States
{
    public class GameStatePause : GameState
    {
        public GameStatePause() : base("GameStatePause") { }
        public override string sceneName => "Pause";
        public override bool requiresSceneLoad => true;
        public override LoadSceneMode loadMode => LoadSceneMode.Single;
        public override bool unLoadAdditiveScene => true;
        public override ITransition inTransition => new FadeTransition();
        public override ITransition outTransition => new FadeTransition();
        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de pausa
            await base.OnEnter(previousState).ConfigureAwait(false);
            Debug.Log("Entrou no estado de pausa");
        }

        public override void UpdateState()
        {
            // Lógica de atualização do estado de pausa
            Debug.Log($"Update no estado de pausa, initialized: {stateInitialized}");
        }

        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de pausa em outras threads
            await base.OnExit().ConfigureAwait(false);
            Debug.Log("Saiu do estado de pausa");
        }
        
    }
}