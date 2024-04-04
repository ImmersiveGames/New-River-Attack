using UnityEngine;
using System.Collections;
namespace RiverAttack
{
    public class GameStatePlayGame : GameState
    {
        public override IEnumerator OnLoadState()
        {
            yield return null;
        }
        public override void EnterState()
        {
            GameManager.instance.inputSystem.Player.Enable();
            GameManager.instance.inputSystem.UiControls.Disable();
            GameManager.instance.inputSystem.BriefingRoom.Disable();
            GamePlayManager.instance.panelMenuGame.StartMenuGame();
            //Debug.Log($"Entra no Estado: PlayGame");
            
            //TODO: dar mais tempo para o pause;
            GamePlayManager.instance.OnStartGame();
        }
        public override void UpdateState()
        {
            Debug.Log("PlayGame!");
        }
        public override void ExitState()
        {
            var score = PlayerManager.instance.playerSettingsList[0].score;
            GameSaveManager.instance.SavePlayerSaves();
            GameSteamManager.UpdateScore(score, false);
            //PlayerManager.instance.ActivePlayers(false);
            GamePlayManager.instance.OnEventDeactivateEnemiesMaster();            
            //Debug.Log($"Sai do Estado: PlayGame");
            System.GC.Collect();
        }
        
    }
}
