using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using RiverAttack;

namespace ImmersiveGames.InputManager
{
    public class ActionManager
    {
        public static event Action<string, InputAction.CallbackContext> EventOnActionTriggered;

        // Utiliza a instância de PlayersInputActions do initializer
        private static PlayersInputActions _inputActions;

        private static readonly Dictionary<string, List<Action<InputAction.CallbackContext>>> ActionListeners =
            new Dictionary<string, List<Action<InputAction.CallbackContext>>>();

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
                {
                    action.started += (context) => NotifyObservers(action.name, context);
                }
            }

            // Adicione um método para ativar/desativar Action Maps
            ActivateActionMap(GameActionMaps.Player); // Troque "DefaultMap" pelo nome do seu Action Map padrão
        }

        public void ActivateActionMap(GameActionMaps actionMapName)
        {
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
            }
            else
            {
                Debug.LogWarning($"Action Map '{actionMapName}' not found.");
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
                listener.Invoke(context);
            }
        }

        private void HandleSpecialAction(InputAction action, InputAction.CallbackContext context)
        {
            // Lógica específica para ação especial
            Debug.Log($"Special Action {action.name} Performed");
        }

        private bool IsSpecialAction(InputAction action)
        {
            // Adicione aqui a lógica para determinar se uma ação é especial ou não
            return action.name.StartsWith("Special");
        }

        public void RegisterAction(string actionName, Action<InputAction.CallbackContext> callback)
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
    }

    public enum GameActionMaps
    {
        Player, UiControls, BriefingRoom
    }
}
