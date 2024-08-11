using System;
using System.Collections.Generic;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ImmersiveGames.MenuManagers.NotificationManager
{
    public class NotificationManager : Singleton<NotificationManager>
    {
        [Header("Configurações")]
        public GameObject notificationPanelPrefab;
        public GameObject choicePanelPrefab;
        public float notificationDisplayTime = 5f; // Tempo que cada notificação ficará visível

        private readonly Queue<NotificationData> _notificationQueue = new Queue<NotificationData>();
        private NotificationPanel _currentPanel;
        private GenericObjectPool<NotificationPanel> _panelPool;
        private GameObject _initiallySelectedObject;

        // Eventos para notificar outros scripts sobre notificações
        public event EventHandler<NotificationEventArgs> EventNotificationAdded;
        public event EventHandler<NotificationEventArgs> EventNotificationAccepted;
        public event EventHandler<NotificationEventArgs> EventNotificationClosed;

        private void Start()
        {
            _panelPool = new GenericObjectPool<NotificationPanel>(notificationPanelPrefab.GetComponent<NotificationPanel>(), transform, 5);
        }

        public void AddNotification(NotificationData notificationData)
        {
            _notificationQueue.Enqueue(notificationData);
            EventNotificationAdded?.Invoke(this, new NotificationEventArgs(notificationData));
            TryShowNextNotification();
        }

        private void TryShowNextNotification()
        {
            if (CanShowNotification())
            {
                if (_notificationQueue.Count == 1)
                {
                    SaveInitialFocus();
                }

                var nextNotification = _notificationQueue.Dequeue();
                InstantiateNotificationPanel(nextNotification);
                _currentPanel.Show(nextNotification.message, () => CloseNotification(nextNotification), null);

                // Fechar automaticamente após o tempo especificado
                StartCoroutine(AutoCloseNotification(_currentPanel, notificationDisplayTime));
            }
        }

        private bool CanShowNotification()
        {
            return _currentPanel == null && _notificationQueue.Count > 0;
        }

        private void InstantiateNotificationPanel(NotificationData nextNotification)
        {
            if (nextNotification.panelPrefab == null)
            {
                nextNotification.panelPrefab = notificationPanelPrefab;
            }
            _currentPanel = _panelPool.GetObject();
            _currentPanel.transform.SetParent(transform, false);
            //_currentPanel.Initialize(nextNotification);
        }

        private void CloseNotification(NotificationData notification)
        {
            EventNotificationClosed?.Invoke(this, new NotificationEventArgs(notification));
            _panelPool.ReleaseObject(_currentPanel);
            _currentPanel = null;

            if (_notificationQueue.Count == 0)
            {
                RestoreInitialFocus();
            }

            TryShowNextNotification();
        }

        public void OnPanelClosed(NotificationPanel panel)
        {
            if (panel == _currentPanel)
            {
                CloseNotification(panel.NotificationData);
            }
        }

        private System.Collections.IEnumerator AutoCloseNotification(NotificationPanel panel, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (panel == _currentPanel)
            {
                panel.ClosePanel(); // Fecha o painel automaticamente após o tempo definido
            }
        }

        private void SaveInitialFocus()
        {
            _initiallySelectedObject = EventSystem.current.currentSelectedGameObject;
        }

        private void RestoreInitialFocus()
        {
            if (_initiallySelectedObject != null)
            {
                EventSystem.current.SetSelectedGameObject(_initiallySelectedObject);
            }
        }
    }

    public class NotificationEventArgs : EventArgs
    {
        public NotificationData NotificationData { get; }

        public NotificationEventArgs(NotificationData notificationData)
        {
            NotificationData = notificationData;
        }
    }
}
