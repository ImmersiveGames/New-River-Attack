using UnityEngine;
namespace RiverAttack
{
    public class GameStatePause : GameState
    {
        readonly GameManager m_GameManager;
        
        public GameStatePause()
        {
            m_GameManager = GameManager.instance;
        }
        public override void OnLoadState()
        {
            throw new System.NotImplementedException();
        }
        public override void EnterState()
        {
            //m_GameManager.startMenu.SetMenuPrincipal();
            //m_GameManager.startMenu.SetMenuHudControl(false);
            //Debug.Log($"Entra no Estado: Pause");
        }
        public override void UpdateState()
        {
            //Debug.Log($"Game Pausado!");
        }
        public override void ExitState()
        {
            //Debug.Log($"Sai do Estado: Pause");
        }
    }
}
