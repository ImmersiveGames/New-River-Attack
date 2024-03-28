using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManagers.States
{
    public class GameStatePause : GameState
    {
        public GameStatePause() : base("GameStatePause") { }

        public override string SceneName => "Pause";
        public override bool RequiresSceneLoad => true;
        public override LoadSceneMode LoadMode => LoadSceneMode.Single;
        public override bool UnLoadAdditiveScene => true;
        
        public override ITransition InTransition => new FadeTransition();
        public override ITransition OutTransition => new FadeTransition();

        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de pausa
            await base.OnEnter(previousState).ConfigureAwait(false);
            DebugManager.Log<GameStatePause>("Entrou no estado de pausa");
        }

        public override void UpdateState()
        {
            // Lógica de atualização do estado de pausa
            DebugManager.Log<GameStatePause>($"Update no estado de pausa, initialized: {StateInitialized}");
        }

        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de pausa em outras threads
            await base.OnExit().ConfigureAwait(false);
            DebugManager.Log<GameStatePause>("Saiu do estado de pausa");
        }
        
    }
}