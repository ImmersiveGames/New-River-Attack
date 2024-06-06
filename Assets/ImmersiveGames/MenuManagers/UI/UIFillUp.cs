using System;
using System.Collections;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.UI
{
    public class UIFillUp: MonoBehaviour
    {
        [SerializeField] private float holdDuration = 3f;
        private Image _image;
        private float _holdTimer;
        private Coroutine _exitButtonCoroutine;
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.BriefingRoom);
            InputGameManager.RegisterAction("Hold_Exit",FillImage, UnFillImage );
        }

        private void OnDisable()
        {
            InputGameManager.UnregisterHoldAction("Hold_Exit",FillImage, UnFillImage );
            if(_exitButtonCoroutine != null)
                StopCoroutine(_exitButtonCoroutine);
        }

        private void FillImage(InputAction.CallbackContext context)
        {
                DebugManager.Log<UIFillUp>($"Enche a barra {context}");
                _exitButtonCoroutine = StartCoroutine(FillImageOverTime());
        }
        private void UnFillImage(InputAction.CallbackContext context)
        {
            _holdTimer = 0f;
            if(_exitButtonCoroutine != null)
                StopCoroutine(_exitButtonCoroutine); // Parar qualquer preenchimento ainda em andamento
            _image.fillAmount = 0f; // Reiniciar o preenchimento quando o bot�o � liberado   
        }
        
        private IEnumerator FillImageOverTime()
        {
            while (_holdTimer < holdDuration)
            {
                _holdTimer += Time.deltaTime;

                var fillAmount = Mathf.Clamp01(_holdTimer / holdDuration);
                _image.fillAmount = fillAmount;

                yield return null;
            }

            // Ação a ser executada após manter o botão pressionado por 3 segundos
            //Debug.Log("Botão de saída mantido pressionado por 3 segundos!");
            StopAllCoroutines();
            if(_exitButtonCoroutine != null)
                StopCoroutine(_exitButtonCoroutine);
            InputGameManager.ActionManager.OnEventOnActionComplete();
        }
    }
}