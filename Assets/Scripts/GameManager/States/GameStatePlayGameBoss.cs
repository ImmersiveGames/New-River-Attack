using System.Collections;
using UnityEngine;
namespace RiverAttack
{
    public class GameStatePlayGameBoss : GameState
    {
        public override IEnumerator OnLoadState()
        {
            yield return null;
        }
        public override void EnterState()
        {
            GamePlayManager.instance.panelMenuGame.StartMenuGame();
            Debug.Log($"Entra no Estado: Boss Fight");

            //TODO: dar mais tempo para o pause;
            GamePlayManager.instance.OnStartGame();
        }
        public override void UpdateState()
        {
            Debug.Log("Boss Fight!");
        }
        public override void ExitState()
        {
            PlayerManager.instance.ActivePlayers(false);
            GamePlayManager.instance.OnEventDeactivateEnemiesMaster();
           
            Debug.Log($"Sai do Estado: Boss Fight");
            System.GC.Collect();
        }
    }
}
