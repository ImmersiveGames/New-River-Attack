using System.Collections;
using UnityEngine;
namespace RiverAttack
{
    public class GameStateMenu : GameState
    {
        private const float TIME_TO_FADE_BGM = 0.01f;
        public override IEnumerator OnLoadState()
        {
            yield return null;
        }
        public override void EnterState()
        {
            //Debug.Log($"Entra no Estado: Menu");
            GameManager.instance.inputSystem.Player.Disable();
            GameManager.instance.inputSystem.UiControls.Enable();
            GameManager.instance.inputSystem.BriefingRoom.Disable();
            PlayerManager.instance.ActivePlayers(false);
            GameAudioManager.instance.ChangeBGM(BgmTypes.Menu, TIME_TO_FADE_BGM);
        }
        public override void UpdateState()
        {
           //Debug.Log($"Rodando no Estado: Menu");
        }
        public override void ExitState()
        {
            //Debug.Log($"Saindo no Estado: Menu");
        }
    }
}
