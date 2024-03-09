using UnityEngine;

namespace ImmersiveGames.MenuManagers.NotificationManager.Tests
{
    public class TestNotificationManager: MonoBehaviour
    {
        private NotificationManager _notificationManager;
        private void Start()
        {
            _notificationManager = GetComponent<NotificationManager>();
            // Exemplo de chamada de notificação para teste
            _notificationManager.AddNotification("Bem-vindo ao jogo!");
        }
        private void Update()
        {
            // Exemplo: pressionar a tecla de espaço para adicionar notificações
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _notificationManager.AddNotification("Nova mensagem!");
            }
        }
    }
}