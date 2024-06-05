using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.MenuManagers.NotificationManager.Tests
{
    public class TestNotificationManager : MonoBehaviour
    {
        public GameObject notificationPanelPrefab;
        public GameObject choicePanelPrefab;

        private void Start()
        {
            // Inscrever-se nos eventos do NotificationManager
            NotificationManager.instance.EventNotificationAdded += HandleNotificationAdded;
            NotificationManager.instance.EventNotificationAccepted += HandleNotificationAccepted;
            NotificationManager.instance.EventNotificationClosed += HandleNotificationClosed;
        }

        private void OnDestroy()
        {
            // Desinscrever-se dos eventos do NotificationManager para evitar vazamentos de memória
            NotificationManager.instance.EventNotificationAdded -= HandleNotificationAdded;
            NotificationManager.instance.EventNotificationAccepted -= HandleNotificationAccepted;
            NotificationManager.instance.EventNotificationClosed -= HandleNotificationClosed;
        }

        private void HandleNotificationAdded(NotificationData notificationData)
        {
            Debug.Log("Notification added: " + notificationData.message);
        }

        private void HandleNotificationAccepted(NotificationData notificationData)
        {
            Debug.Log("Notification accepted: " + notificationData.message);
        }

        private void HandleNotificationClosed(NotificationData notificationData)
        {
            Debug.Log("Notification closed: " + notificationData.message);
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                AddTextNotification($"This is a simple text notification.{Time.time}");
            }

            if (Keyboard.current.altKey.wasPressedThisFrame)
            {
                AddConfirmationNotification($"Do you want to confirm this action? {Time.time}", ConfirmAction);
            }
        }

        private void AddTextNotification(string message)
        {
            var notificationData = new NotificationData
            {
                panelPrefab = notificationPanelPrefab,
                message = message,
            };

            NotificationManager.instance.AddNotification(notificationData);
        }

        private void AddConfirmationNotification(string message, System.Action confirmAction)
        {
            var notificationData = new NotificationData
            {
                panelPrefab = choicePanelPrefab,
                message = message,
                ConfirmAction = confirmAction,
            };

            NotificationManager.instance.AddNotification(notificationData);
        }

        private void ConfirmAction()
        {
            Debug.Log("Action confirmed!");
            // Você pode adicionar outras ações a serem executadas após a confirmação aqui, se necessário.
        }
    }
}
