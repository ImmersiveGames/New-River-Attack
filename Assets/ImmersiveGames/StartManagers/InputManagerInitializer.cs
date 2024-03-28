using System;
using System.Threading;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames
{
    public class InputManagerInitializer : MonoBehaviour
    {
        private static PlayersInputActions _inputActions;
        private static ActionManager _actionManager;

        // Propriedades para acessar instâncias únicas de PlayersInputActions e ActionManager
        public static PlayersInputActions InputActions
        {
            get
            {
                if (_inputActions != null) return _inputActions;
                _inputActions = new PlayersInputActions();
                _inputActions.Enable();
                return _inputActions;
            }
        }

        internal static ActionManager ActionManager
        {
            get
            {
                if (_actionManager != null) return _actionManager;
                _actionManager = new ActionManager(InputActions);
                return _actionManager;
            }
        }
        protected internal static void RegisterAction(string actionName, Action<InputAction.CallbackContext> callbackComplete, Action<InputAction.CallbackContext> callbackCancel)
        {
            ActionManager.RegisterAction($"{actionName}_Start", callbackComplete);
            ActionManager.RegisterAction($"{actionName}_Cancel", callbackCancel);
        }
        protected internal static void RegisterAction(string actionName, Action<InputAction.CallbackContext> callback)
        {
            ActionManager.RegisterAction(actionName, callback);
        }
        protected internal static void UnregisterAction(string actionName, Action<InputAction.CallbackContext> callback)
        {
            ActionManager.UnregisterAction(actionName, callback);
        }

        protected static void DisableInputs()
        {
            _inputActions?.Disable();
        }

        private void OnDestroy()
        {
            // Desativa os Inputs ao desabilitar o objeto
            _inputActions?.Disable();
            _actionManager = null;
        }
    }
}