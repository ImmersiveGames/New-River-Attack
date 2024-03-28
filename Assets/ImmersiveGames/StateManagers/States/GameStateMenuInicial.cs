using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManagers.States
{
    public class GameStateMenuInicial: GameState
    {
        public GameStateMenuInicial() : base("GameStateMenuInicial") { }
        public override string SceneName => "MenuInicialNovo";
        public override bool RequiresSceneLoad => true;
        public override LoadSceneMode LoadMode => LoadSceneMode.Single;
        public override bool UnLoadAdditiveScene => true;
        public override ITransition InTransition => new FadeTransition();
        public override ITransition OutTransition => new FadeTransition();

        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de Menu Inicial
            await base.OnEnter(previousState).ConfigureAwait(false);
            DebugManager.Log<GameStateMenuInicial>("Entrou no estado de Menu Inicial");
        }

        public override void UpdateState()
        {
            // Lógica de atualização do estado de Menu Inicial
            DebugManager.Log<GameStateMenuInicial>($"Update no estado de Menu Inicial, initialized: {StateInitialized}");
        }

        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de Menu Inicial em outras threads
            await base.OnExit().ConfigureAwait(false);
            DebugManager.Log<GameStateMenuInicial>("Saiu do estado de Menu Inicial");
        }
    }
}