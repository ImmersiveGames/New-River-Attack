using System.Collections;
using UnityEngine;
namespace RiverAttack
{
    public class GameStatePause : GameState
    {
        
        public override IEnumerator OnLoadState()
        {
            //Debug.Log($"On Load Estado: Pause");
            GamePlayManager.instance.inputSystem.UI_Controlls.Enable();
            yield return null;
            
        }
        public override void EnterState()
        {
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
            GamePlayManager.instance.inputSystem.UI_Controlls.Disable();
            GamePlayManager.instance.panelMenuGame.PauseMenu(false);
        }
    }
}
