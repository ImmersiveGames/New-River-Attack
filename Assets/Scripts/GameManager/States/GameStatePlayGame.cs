using UnityEngine;
namespace RiverAttack
{
    public class GameStatePlayGame : GameState
    {
        readonly GamePlayManager m_GamePlayManager;
        readonly PlayerManager m_PlayerManager;
        
        public override void OnLoadState()
        {
            
        }
        public override void EnterState()
        {
            GamePlayManager.instance.panelMenuGame.StartMenuGame();
           Debug.Log($"Entra no Estado: PlayGame");
            //m_GameManager.PanelBase.SetMenuPrincipal();
            //m_GameManager.startMenu.SetMenuHudControl(true);
            /*if (!m_PlayerManager.haveAnyPlayerInitialized)
                m_PlayerManager.InstantiatePlayers();*/

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
            
            Debug.Log($"Sai do Estado: PlayGame");
        }
    }
}
