using ImmersiveGames.DebugManagers;
using RiverAttack;
using UnityEngine;

namespace ImmersiveGames.InputManager.Tests
{
    public class ActionManagerTest : MonoBehaviour
    {
        private PlayersInputActions _inputActions;
        private ActionManager _actionManager;

        private void Start()
        {
            // Inicializa o PlayersInputActions
            _inputActions = new PlayersInputActions();
            _inputActions.Enable();

            // Inicializa o ActionManager com a instância de PlayersInputActions
            _actionManager = new ActionManager(_inputActions);

            // Teste: Ativar Action Map específico
            TestActivateActionMap(GameActionMaps.UiControls);
        }

        private void OnDestroy()
        {
            // Certifique-se de desativar o PlayersInputActions ao destruir o objeto
            _inputActions?.Disable();

            // Certifique-se de desativar o ActionManager ao destruir o objeto
            _actionManager = null;
        }

        private void TestActivateActionMap(GameActionMaps actionMapName)
        {
            DebugManager.Log($"Running Test: Activate Action Map '{actionMapName}'");

            // Execute o método para ativar o Action Map específico
            _actionManager.ActivateActionMap(actionMapName);

            // Mensagens de depuração
            DebugManager.Log($"Input Active: {_inputActions.asset.enabled}");

            foreach (var actionMap in _inputActions.asset.actionMaps)
            {
                DebugManager.Log($"Action Map '{actionMap.name}' Active: {actionMap.enabled}");
            }
        }
    }
}