using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.MenuManagers.PanelGameManagers
{
    public class PanelGamePause : MonoBehaviour
    {
        [SerializeField] private RectTransform cursor;
        [SerializeField] private RectTransform[] menus;
        
        private void Awake()
        {
            gameObject.SetActive(false);
            GamePlayManager.instance.EventGamePause += SetPauseGame;
        }

        private void OnEnable()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            InputGameManager.RegisterAction("Pause", StartPauseMenu );
        }

        private void StartPauseMenu(InputAction.CallbackContext obj)
        {
            ButtonContinue();
        }

        private void SetPauseGame()
        {
            DebugManager.Log<PanelGamePause>("PAUSE");
            gameObject.SetActive(true);
        }

        public void ButtonContinue()
        {
            DebugManager.Log<PanelGamePause>("Ação do MENU");
            gameObject.SetActive(false);
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Player);
            
        }

        public async void ButtonReturnMenu()
        {
            gameObject.SetActive(false);
            GamePlayManager.instance.OnEventGameUnPause();
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);
        }
        /*

        #region UnityMethods

        private void OnEnable()
        {
            
            DebugManager.Log<PanelGamePause>("Teste Enable");
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            //InputGameManager.RegisterAction("Pause", StartUnPauseMenu );
            
            GamePlayManager.instance.EventGameUnPause += SetUnPauseGame;
        }

        private void OnDisable()
        {
            //InputGameManager.ActionManager.RestoreActionMap();
            GamePlayManager.instance.EventGameUnPause -= SetUnPauseGame;
        }

        private void OnDestroy()
        {
            GamePlayManager.instance.EventGamePause -= SetPauseGame;
        }

        #endregion
        private void StartUnPauseMenu(InputAction.CallbackContext obj)
        {
            DebugManager.Log<PanelGamePause>("CALL UNPAUSE");
            GamePlayManager.instance.OnEventGameUnPause();
            SetUnPauseGame();
        }
        private void SetPauseGame()
        {
            DebugManager.Log<PanelGamePause>("PAUSE");
            gameObject.SetActive(true);
        }
        private void SetUnPauseGame()
        {
            DebugManager.Log<PanelGamePause>("UNPAUSE");
            gameObject.SetActive(false);
        }*/
    }
}