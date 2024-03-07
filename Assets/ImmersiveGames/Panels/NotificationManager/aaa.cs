/*namespace ImmersiveGames.Panels.NotificationManager
{
    using System;
using ImmersiveGames.InputManager;
using RiverAttack;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ImmersiveGames.Panels.NotificationManager
{
    public class NotificationPanel : MonoBehaviour
    {
        public AudioEvent notificationSound;
        
        public TMP_Text messageText;
        public Button closeButton;
        public string animationOpen = "OpenTrigger";
        public string animationClose = "CloseTrigger";
        
        private Animator _animator;
        private AudioSource _audioSource;
        private PlayersInputActions _inputActions;
        private ActionManager _actionManager;
        private NotificationManager _notificationManager;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponentInParent<AudioSource>();
            _notificationManager = GetComponentInParent<NotificationManager>();
        }

        private void Start()
        {
            // Inicializa o PlayersInputActions
            _inputActions = InputManagerInitializer.InputActions;
            _inputActions.Enable();

            // Inicializa o ActionManager com a instância de PlayersInputActions
            _actionManager = InputManagerInitializer.ActionManager;

            _actionManager.ActivateActionMap(GameActionMaps.UiControls);

            // Registra a ação com a função de callback
            _actionManager.RegisterAction("EscNotification", OnInputCloseMessage);
        }

        private void OnDestroy()
        {
            // Certifique-se de desativar o ActionManager ao destruir o objeto
            _actionManager = null;
        }

        public void ShowMessage(string message, System.Action onClose)
        {
            messageText.text = message;
            closeButton.onClick.RemoveAllListeners(); // Limpar listeners antigos
            closeButton.onClick.AddListener(() =>
            {
                onClose?.Invoke();
                if (_animator != null)
                {
                    _animator.SetTrigger(animationClose); // Substitua "CloseTrigger" pelo nome do trigger da sua animação de fechamento
                }
                ClosePanel();
            });
            if (_animator != null)
            {
                _animator.SetTrigger(animationOpen); // Substitua "OpenTrigger" pelo nome do trigger da sua animação de abertura
            }
            notificationSound.PlayOnShot(_audioSource);
        }
        private void OnInputCloseMessage(InputAction.CallbackContext context)
        {
            _notificationManager.OnNotificationClosed();
            if (_animator != null)
            {
                _animator.SetTrigger(animationClose); // Substitua "CloseTrigger" pelo nome do trigger da sua animação de fechamento
            }
            ClosePanel();
            Debug.Log("Close Notification performed!");
        }
        

        // Método chamado pela animação de fechamento
        private void ClosePanel()
        {
            Destroy(gameObject);
        }
    }
}
}*/