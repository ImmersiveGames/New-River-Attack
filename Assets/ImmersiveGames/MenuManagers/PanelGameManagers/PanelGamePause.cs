using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.MenuManagers.PanelGameManagers
{
    public class PanelGamePause : MonoBehaviour
    {
        
        private void Awake()
        {
            gameObject.SetActive(false);
            GamePlayManager.instance.EventGamePause += SetPauseGame;
        }
        private void SetPauseGame()
        {
            DebugManager.Log<PanelGamePause>("PAUSE");
            gameObject.SetActive(true);
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