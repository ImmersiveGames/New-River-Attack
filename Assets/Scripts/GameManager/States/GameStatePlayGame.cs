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
            GamePlayManager.instance.inputSystem.Player.Enable();
            GamePlayManager.instance.inputSystem.UI_Controlls.Disable();
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
