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
            PlayerManager.instance.ActivePlayers(false);
            GamePlayManager.instance.OnEventDeactivateEnemiesMaster();
           
            //TODO: Atualizar o Leaderboards
            //Debug.Log($"Sai do Estado: PlayGame");
            System.GC.Collect();
        }
    }
}
