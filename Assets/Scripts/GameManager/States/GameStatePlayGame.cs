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
            int score = PlayerManager.instance.playerSettingsList[0].score;
            GameSteamManager.UpdateScore(score, false);
            PlayerManager.instance.ActivePlayers(false);
            GamePlayManager.instance.OnEventDeactivateEnemiesMaster();
            GameSaveManager.instance.SavePlayerSaves();
            //Debug.Log($"Sai do Estado: PlayGame");
            System.GC.Collect();
        }
        
    }
}
