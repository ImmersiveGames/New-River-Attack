using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine.SceneManagement;

namespace NewRiverAttack.StateManagers.States
{
    public class GameStateBriefingRoom: GameState
    {
        public GameStateBriefingRoom() : base("GameStateBriefingRoom") { }

        protected override async Task OnEnter(IState previousState)
        {
            await base.OnEnter(previousState).ConfigureAwait(false);
            DebugManager.Log<GameStateBriefingRoom>("Entrou no estado de Briefing Room");
            
            //TODO: Iniciar o Tutorial
            
        }

        public override void UpdateState()
        {
            if (StateFinalization == false) return;
            // Lógica de atualização do estado de pausa
            DebugManager.Log<GameStateBriefingRoom>($"Update no estado de Briefing Room, initialized: {StateInitialized}");
        }

        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de pausa em outras threads
            await base.OnExit().ConfigureAwait(false);
            DebugManager.Log<GameStateBriefingRoom>(" Saiu do estado de Briefing Room");
        }

        public override bool RequiresSceneLoad => true;
        public override string SceneName => "BriefingRoom";
        public override LoadSceneMode LoadMode => LoadSceneMode.Single;
        public override bool UnLoadAdditiveScene => true;
        public override ITransition InTransition => new FadeTransition();
        public override ITransition OutTransition => new FadeTransition();
    }
}