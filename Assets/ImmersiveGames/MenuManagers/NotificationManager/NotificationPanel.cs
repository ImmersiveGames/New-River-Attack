using ImmersiveGames.InputManager;
using RiverAttack;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.NotificationManager
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
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponentInParent<AudioSource>();
        }

        private void Start()
        {
            _inputActions = InputManagerInitializer.InputActions;
            _inputActions.Enable();

            _actionManager = InputManagerInitializer.ActionManager;

            _actionManager.ActivateActionMap(GameActionMaps.UiControls);

            ActionManager.RegisterAction("EscNotification", OnInputCloseMessage);
        }

        private void OnDestroy()
        {
            _actionManager.UnregisterAction("EscNotification", OnInputCloseMessage);
        }

        public void ShowMessage(string message, System.Action onClose, bool useClosingAnimation = true)
        {
            messageText.text = message;
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                onClose?.Invoke();
                if (useClosingAnimation && _animator != null)
                {
                    _animator.SetTrigger(animationClose);
                }
                ClosePanel();
            });
            if (_animator != null)
            {
                _animator.SetTrigger(animationOpen);
            }
            notificationSound.PlayOnShot(_audioSource);
        }

        private void OnInputCloseMessage(InputAction.CallbackContext context)
        {
            closeButton.onClick.Invoke();
        }

        private void ClosePanel()
        {
            if (_animator != null && !string.IsNullOrEmpty(animationClose))
            {
                // Se houver animação, aciona a animação de fechamento
                _animator.SetTrigger(animationClose);

                // Obtém a duração da animação
                var animationDuration = Utils.Tools.GetAnimationDuration(_animator, animationClose);

                // Agendando a destruição após a duração da animação
                StartCoroutine(DelayedDestroyCoroutine(animationDuration));
            }
            else
            {
                // Se não houver animação, destrua imediatamente o objeto
                Destroy(gameObject);
            }
        }
        // Coroutine para destruir o objeto após um atraso
        private System.Collections.IEnumerator DelayedDestroyCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            // Destrói o objeto após o atraso
            Destroy(gameObject);
        }
    }
}
