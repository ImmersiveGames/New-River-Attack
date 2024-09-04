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
        /*private void Awake()
        {
            gameObject.SetActive(false);
            GamePlayManager.instance.EventGameReady += SetHudMenu;
            GamePlayManager.instance.EventGameUnPause += SetHudMenu;
        }
        private void OnDestroy()
        {
            GamePlayManager.instance.EventGameReady -= SetHudMenu;
            GamePlayManager.instance.EventGameUnPause -= SetHudMenu;
        }

        private void SetHudMenu()
        {
            gameObject.SetActive(true);
            InputGameManager.RegisterAction("PauseGame", StartPauseMenu );
        }
        private void StartPauseMenu(InputAction.CallbackContext obj)
        {
            DebugManager.Log<PanelGameHud>("CALL PAUSE");
            DisableGameObject();
            GamePlayManager.instance.OnEventGamePause();
        }

        private void DisableGameObject()
        {
            gameObject.SetActive(false); 
        }*/
    }
}