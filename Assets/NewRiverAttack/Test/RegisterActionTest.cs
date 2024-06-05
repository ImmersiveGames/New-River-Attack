using System;
using ImmersiveGames;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewRiverAttack.Test
{
    public class RegisterActionTest : MonoBehaviour
    {
        private PlayersInputActions _inputActions;
        private ActionManager _actionManager;

        private void Start()
        {
            // Inicializa o PlayersInputActions
            _inputActions = InputGameManager.InputActions;
            _inputActions.Enable();

            // Inicializa o ActionManager com a instância de PlayersInputActions
            _actionManager = InputGameManager.ActionManager;

            // Teste: Registrar uma ação
            TestRegisterAction("EscNotification", OnJumpAction);
            TestRegisterAction("BackButton", OnTest);
        }

        private void OnDestroy()
        {
            // Certifique-se de desativar o PlayersInputActions ao destruir o objeto
            _inputActions?.Disable();

            // Certifique-se de desativar o ActionManager ao destruir o objeto
            _actionManager = null;
        }

        private void TestRegisterAction(string actionName, Action<InputAction.CallbackContext> callback)
        {
            DebugManager.Log<RegisterActionTest>($"Running Test: Register Action '{actionName}'");

            // Ativa o Action Map relevante antes de registrar a ação (ajuste conforme necessário)
            _actionManager.ActivateActionMap(ActionManager.GameActionMaps.UiControls);

            // Registra a ação com a função de callback
            _actionManager.RegisterAction(actionName, callback);

            // Mensagem de depuração indicando que a ação foi registrada
            DebugManager.Log<RegisterActionTest>($"Action '{actionName}' successfully registered");

            // Simula a execução da ação (pode ser ajustado conforme necessário)
            SimulateAction(actionName);
        }

        private void OnJumpAction(InputAction.CallbackContext context)
        {
            DebugManager.Log<RegisterActionTest>("Jump action performed!");
        }
        private void OnTest(InputAction.CallbackContext context)
        {
            DebugManager.Log<RegisterActionTest>("Botão B ativado!");
        }

        private void SimulateAction(string actionName)
        {
            // Simula a execução da ação para testar o registro
            DebugManager.Log<RegisterActionTest>($"Simulating action: {actionName}");

            // Chama os observadores registrados para a ação (o ActionManager deve chamar a função de callback)
            _actionManager.NotifyObservers(actionName, new InputAction.CallbackContext());
        }
    }
}
