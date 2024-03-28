using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;

namespace ImmersiveGames.InputManager
{
    public sealed class ActionManager
    {
        #region Events and Actions Handling
        
        internal event Action<string, InputAction.CallbackContext> EventOnActionTriggered;
        internal event Action EventOnActionComplete;
        
        private static readonly Dictionary<string, List<Action<InputAction.CallbackContext>>> ActionListeners =
                    new Dictionary<string, List<Action<InputAction.CallbackContext>>>();
        internal void RegisterAction(string actionName, Action<InputAction.CallbackContext> callback)
        {
            if (!ActionListeners.TryGetValue(actionName, out var listener))
            {
                listener = new List<Action<InputAction.CallbackContext>>();
                ActionListeners[actionName] = listener;
            }

            listener.Add(callback);
        }

        public void UnregisterAction(string actionName, Action<InputAction.CallbackContext> callback)
        {
            if (ActionListeners.TryGetValue(actionName, out var listener))
            {
                listener.Remove(callback);
            }
        }
        internal void NotifyObservers(string actionName, InputAction.CallbackContext context)
        {
            // Notifique todos os observadores globais
            EventOnActionTriggered?.Invoke(actionName, context);

            // Notifique observadores específicos para a ação
            if (!ActionListeners.TryGetValue(actionName, out var listeners)) return;
            foreach (var listener in listeners)
            {
                listener?.Invoke(context);
            }
        }
        #endregion
        
        #region Action Map Management
        
        private const GameActionMaps DefaultGameActionMaps = GameActionMaps.UiControls;
        private GameActionMaps _lastGameActionMaps;
        private GameActionMaps _currentGameActionMaps;
        public void RestoreActionMap()
        {
            ActivateActionMap(_lastGameActionMaps);
        }

        public void ActivateActionMap(GameActionMaps actionMapName)
        {
            _lastGameActionMaps = _currentGameActionMaps;
            // Desativa todos os Action Maps
            foreach (var actionMap in _inputActions.asset.actionMaps)
            {
                actionMap.Disable();
            }

            // Ativa o Action Map especificado
            var mapToActivate = _inputActions.asset.FindActionMap(actionMapName.ToString());
            if (mapToActivate != null)
            {
                mapToActivate.Enable();
                _currentGameActionMaps = actionMapName;
            }
            else
            {
                DebugManager.LogWarning($"Action Map '{actionMapName}' not found.");
            }
            DebugManager.Log($"[Action Map] '{actionMapName}' Active.");
        }
        
        #endregion
        
        #region Initialization and All Registration

        // Utiliza a instância de PlayersInputActions do initializer
        private readonly PlayersInputActions _inputActions;
        internal ActionManager(PlayersInputActions inputActions)
        {
            _inputActions = inputActions;
            RegisterAllActions();
        }

        private void RegisterAllActions()
        {
            var actionMaps = _inputActions.asset.actionMaps;
            foreach (var action in actionMaps.Select(actionMap => actionMap.actions).SelectMany(actions => actions))
            {
                if (IsSpecialAction(action))
                {
                    action.started += (context) => HandleSpecialAction(action, context);
                }
                else
                if (IsHoldAction(action))
                {
                    action.started += (context) => HandleHoldAction(action, context);
                    action.canceled += (context) => HandleHoldAction(action, context);
                }
                else
                {
                    action.performed += (context) => NotifyObservers(action.name, context);
                }
            }

            // Adicione um método para ativar/desativar Action Maps
            ActivateActionMap(DefaultGameActionMaps); // Troque "DefaultMap" pelo nome do seu Action Map padrão
        }
        
        #endregion
        

        #region Special Actions Handling

        private void HandleSpecialAction(InputAction action, InputAction.CallbackContext context)
        {
            // Lógica específica para ação especial;
            DebugManager.Log($"Special Action {action.name} Performed in context {context}");
        }

        private bool IsSpecialAction(InputAction action)
        {
            // Adicione aqui a lógica para determinar se uma ação é especial ou não
            return action.name.StartsWith("Special");
        }
        private void HandleHoldAction(InputAction action, InputAction.CallbackContext context)
        {
            // Lógica específica para ação especial;
            if (context.started)
            {
                var completeName = action.name + "_Start";
                DebugManager.Log($"Hold Action {completeName} Performed in Performed");
                
                ActionListeners.TryGetValue(completeName, out var listener);
                if (listener != null)
                {
                    foreach (var ativeAction in listener)
                    {
                        ativeAction?.Invoke(context);
                    }
                }
            }
            if (context.canceled)
            {
                var completeName = action.name + "_Cancel";
                DebugManager.Log($"Hold Action {completeName} Performed in Cancel");
                
                ActionListeners.TryGetValue(completeName, out var listener);
                if (listener == null) return;
                foreach (var ativeAction in listener)
                {
                    ativeAction?.Invoke(context);
                }

            }
            
        }
        private bool IsHoldAction(InputAction action)
        {
            // Adicione aqui a lógica para determinar se uma ação é especial ou não
            return action.name.StartsWith("Hold_");
        }

        #endregion
        public enum GameActionMaps
        {
            Player, UiControls, BriefingRoom, Notifications, Shopping, HUD
        }

        public void OnEventOnActionComplete()
        {
            EventOnActionComplete?.Invoke();
        }
    }
    
}
