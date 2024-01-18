using System;
using System.Collections;
namespace RiverAttack
{
    public class GameStateEndGame : GameState
    {
        public override IEnumerator OnLoadState()
        {
            //throw new System.NotImplementedException();
            GameManager.DestroyGamePlay();
            yield return null;
        }

        public override void EnterState()
        {
            //Debug.Log($"Entra no Estado: EndGame");
            switch (GameManager.instance.gameModes)
            {
                case GameManager.GameModes.Classic:
                    GameSteamManager.UnlockAchievement("ACH_FINISH_CLASSIC");
                    break;
                case GameManager.GameModes.Mission:
                    GameSteamManager.UnlockAchievement("ACH_FINISH_MISSION");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public override void UpdateState()
        {
            //Debug.Log($"End Game - Credits");
        }
        public override void ExitState()
        {
            //Debug.Log($"Saida no Estado: End Game");
        }
    }
}
