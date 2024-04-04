using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers.Interfaces;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManagers.States
{
    public class GameStatePause : GameState
    {
        public GameStatePause() : base("GameStatePause") { }

        public override string SceneName => "";
        public override bool RequiresSceneLoad => false;
        public override LoadSceneMode LoadMode => LoadSceneMode.Single;
        public override bool UnLoadAdditiveScene => false;
        
        public override ITransition InTransition => null;
        public override ITransition OutTransition => null;

        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de pausa
            await base.OnEnter(previousState).ConfigureAwait(false);
            MainThreadTaskExecutor.RunOnMainThread(() =>
            {
                Time.timeScale = 0;
            });
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
            MainThreadTaskExecutor.RunOnMainThread(() =>
            {
                Time.timeScale = 1;
            });
            DebugManager.Log<GameStatePause>("Saiu do estado de pausa");
        }
        
    }
}