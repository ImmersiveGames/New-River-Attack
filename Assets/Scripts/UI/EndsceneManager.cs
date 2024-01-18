using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace RiverAttack 
{    public class EndsceneManager : MonoBehaviour
    {
        [Header("Exit Button Settings")]
        [SerializeField] Image fillExitButtonImage;
        [SerializeField] private float holdTimer = 0f;
        [SerializeField] float holdDuration = 3f;
        [SerializeField] int sceneToLoad;

        PlayersInputActions m_InputSystem;
        Coroutine m_ExitButtonCoroutine;

        bool m_IsExitButtonPressed = false;

        void OnEnable()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.BriefingRoom.Enable();

            m_InputSystem.BriefingRoom.Exit.started += ctx => StartHoldTime();
            m_InputSystem.BriefingRoom.Exit.canceled += ctx => StopHoldTime();
        }

        void OnDisable()
        {
            m_InputSystem.BriefingRoom.Disable();
        }

        void StartHoldTime()
        {
            m_IsExitButtonPressed = true;

            m_ExitButtonCoroutine = StartCoroutine(FillImageOverTime());
        }

        void StopHoldTime()
        {
            m_IsExitButtonPressed = false;
            holdTimer = 0f;
            StopCoroutine(m_ExitButtonCoroutine); // Parar qualquer preenchimento ainda em andamento
            fillExitButtonImage.fillAmount = 0f; // Reiniciar o preenchimento quando o bot�o � liberado        
        }

        IEnumerator FillImageOverTime()
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

        void BackToMainScene()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

