using UnityEngine;
namespace RiverAttack
{
    public class GameStateHub: GameState
    {

        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: HUB");
        }
        public override void UpdateState()
        {
            Debug.Log($"Game HUB!");
        }
        public override void ExitState()
        {
            Debug.Log($"Sai do Estado: HUB");
        }
    }
}
