﻿using ImmersiveGames.DebugManagers;
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
        private PanelGameOver _panelGameOver;
        private PanelGameComplete _panelGameComplete;
        private GamePlayManager _gamePlayManager;

        private bool _inLoad;
        
        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            SetupInitial();
            InputGameManager.UnregisterAction("PauseGame", StartPauseMenu );
            InputGameManager.RegisterAction("PauseGame", StartPauseMenu );
            _gamePlayManager.EventGameReady += SetHudMenu;
            _gamePlayManager.EventGameOver += SetGameOverMenu;
            _gamePlayManager.EventGameFinisher += SetGameComplete;
        }

        private void Start()
        {
            _inLoad = false;
        }

        private void OnDestroy()
        {
            _gamePlayManager.EventGameReady -= SetHudMenu;
            _gamePlayManager.EventGameFinisher -= SetGameComplete;
            InputGameManager.UnregisterAction("Pause", StartUnPauseMenu );
            InputGameManager.UnregisterAction("PauseGame", StartPauseMenu );
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _panelGameHud = GetComponentInChildren<PanelGameHud>(true);
            _panelGamePause = GetComponentInChildren<PanelGamePause>(true);
            _panelGameOver = GetComponentInChildren<PanelGameOver>(true);
            _panelGameComplete = GetComponentInChildren<PanelGameComplete>(true);
            
        }
        private void SetHudMenu()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Player);
            InputGameManager.UnregisterAction("PauseGame", StartPauseMenu );
            InputGameManager.RegisterAction("PauseGame", StartPauseMenu );
            _panelGameHud.gameObject.SetActive(true);
            _panelGamePause.gameObject.SetActive(false);
            _panelGameOver.gameObject.SetActive(false);
            _panelGameComplete.gameObject.SetActive(false);
        }

        private void SetPauseMenu()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            InputGameManager.UnregisterAction("Pause", StartUnPauseMenu );
            InputGameManager.RegisterAction("Pause", StartUnPauseMenu );
            _panelGameHud.gameObject.SetActive(false);
            _panelGamePause.gameObject.SetActive(true);
            _panelGameOver.gameObject.SetActive(false);
            _panelGameComplete.gameObject.SetActive(false);
        }
        
        private void SetGameOverMenu()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            InputGameManager.UnregisterAction("Pause", StartPauseMenu );
            _panelGameHud.gameObject.SetActive(false);
            _panelGamePause.gameObject.SetActive(false);
            _panelGameOver.gameObject.SetActive(true);
            _panelGameComplete.gameObject.SetActive(false);
        }
        private void SetGameComplete()
        {
            _panelGameHud.gameObject.SetActive(false);
            _panelGamePause.gameObject.SetActive(false);
            _panelGameOver.gameObject.SetActive(false);
            _panelGameComplete.gameObject.SetActive(true);
        }
        
        private void StartPauseMenu(InputAction.CallbackContext context)
        {
            ButtonPause();
        }
        private void StartUnPauseMenu(InputAction.CallbackContext context)
        {
            ButtonContinue();
        }
        public void ButtonReturnMenu()
        {
            Time.timeScale = 1;
            _= GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(true);
        }
        public void ButtonReturnHub()
        {
            Time.timeScale = 1;
            _= GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateHub.ToString()).ConfigureAwait(true);
        }

        private void ButtonPause()
        {
            if (!_gamePlayManager.ShouldBePlayingGame) return;
            if (_inLoad) return;
            _inLoad = true;
            DebugManager.Log<PanelGameHud>("CALL PAUSE");
            SetPauseMenu();
            _gamePlayManager.OnEventGamePause();
            _inLoad = false;
        }

        public void ButtonContinue()
        {
            if (!_gamePlayManager.ShouldBePlayingGame) return;
            if (_inLoad) return;
            _inLoad = true;
            DebugManager.Log<PanelGameHud>($"CALL UNPAUSE {_gamePlayManager.ShouldBePlayingGame}");
            SetHudMenu();
            _gamePlayManager.OnEventGameUnPause();
            _inLoad = false;
        }

        public void ButtonReload()
        {
            
            _gamePlayManager.OnEventGameReload();
            SetupInitial();
        }
        private void SetupInitial()
        {
            _panelGameHud.gameObject.SetActive(false);
            _panelGamePause.gameObject.SetActive(false);
            _panelGameOver.gameObject.SetActive(false);
            _panelGameComplete.gameObject.SetActive(false);
        }
    }
}