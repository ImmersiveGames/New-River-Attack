using ImmersiveGames.InputManager;
using RiverAttack;
using UnityEngine;

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

        public static ActionManager ActionManager
        {
            get
            {
                if (_actionManager != null) return _actionManager;
                _actionManager = new ActionManager(InputActions);
                return _actionManager;
            }
        }

        private void OnDisable()
        {
            // Desativa os Inputs ao desabilitar o objeto
            _inputActions?.Disable();
        }
    }
}