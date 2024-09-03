using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.MenuManagers.PanelGameManagers
{
    public class PanelGameHud : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
            GamePlayManager.instance.EventGameReady += SetHudMenu;
            GamePlayManager.instance.EventGameUnPause += SetHudMenu;
        }
        
        private void SetHudMenu()
        {
            gameObject.SetActive(true);
            InputGameManager.RegisterAction("Pause", StartPauseMenu );
        }
        private void StartPauseMenu(InputAction.CallbackContext obj)
        {
            DebugManager.Log<PanelGameHud>("CALL PAUSE");
            GamePlayManager.instance.OnEventGamePause();
        }
        /*#region UnityMethods

        private void Awake()
        {
            gameObject.SetActive(false);
            GamePlayManager.instance.EventGameReady += SetHudMenu;
            GamePlayManager.instance.EventGameUnPause += SetHudMenu;
        }

        private void OnEnable()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Player);
            InputGameManager.RegisterAction("Pause", StartPauseMenu );
            
            GamePlayManager.instance.EventGamePause += DisableHud;
        }

        private void OnDisable()
        {
            //InputGameManager.ActionManager.RestoreActionMap();
            
            GamePlayManager.instance.EventGamePause -= DisableHud;
        }

        private void OnDestroy()
        {
            GamePlayManager.instance.EventGameReady -= SetHudMenu;
            InputGameManager.ActionManager.UnregisterAction("Pause", StartPauseMenu);
        }

        #endregion
        
        private void SetHudMenu()
        {
            gameObject.SetActive(true);
        }
        private void DisableHud()
        {
            
            gameObject.SetActive(false);
        }
        private void StartPauseMenu(InputAction.CallbackContext obj)
        {
            DebugManager.Log<PanelGameHud>("CALL PAUSE");
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);
            GamePlayManager.instance.OnEventGamePause();
        }*/
    }
}