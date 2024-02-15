using System.Collections;
using UnityEngine;
namespace RiverAttack
{
    public class GameStatePause : GameState
    {
        public override IEnumerator OnLoadState()
        {
            //Debug.Log($"On Load Estado: Pause");
            
            yield return null;
            
        }
        public override void EnterState()
        {
            Time.timeScale = 0;
            GameManager.instance.inputSystem.Player.Disable();
            GameManager.instance.inputSystem.UI_Controlls.Enable();
            GameManager.instance.inputSystem.BriefingRoom.Disable();
            GamePlayManager.instance.panelMenuGame.PauseMenu(true);
            //Debug.Log($"Entra no Estado: Pause");
        }
        public override void UpdateState()
        {
            Debug.Log($"Game Pausado!");
        }
        public override void ExitState()
        {
            //Debug.Log($"Sai do Estado: Pause");
            Time.timeScale = 1;
            GamePlayManager.instance.panelMenuGame.PauseMenu(false);
        }
    }
}
