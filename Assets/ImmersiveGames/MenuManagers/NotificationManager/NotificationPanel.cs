using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.NotificationManager
{
    public class NotificationPanel : MonoBehaviour
    {
        public TMP_Text messageText;
        public Button closeButton;
        public Button confirmButton;
        
        public float openTimeDuration;
        public float closeTimeDuration;

        private Animator _animator;
        private AudioSource _audioSource;
        protected NotificationData NotificationData;

        private static readonly int OpenTrigger = Animator.StringToHash("OpenTrigger");
        private static readonly int CloseTrigger = Animator.StringToHash("CloseTrigger");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }
        private void Start()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Notifications);
            InputGameManager.RegisterAction("CloseNotification",ButtonClose );
            if(confirmButton != null)
                InputGameManager.RegisterAction("ConfirmNotification",ButtonConfirm );
        }
        private void OnDisable()
        {
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            InputGameManager.UnregisterAction("CloseNotification",ButtonClose );
            if(confirmButton != null)
                InputGameManager.UnregisterAction("ConfirmNotification",ButtonConfirm );
            
            InputGameManager.ActionManager.RestoreActionMap();
            _animator = null;
            _audioSource = null;
        }

        public void Show(string message, Action onClose, Action onConfirm = null)
        {
            // Etapa 1: Reproduzir som de notificação
            if (_audioSource != null)
            {
                _audioSource.Play();
            }

            // Etapa 2: Definir texto da mensagem
            messageText.text = message;

            // Etapa 3: Verificar se o botão de fechar está presente
            if (closeButton == null)
            {
                DebugManager.LogError<NotificationPanel>("CloseButton is missing!");
                return; // Abort the Show method if closeButton is missing
            }

            // Etapa 4: Mostrar/ocultar o botão de confirmação
            if (confirmButton != null)
            {
                confirmButton.gameObject.SetActive(onConfirm != null);
            }

            // Etapa 5: Abrir o painel
            if (_animator != null)
            {
                _animator.SetTrigger(OpenTrigger);
            }
            else
            {
                StartCoroutine(PanelsHelper.TogglePanel(gameObject, openTimeDuration, closeTimeDuration, true));
                
            }

            // Etapa 6: Armazenar dados da notificação
            NotificationData = new NotificationData
            {
                message = message,
                ConfirmAction = onConfirm,
            };

            // Etapa 7: Adicionar ouvintes aos botões
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                onClose?.Invoke();
                ClosePanel();
            });

            if (onConfirm != null && confirmButton != null)
            {
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(() =>
                {
                    onConfirm.Invoke();
                    onClose.Invoke();
                    ClosePanel();
                });
            }
        }

        private void ClosePanel()
        {
            // Etapa 1: Fechar o painel
            if (_animator != null)
            {
                _animator.SetTrigger(CloseTrigger);
                var animationDuration = global::Utils.Tools.GetAnimationDuration(_animator, CloseTrigger.ToString());
                StartCoroutine(DelayedDestroyCoroutine(animationDuration));
            }
            else
            {
                StartCoroutine(PanelsHelper.TogglePanel(gameObject, openTimeDuration, closeTimeDuration, false));
            }
        }
        private void ButtonClose(InputAction.CallbackContext context)
        {
            DebugManager.Log<NotificationPanel>("Close Button action performed!");
            closeButton.onClick?.Invoke();
        }
        
        private void ButtonConfirm(InputAction.CallbackContext context)
        {
            DebugManager.Log<NotificationPanel>("Confirm Button action performed!");
            confirmButton.onClick?.Invoke();
        }

        private System.Collections.IEnumerator DelayedDestroyCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
    }
}
