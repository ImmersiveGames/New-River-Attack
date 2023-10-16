namespace RiverAttack
{
    public class GameStatePlayGame : GameState
    {
        readonly GamePlayManager m_GamePlayManager;
        readonly PlayerManager m_PlayerManager;
        public GameStatePlayGame()
        {
            m_GamePlayManager = GamePlayManager.instance;
        }
        public override void EnterState()
        {
           // Debug.Log($"Entra no Estado: PlayGame");
            //m_GameManager.PanelBase.SetMenuPrincipal();
            //m_GameManager.startMenu.SetMenuHudControl(true);
            if (!m_PlayerManager.haveAnyPlayerInitialized)
                m_PlayerManager.InstantiatePlayers();

            //TODO: dar mais tempo para o pause;
            m_GamePlayManager.OnStartGame();
        }
        public override void UpdateState()
        {
            //Debug.Log($"PlayGame!");
        }
        public override void ExitState()
        {
            m_PlayerManager.ActivePlayers(false);
            m_GamePlayManager.OnEventDeactivateEnemiesMaster();
            
            //Debug.Log($"Sai do Estado: PlayGame");
        }
    }
}
