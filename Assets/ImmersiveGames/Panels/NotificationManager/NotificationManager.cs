using System.Collections.Generic;
using UnityEngine;

namespace ImmersiveGames.Panels.NotificationManager
{
    public class NotificationManager : MonoBehaviour
    {
        [SerializeField] private GameObject notificationPrefab;
        private readonly Queue<string> _notificationQueue = new Queue<string>();
        private NotificationPanel _currentPanel;

        private void Start()
        {
            // Exemplo de chamada de notificação para teste
            AddNotification("Bem-vindo ao jogo!");
        }

        private void Update()
        {
            // Exemplo: pressionar a tecla de espaço para adicionar notificações
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddNotification("Nova mensagem!");
            }
        }

        public void AddNotification(string message)
        {
            _notificationQueue.Enqueue(message);
            if (_notificationQueue.Count == 1 && !HasActivePanel())
            {
                ShowNextNotification();
            }
        }

        public void ShowNextNotification()
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
                Debug.LogError("NotificationPanel component not found on instantiated object.");
                // Lide com o erro de forma apropriada
            }
        }

        internal void OnNotificationClosed()
        {
            _notificationQueue.Dequeue();
            _currentPanel = null;

            if (_notificationQueue.Count > 0)
            {
                ShowNextNotification();
            }
        }

        public bool HasActivePanel()
        {
            return _currentPanel != null;
        }
    }
}
