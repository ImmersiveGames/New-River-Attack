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
    public class PanelGamePause : MonoBehaviour
    {
        /*private void Awake()
        {
            gameObject.SetActive(false);
            GamePlayManager.instance.EventGamePause += SetPauseGame;
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
            GamePlayManager.instance.OnEventGameUnPause();
        }

        public void ButtonReturnMenu()
        {
            Time.timeScale = 1;
            //InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            _= GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);
        }*/
    }
}