using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManager.States
{
    public class GameStateBriefingRoom: GameState
    {
        public GameStateBriefingRoom() : base("GameStateBriefingRoom") { }

        protected override async Task OnEnter(IState previousState)
        {
            await base.OnEnter(previousState).ConfigureAwait(false);
            DebugManager.Log("Entrou no estado de Briefing Room");
            
            //TODO: Iniciar o Tutorial
        }

        public override void UpdateState()
        {
            // Lógica de atualização do estado de pausa
            DebugManager.Log($"Update no estado de Briefing Room, initialized: {stateInitialized}");
        }

        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de pausa em outras threads
            await base.OnExit().ConfigureAwait(false);
            DebugManager.Log("Saiu do estado de Briefing Room");
        }

        public override bool requiresSceneLoad => true;
        public override string sceneName => "BriefingRoom";
        public override LoadSceneMode loadMode => LoadSceneMode.Single;
        public override bool unLoadAdditiveScene => true;
        public override ITransition inTransition => new FadeTransition();
        public override ITransition outTransition => new FadeTransition();
    }
}