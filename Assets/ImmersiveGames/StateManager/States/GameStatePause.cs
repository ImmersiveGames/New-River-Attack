using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGames
{
    public class GameStatePause : GameState
    {
        public GameStatePause() : base("GameStatePause") { }
        public override string sceneName => "Pause";
        public override bool requiresSceneLoad => true;
        public override LoadSceneMode loadMode => LoadSceneMode.Additive;
        public override bool unLoadAdditiveScene => false;
        public override ITransition inTransition => new FadeTransition();
        public override ITransition outTransition => new FadeTransition();
        protected override async Task OnEnter(IState previousState)
        {
            // Lógica específica ao entrar no estado de pausa
            await base.OnEnter(previousState).ConfigureAwait(false);
            Debug.Log("Entrou no estado de pausa");
        }

        public override void UpdateState()
        {
            // Lógica de atualização do estado de pausa
            Debug.Log($"Update no estado de pausa, initialized: {stateInitialized}");
        }

        protected override async Task OnExit()
        {
            // Lógica específica ao sair do estado de pausa em outras trheds
            await base.OnExit().ConfigureAwait(false);
            Debug.Log("Saiu do estado de pausa");
        }
        
    }
}