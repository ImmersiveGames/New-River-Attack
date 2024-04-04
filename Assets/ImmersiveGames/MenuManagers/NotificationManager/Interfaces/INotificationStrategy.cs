using UnityEngine.InputSystem;

namespace ImmersiveGames.MenuManagers.NotificationManager.Interfaces
{
    public interface INotificationStrategy
    {
        void Show(NotificationPanel notificationPanel, string message, System.Action onClose, System.Action onConfirm = null);
        void OnInputCloseMessage(NotificationPanel notificationPanel, InputAction.CallbackContext context);
        void OnInputConfirmMessage(NotificationPanel notificationPanel, InputAction.CallbackContext context);
    }
    namespace ImmersiveGames.MenuManagers.NotificationManager.Interfaces
    {
        public enum NotificationType
        {
            Text,Choice
        }
    }
}