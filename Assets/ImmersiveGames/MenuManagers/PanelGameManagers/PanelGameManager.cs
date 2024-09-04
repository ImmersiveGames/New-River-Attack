using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.MenuManagers.PanelGameManagers
{
    public sealed class PanelGameManager : MonoBehaviour
    {
        private PanelGameHud _panelGameHud;
        private PanelGamePause _panelGamePause;
        private GamePlayManager _gamePlayManager;

        #region Delegates

        public delegate void PanelGameHandle();

        public event PanelGameHandle EventPauseGame;
        public event PanelGameHandle EventUnPauseGame;
        public event PanelGameHandle EventHudGame;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            InputGameManager.RegisterAction("PauseGame", StartPauseMenu );
            GamePlayManager.Instance.EventGameReady += SetHudMenu;
        }

        private void OnDisable()
        {
            GamePlayManager.Instance.EventGameReady -= SetHudMenu;
            InputGameManager.UnregisterAction("Pause", StartUnPauseMenu );
            InputGameManager.UnregisterAction("PauseGame", StartPauseMenu );
        }

        private void SetHudMenu()
        {
            _panelGameHud.gameObject.SetActive(true);
            _panelGamePause.gameObject.SetActive(false);
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _panelGameHud = GetComponentInChildren<PanelGameHud>(true);
            _panelGamePause = GetComponentInChildren<PanelGamePause>(true);
        }
        
        private void StartPauseMenu(InputAction.CallbackContext context)
        {
            DebugManager.Log<PanelGameHud>("CALL PAUSE");
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            InputGameManager.RegisterAction("Pause", StartUnPauseMenu );
            _panelGameHud.gameObject.SetActive(false);
            _panelGamePause.gameObject.SetActive(true);
            _gamePlayManager.OnEventGamePause();
        }
        private void StartUnPauseMenu(InputAction.CallbackContext context)
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Player);
            InputGameManager.RegisterAction("PauseGame", StartPauseMenu );
            DebugManager.Log<PanelGameHud>("CALL UNPAUSE");
            _panelGameHud.gameObject.SetActive(true);
            _panelGamePause.gameObject.SetActive(false);
            _gamePlayManager.OnEventGameUnPause();
        }
        public void ButtonReturnMenu()
        {
            Time.timeScale = 1;
            //InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            _= GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(true);
        }
        
        
        #region Call Events

        private void OnEventPauseGame()
        {
            EventPauseGame?.Invoke();
        }
        private void OnEventUnPauseGame()
        {
            EventUnPauseGame?.Invoke();
        }
        private void OnEventHudGame()
        {
            EventHudGame?.Invoke();
        }
        #endregion


        
    }
}