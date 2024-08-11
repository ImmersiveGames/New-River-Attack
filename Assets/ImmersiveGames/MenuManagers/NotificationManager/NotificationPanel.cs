using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.NotificationManager
{
    public class NotificationPanel : MonoBehaviour
    {
        public TMP_Text messageText;
        public Button confirmButton;

        public float openTimeDuration;
        public float closeTimeDuration;

        private Animator _animator;
        private AudioSource _audioSource;

        private static readonly int OpenTrigger = Animator.StringToHash("OpenTrigger");
        private static readonly int CloseTrigger = Animator.StringToHash("CloseTrigger");

        public NotificationData NotificationData { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            // Se houver botões de confirmação, registre as ações, se necessário
            if (confirmButton != null)
            {
                InputGameManager.RegisterAction("ConfirmNotification", ButtonConfirm);
            }
        }

        private void OnDestroy()
        {
            if (confirmButton != null)
            {
                InputGameManager.UnregisterAction("ConfirmNotification", ButtonConfirm);
            }

            InputGameManager.ActionManager.RestoreActionMap();
        }

        public void Show(string message, Action onClose, Action onConfirm = null)
        {
            PlayNotificationSound();
            SetMessageText(message);
            ConfigureButtons(onClose, onConfirm);
            OpenPanel();
        }

        private void PlayNotificationSound()
        {
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
        }

        private void SetMessageText(string message)
        {
            messageText.text = message;
        }

        private void ConfigureButtons(Action onClose, Action onConfirm)
        {
            // Se houver botões de confirmação, configure-os
            if (confirmButton != null)
            {
                confirmButton.gameObject.SetActive(onConfirm != null);
                if (onConfirm != null)
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
        }

        private void OpenPanel()
        {
            if (_animator != null)
            {
                _animator.SetTrigger(OpenTrigger);
            }
            else
            {
                StartCoroutine(PanelsHelper.TogglePanel(gameObject, openTimeDuration, closeTimeDuration, true));
            }
        }

        public void ClosePanel()
        {
            if (_animator != null)
            {
                _animator.SetTrigger(CloseTrigger);
                var animationDuration = global::Utils.Tools.GetAnimationDuration(_animator, CloseTrigger.ToString());
                StartCoroutine(DelayedClose(animationDuration));
            }
            else
            {
                StartCoroutine(DelayedClose(closeTimeDuration));
            }
        }

        private System.Collections.IEnumerator DelayedClose(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
            NotificationManager.instance.OnPanelClosed(this); // Notifica o manager que o painel foi fechado
        }

        private void ButtonConfirm(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            confirmButton?.onClick?.Invoke();
        }
    }
}
