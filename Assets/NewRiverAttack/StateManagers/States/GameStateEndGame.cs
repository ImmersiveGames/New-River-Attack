using System.Threading.Tasks;
using ImmersiveGames;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine.SceneManagement;

namespace NewRiverAttack.StateManagers.States
{
    public class GameStateEndGame : GameState
    {
        public GameStateEndGame() : base("GameStateEndGame") { }
        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de Menu Inicial
            await base.OnEnter(previousState).ConfigureAwait(false);
            DebugManager.Log<GameStateEndGame>("Entrou no estado de FINAL");
        }
        public override void UpdateState()
        {
            // Lógica de atualização do estado de Menu Inicial
            DebugManager.Log<GameStateEndGame>($"Update no estado de HUB, initialized: {StateInitialized}");
        }
        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de Menu Inicial em outras threads
            await base.OnExit().ConfigureAwait(false);
            DebugManager.Log<GameStateEndGame>("Saiu do estado de Final");
        }

        public override bool RequiresSceneLoad => true;
        public override string SceneName => "EndGameCredits";
        public override LoadSceneMode LoadMode => LoadSceneMode.Single;
        public override bool UnLoadAdditiveScene => true;
        public override ITransition InTransition => new FadeTransition();
        public override ITransition OutTransition => new FadeTransition();
    }
}