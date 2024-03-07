using ImmersiveGames.InputManager.InputActionAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.InputManager
{
    public class InputManagerxxx : MonoBehaviour
    {
        [SerializeField] private InputActionAssetReference playerInputAssetReference;
        private InputActionMap _selectedInputActionMap;
        private Manager _eventManager;

        private void Awake()
        {
            _eventManager = Manager.Instance;
            Debug.Log("InputManager Awake");
            Initialize();
        }

        public void Initialize()
        {
            if (playerInputAssetReference == null)
            {
                Debug.LogError("InputActionAssetReference not assigned!");
                return;
            }

            var playerInputAsset = playerInputAssetReference.inputActionAsset;

            if (playerInputAsset == null)
            {
                Debug.LogError("InputActionAsset not assigned in InputActionAssetReference!");
                return;
            }

            // Carrega a configuração externa
            var config = Resources.Load<InputManagerConfig>("InputManagerConfig");

            if (config == null)
            {
                Debug.LogError("InputManagerConfig not found in Resources!");
                return;
            }

            // Seleciona o InputActionMap desejado
            _selectedInputActionMap = playerInputAsset.FindActionMap(config.selectedInputActionMapName);

            if (_selectedInputActionMap == null)
            {
                Debug.LogError($"InputActionMap '{config.selectedInputActionMapName}' not found!");
                return;
            }

            var comboAction = _selectedInputActionMap.FindAction(config.escNotificationActionName);

            if (comboAction == null)
            {
                Debug.LogError($"{config.escNotificationActionName} not found in the selected InputActionMap!");
                return;
            }

            RegisterActionEvents(_selectedInputActionMap);
            comboAction.performed += OnActionCombo;
        }

        private void OnActionCombo(InputAction.CallbackContext context)
        {
            _eventManager.TriggerAction("Combo", context);
        }

        private void RegisterActionEvents(InputActionMap actionMap)
        {
            foreach (var action in actionMap.actions)
            {
                action.performed += OnActionPerformed;
                action.canceled += OnActionCanceled;
            }
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            _eventManager.TriggerAction(context.action.name, context);

            if (IsComboAction(context.action.name))
            {
                _eventManager.CheckCombo(context.action.name, context.time, context);
            }
        }

        private void OnActionCanceled(InputAction.CallbackContext context)
        {
            _eventManager.TriggerAction(context.action.name + "_Canceled", context);
        }

        private static bool IsComboAction(string actionName)
        {
            return actionName.StartsWith("ComboAction");
        }
    }
}
