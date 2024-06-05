using System;
using System.Collections.Generic;
using ImmersiveGames.Utils;
using UnityEngine;

namespace ImmersiveGames.MenuManagers.NotificationManager
{
    public class NotificationManager : Singleton<NotificationManager>
    {
        [Header("Configurações")]
        public GameObject notificationPanelPrefab;
        public GameObject choicePanelPrefab;

        private readonly Queue<NotificationData> _notificationQueue = new Queue<NotificationData>();
        private NotificationPanel _currentPanel;

        // Eventos para indicar outros scripts sobre notificações
        public event Action<NotificationData> EventNotificationAdded;
        public event Action<NotificationData> EventNotificationAccepted;
        public event Action<NotificationData> EventNotificationClosed;
        

        public void AddNotification(NotificationData notificationData)
        {
            _notificationQueue.Enqueue(notificationData);
            EventNotificationAdded?.Invoke(notificationData);
            TryShowNextNotification();
        }

        private void TryShowNextNotification()
        {
            if (_currentPanel != null || _notificationQueue.Count <= 0) return;
            var nextNotification = _notificationQueue.Dequeue();
            _currentPanel = Instantiate(nextNotification.panelPrefab, transform).GetComponent<NotificationPanel>();
            _currentPanel.Show(nextNotification.message, 
                () =>
            {
                EventNotificationClosed?.Invoke(nextNotification);
                CloseNotification();
                TryShowNextNotification();
            }, 
                ()=>
            {
                EventNotificationAccepted?.Invoke(nextNotification);
                nextNotification.ConfirmAction.Invoke();
            });
        }

        private void CloseNotification()
        {
            _currentPanel = null;
            instance.TryShowNextNotification();
        }
        
    }
}
