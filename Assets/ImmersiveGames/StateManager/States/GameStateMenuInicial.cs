using System.Threading.Tasks;
using ImmersiveGames.InputManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManager.States
{
    public class GameStateMenuInicial: GameState
    {
        public GameStateMenuInicial() : base("GameStateMenuInicial") { }
        public override string sceneName => "MenuInicialNovo";
        public override bool requiresSceneLoad => true;
        public override LoadSceneMode loadMode => LoadSceneMode.Single;
        public override bool unLoadAdditiveScene => true;
        public override ITransition inTransition => new FadeTransition();
        public override ITransition outTransition => new FadeTransition();

        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de Menu Inicial
            await base.OnEnter(previousState).ConfigureAwait(false);
            
            Debug.Log("Entrou no estado de Menu Inicial");
        }

        public override void UpdateState()
        {
            // Lógica de atualização do estado de Menu Inicial
            Debug.Log($"Update no estado de Menu Inicial, initialized: {stateInitialized}");
        }

        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de Menu Inicial em outras threads
            await base.OnExit().ConfigureAwait(false);
            Debug.Log("Saiu do estado de Menu Inicial");
        }
    }
}