using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.InputManager
{
    public class Manager : MonoBehaviour
    {
        private static Manager _instance;
        private static readonly object Lock = new object();

        public static Manager Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<Manager>();
                        if (_instance == null)
                        {
                            GameObject managerObject = new GameObject(nameof(Manager));
                            _instance = managerObject.AddComponent<Manager>();
                            managerObject.tag = "Manager";  // Adicione a tag correta.
                            DontDestroyOnLoad(managerObject);
                        }
                    }
                    return _instance;
                }
            }
        }

        private readonly Dictionary<string, List<Action<InputAction.CallbackContext>>> _actionListeners =
            new Dictionary<string, List<Action<InputAction.CallbackContext>>>();

        private readonly List<ComboSequence> _comboSequences = new List<ComboSequence>();

        private string _previousAction = "";
        private double _lastActionTime = -1;

        private float _maxComboTimeBetweenActions = 0.5f;

        public void SetMaxComboTime(float maxTime) => _maxComboTimeBetweenActions = maxTime;

        public void RegisterAction(string actionName, Action<InputAction.CallbackContext> callback)
        {
            if (!_actionListeners.TryGetValue(actionName, out var listener))
            {
                listener = new List<Action<InputAction.CallbackContext>>();
                _actionListeners[actionName] = listener;
            }

            listener.Add(callback);
        }

        public void UnregisterAction(string actionName, Action<InputAction.CallbackContext> callback)
        {
            if (_actionListeners.TryGetValue(actionName, out var listener))
            {
                listener.Remove(callback);
            }
        }

        public bool InputActionExists(string actionName) => _actionListeners.ContainsKey(actionName);

        public void TriggerAction(string actionName, InputAction.CallbackContext context)
        {
            if (_actionListeners.TryGetValue(actionName, out var actionListener))
            {
                foreach (var listener in actionListener)
                {
                    listener(context);
                }
            }

            CheckCombo(actionName, context);
        }

        internal void CheckCombo(string actionName, InputAction.CallbackContext context)
        {
            if (_lastActionTime < 0)
            {
                _lastActionTime = Time.realtimeSinceStartup;
                return;
            }

            double currentTime = Time.realtimeSinceStartup;

            // Restante do código...
        }

        internal void CheckCombo(string actionName, double currentTime, InputAction.CallbackContext context)
        {
            if (IsComboAction(actionName) && _previousAction != "" && _previousAction != actionName &&
                currentTime - _lastActionTime <= _maxComboTimeBetweenActions)
            {
                foreach (var comboSequence in _comboSequences)
                {
                    if (comboSequence.Matches(actionName, _previousAction))
                    {
                        TriggerComboEvent(comboSequence.Name, context);
                        ResetCombo();
                        break;
                    }
                }
            }

            _previousAction = actionName;
            _lastActionTime = currentTime;
        }

        public void AddComboSequence(string comboName, List<string> actionNames, float maxTimeBetweenActions)
        {
            _comboSequences.Add(new ComboSequence(comboName, actionNames, maxTimeBetweenActions));
        }

        private bool IsComboAction(string actionName) => actionName.StartsWith("ComboAction");

        private void ResetCombo()
        {
            _previousAction = "";
            _lastActionTime = -1;
        }

        private void TriggerComboEvent(string comboName, InputAction.CallbackContext context)
        {
            TriggerAction("Combo_" + comboName, context);
        }

        private void Awake()
        {
            Debug.Log("Manager Awake");
        }
    }
}
