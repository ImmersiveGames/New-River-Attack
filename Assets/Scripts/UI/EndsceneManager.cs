using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace RiverAttack 
{    public class EndsceneManager : MonoBehaviour
    {
        [Header("Exit Button Settings")]
        [SerializeField]
        private Image fillExitButtonImage;
        [SerializeField] private float holdTimer = 0f;
        [SerializeField] private float holdDuration = 3f;
        [SerializeField] private int sceneToLoad;

        private PlayersInputActions m_InputSystem;
        private Coroutine m_ExitButtonCoroutine;

        private bool m_IsExitButtonPressed = false;

        private void OnEnable()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.BriefingRoom.Enable();

            m_InputSystem.BriefingRoom.Exit.started += ctx => StartHoldTime();
            m_InputSystem.BriefingRoom.Exit.canceled += ctx => StopHoldTime();
        }

        private void OnDisable()
        {
            m_InputSystem.BriefingRoom.Disable();
        }

        private void StartHoldTime()
        {
            m_IsExitButtonPressed = true;

            m_ExitButtonCoroutine = StartCoroutine(FillImageOverTime());
        }

        private void StopHoldTime()
        {
            m_IsExitButtonPressed = false;
            holdTimer = 0f;
            StopCoroutine(m_ExitButtonCoroutine); // Parar qualquer preenchimento ainda em andamento
            fillExitButtonImage.fillAmount = 0f; // Reiniciar o preenchimento quando o bot�o � liberado        
        }

        private IEnumerator FillImageOverTime()
        {
            while (m_IsExitButtonPressed && holdTimer < holdDuration)
            {
                holdTimer += Time.deltaTime;

                float fillAmount = Mathf.Clamp01(holdTimer / holdDuration);
                fillExitButtonImage.fillAmount = fillAmount;

                yield return null;
            }

            // Ação a ser executada após manter o botão pressionado por 3 segundos
            BackToMainScene();
        }

        private void BackToMainScene()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

