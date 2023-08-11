using UnityEngine;
namespace RiverAttack
{
    public class GameStatePlayGame: GameState
    {
        GamePlayManager m_GamePlayManager;
        public override void EnterState()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlayManager.UnPauseGame();
            Debug.Log($"Entra no Estado: PlayGame");
        }
        public override void UpdateState()
        {
            Debug.Log($"PlayGame!");
        }
        public override void ExitState()
        {
            Debug.Log($"Sai do Estado: PlayGame");
            m_GamePlayManager = GamePlayManager.instance;
            m_GamePlayManager.PauseGame();
        }
    }
}
