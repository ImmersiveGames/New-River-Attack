using UnityEngine.InputSystem;
namespace Utils
{
    public interface ICommand
    {
        void Execute();
        void Execute(InputAction.CallbackContext callbackContext);
        void UnExecute();
        void UnExecute(InputAction.CallbackContext callbackContext);
    }
}
