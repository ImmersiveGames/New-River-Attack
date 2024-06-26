using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine.SceneManagement;

namespace NewRiverAttack.StateManagers.States
{
    public class GameStateGameOver: GameState
    {
        public GameStateGameOver() : base("GameStateGameOver") { }

        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de game
            await base.OnEnter(previousState).ConfigureAwait(false);
            DebugManager.Log<GameStateGameOver>("Entrou no estado de Game Over");
        }
        
        public override void UpdateState()
        {
            DebugManager.Log<GameStateGameOver>($"Update no estado de pausa, initialized: {StateInitialized}");
        }
        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de Game em outras threads
            await base.OnExit().ConfigureAwait(false);
            DebugManager.Log<GameStateGameOver>("Saiu do estado de Game Over");
        }

        public override bool RequiresSceneLoad => false;
        public override string SceneName => "";
        public override LoadSceneMode LoadMode => LoadSceneMode.Single;
        public override bool UnLoadAdditiveScene => false;
        public override ITransition InTransition => null;
        public override ITransition OutTransition => null;
    }
}