using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.NotificationManager
{
    public class NotificationManager : MonoBehaviour
    {
        [SerializeField] private GameObject notificationPrefab;
        private readonly Queue<string> _notificationQueue = new Queue<string>();
        private NotificationPanel _currentPanel;

        public void AddNotification(string message)
        {
            _notificationQueue.Enqueue(message);
            if (_notificationQueue.Count == 1 && !HasActivePanel())
            {
                ShowNextNotification();
            }
        }

        private void ShowNextNotification()
        {
            if (_notificationQueue.Count <= 0) return;
            var message = _notificationQueue.Peek();
            var notificationObject = Instantiate(notificationPrefab, transform);
            var notificationPanel = notificationObject.GetComponent<NotificationPanel>();

            if (notificationPanel != null)
            {
                _currentPanel = notificationPanel;
                notificationPanel.ShowMessage(message, OnNotificationClosed);
            }
            else
            {
                DebugManager.LogError("NotificationPanel component not found on instantiated object.");
                // Lide com o erro de forma apropriada
            }
        }

        private void OnNotificationClosed()
        {
            _notificationQueue.Dequeue();
            _currentPanel = null;

            if (_notificationQueue.Count > 0)
            {
                ShowNextNotification();
            }
        }
        
        private bool HasActivePanel()
        {
            return _currentPanel != null;
        }
    }
}
