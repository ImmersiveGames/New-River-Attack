using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.MenuManagers.Abstracts;
using NewRiverAttack.GamePlayManagers;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers
{
    public class GameStartMenu : AbstractMenuManager
    {
        public PanelsMenuReference[] panelsMenuReferences;
        public Button[] disableButtons;
        public bool activeScreenWash;
        
        #region Unity Methods

        private void Awake()
        {
            SetMenu(panelsMenuReferences);
        }

        private void Start()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            //InputGameManager.RegisterAction("BackButton",InputBackButton );
            //ActivateMenu(0);
            DebugManager.Log<GameStartMenu>("Inicia o menu.");
        }

        private void OnDisable()
        {
            InputGameManager.ActionManager.RestoreActionMap();
            GamePlayManager.instance.EventGameOver -= OnGameOver;
        }
        
        private void OnDestroy()
        {
            //InputGameManager.UnregisterAction("BackButton",InputBackButton );
        }
        #endregion
        protected override void OnEnterMenu(PanelsMenuReference panelsMenuGameObject)
        {
            DebugManager.Log<GameStartMenu>("Entrando no menu.");
        }

        protected override void OnExitMenu(PanelsMenuReference panelsMenuGameObject)
        {
            DebugManager.Log<GameStartMenu>("Saindo Do Menu");
        }

        private void OnGameOver()
        {
            ActivateMenu(1);
        }
    }
}