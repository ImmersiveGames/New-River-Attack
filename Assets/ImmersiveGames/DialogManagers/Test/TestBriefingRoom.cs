﻿using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.DialogManagers.Test
{
    public class TestBriefingRoom: MonoBehaviour
    {
        private async void Start()
        {
            if (InitializationManager.StateManager == null)
            {
                var stateManager = new StateManager();
                // Adicione os estados ao StateManager
                stateManager.AddState(new GameStateBriefingRoom());
                await stateManager.ChangeStateAsync("GameStateBriefingRoom").ConfigureAwait(false);
                InputManagerInitializer.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.BriefingRoom);
            }
            
        }
        
        private void InputExitDialogCancel(InputAction.CallbackContext context)
        {
            DebugManager.Log<TestBriefingRoom>($"Cancelou o Botão B");
     
        }
        private void InputExitDialog(InputAction.CallbackContext context)
        {

            DebugManager.Log<TestBriefingRoom>($"iniciou o Botão B");

        }
    }
}