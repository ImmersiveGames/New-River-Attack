using UnityEngine;
namespace RiverAttack
{
    public class GameStateEndGame : GameState
    {
        public override void OnLoadState()
        {
            //throw new System.NotImplementedException();
            GameManager.instance.DestroyGamePlay();
        }

        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: EndGame");
        }
        public override void UpdateState()
        {
            Debug.Log($"End Game - Credits");
        }
        public override void ExitState()
        {
            Debug.Log($"Saida no Estado: End Game");
        }
    }
}
