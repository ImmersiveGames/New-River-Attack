using System.Collections;
namespace RiverAttack
{
    public class GameStateTutorial: GameState
    {
        private const float TIME_TO_FADE_BGM = 0.01f;
        public override IEnumerator OnLoadState()
        {
            yield return null;
        }
        public override void EnterState()
        {
            //Debug.Log($"Entrando no Estado: Tutorial");
            GameManager.instance.inputSystem.Player.Disable();
            GameManager.instance.inputSystem.UiControls.Disable();
            GameManager.instance.inputSystem.BriefingRoom.Enable();
            GameAudioManager.instance.ChangeBGM(BgmTypes.Tutorial, TIME_TO_FADE_BGM);
        }
        public override void UpdateState()
        {
            //Debug.Log($"Tutorial");
        }
        public override void ExitState()
        {
            //Debug.Log($"Saindo no Estado: Tutorial");
        }
    }
}
