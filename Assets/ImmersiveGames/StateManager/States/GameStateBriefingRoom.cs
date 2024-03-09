using ImmersiveGames.InputManager;
using ImmersiveGames.ScenesManager.Transitions;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManager.States
{
    public class GameStateBriefingRoom: GameState
    {
        public GameStateBriefingRoom() : base("GameStateBriefingRoom") { }


        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override bool requiresSceneLoad => true;
        public override string sceneName => "BriefingRoom";
        public override LoadSceneMode loadMode => LoadSceneMode.Single;
        public override bool unLoadAdditiveScene => true;
        public override ITransition inTransition => new FadeTransition();
        public override ITransition outTransition => new FadeTransition();
        protected override GameActionMaps stateInputActionMap => GameActionMaps.BriefingRoom;
    }
}