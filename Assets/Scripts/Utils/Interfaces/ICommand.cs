using UnityEngine.InputSystem;
namespace Utils
{
    public interface ICommand
    {
        void Execute(InputAction.CallbackContext callbackContext);
        void UnExecute(InputAction.CallbackContext callbackContext);
    }
}
