using UnityEngine;
namespace RiverAttack
{
    public class GameStatePlayGame: GameState
    {
        readonly GameManager m_GameManager;
        readonly GamePlayManager m_GamePlayManager;
        public GameStatePlayGame()
        {
            m_GameManager = GameManager.instance;
            m_GamePlayManager = GamePlayManager.instance;
        }
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: PlayGame");
            m_GameManager.startMenu.SetMenuPrincipal(1,false);
            m_GameManager.startMenu.SetMenuHudControl(true);
            if(!m_GameManager.haveAnyPlayerInitialized)
                m_GameManager.InstantiatePlayers();
            m_GameManager.ActivePlayers(true);
            m_GameManager.UnPausedMovementPlayers();
            m_GamePlayManager.OnEventActivateEnemiesMaster();
        }
        public override void UpdateState()
        {
            Debug.Log($"PlayGame!");
        }
        public override void ExitState()
        {
            m_GamePlayManager.OnEventDeactivateEnemiesMaster();
            m_GameManager.ActivePlayers(false);
            Debug.Log($"Sai do Estado: PlayGame");
        }
    }
}
