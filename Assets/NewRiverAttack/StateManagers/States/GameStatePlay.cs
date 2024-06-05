using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
using NewRiverAttack.GameManagers;
using NewRiverAttack.LevelBuilder;
using UnityEngine.SceneManagement;

namespace NewRiverAttack.StateManagers.States
{
    public class GameStatePlay : GameState
    {
        public GameStatePlay() : base("GameStatePlay") { }
        public override string SceneName => "GamePlayNovo";
        
        public override bool RequiresSceneLoad => true;
        public override LoadSceneMode LoadMode => LoadSceneMode.Single;
        public override bool UnLoadAdditiveScene => true;
        public override ITransition InTransition => new FadeTransition();
        public override ITransition OutTransition => new FadeTransition();
        
        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de game
            await base.OnEnter(previousState).ConfigureAwait(false);
            DebugManager.Log<GameStatePlay>("Entrou no estado de GamePlay");
        }

        public override void UpdateState()
        {
            // Lógica de atualização do estado de game
            DebugManager.Log<GameStatePlay>($"Update no estado de GamePlay, initialized: {StateInitialized}");
        }

        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de Game em outras threads
            await base.OnExit().ConfigureAwait(false);
            DebugManager.Log<GameStatePlay>("Saiu do estado de GamePlay");
        }
    }
}