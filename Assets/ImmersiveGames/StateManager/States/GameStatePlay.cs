using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManager.States
{
    public class GameStatePlay : GameState
    {
        public GameStatePlay() : base("GameStatePlay") { }
        public override string sceneName => "GamePlayNovo";
        public override bool requiresSceneLoad => true;
        public override LoadSceneMode loadMode => LoadSceneMode.Single;
        public override bool unLoadAdditiveScene => true;
        public override ITransition inTransition => new FadeTransition();
        public override ITransition outTransition => new FadeTransition();
        
        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de game
            await base.OnEnter(previousState).ConfigureAwait(false);
            DebugManager.Log("Entrou no estado de Game");
        }

        public override void UpdateState()
        {
            // Lógica de atualização do estado de game
            DebugManager.Log($"Update no estado de Game, initialized: {stateInitialized}");
        }

        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de Game em outras threads
            await base.OnExit().ConfigureAwait(false);
            DebugManager.Log("Saiu do estado de Game");
        }
    }
}