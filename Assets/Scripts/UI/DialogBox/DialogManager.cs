using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;
using System.Collections.Generic;

namespace RiverAttack 
{
    public class DialogManager : MonoBehaviour
    {
        [Header("Dialog Settings")]
        [SerializeField]
        private Animator speakerAnimatorController;
        [SerializeField] protected TimeLineManager timelineManager;
        [SerializeField] private TMP_Text dialogText;
        [SerializeField] private float letterSpeed = 0.05f;
        [SerializeField] private GameObject nextCursor;
        [SerializeField] private List<LocalizedString> myDialogs;
        [SerializeField] public float[] dialogAnimationStartTime;

        [Header("Exit Button Settings")]
        [SerializeField]
        private Image fillExitButtonImage;
        [SerializeField] private float holdTimer;
        [SerializeField] private float holdDuration = 3f;

        private PlayersInputActions m_InputSystem;

        private string[] m_Sentences;
        private int m_SentenceIndex;
        private bool m_IsTyping;
        private bool m_IsExitButtonPressed;

        private Coroutine m_TypingCoroutine;
        private Coroutine m_ExitButtonCoroutine;
        private static readonly int Speak = Animator.StringToHash("Speak");

        private void OnEnable()
        {
            m_InputSystem = GameManager.instance.inputSystem;
            m_InputSystem.BriefingRoom.Enable();

            m_InputSystem.BriefingRoom.Next.performed += NextButton;
            m_InputSystem.BriefingRoom.Exit.started += ctx => StartHoldTime();
            m_InputSystem.BriefingRoom.Exit.canceled += ctx => StopHoldTime();
        }

        private void OnDisable()
        {
            m_InputSystem.BriefingRoom.Disable();
        }


        private void Start()
        {
            m_Sentences = GetLocalization();
            m_TypingCoroutine = StartCoroutine(TypeSentence(m_Sentences[m_SentenceIndex]));
        }


        private string[] GetLocalization()
        {
            return myDialogs.ConvertAll(ls => ls.GetLocalizedString()).ToArray();
        }

        private void NextButton(InputAction.CallbackContext context)
        {
            if (m_IsTyping)
            {
                // Se ainda estiver digitando, exiba o texto inteiro imediatamente
                StopCoroutine(m_TypingCoroutine);
                dialogText.text = m_Sentences[m_SentenceIndex];
                IsTyping(false);
            }
            else
            {
                // Se a frase terminou, avance para a próxima ou encerre o di�logo
                NextSentence();
            }
        }

        private void IsTyping(bool isSpeaking)
        {
            m_IsTyping = isSpeaking;
            nextCursor.SetActive(!isSpeaking);
            if (speakerAnimatorController != null)
                speakerAnimatorController.SetBool(Speak, isSpeaking);
        }

        private IEnumerator TypeSentence(string sentence)
        {
            dialogText.text = "";
                        
            IsTyping(true);

            foreach (char letter in sentence.ToCharArray())
            {
                dialogText.text += letter;
                yield return new WaitForSeconds(letterSpeed);
            }
            
            IsTyping(false);
        }

        private void NextSentence()
        {
            if (m_SentenceIndex < m_Sentences.Length - 1)
            {
                m_SentenceIndex++;
                NextSlideAnim(m_SentenceIndex);
                m_TypingCoroutine = StartCoroutine(TypeSentence(m_Sentences[m_SentenceIndex]));
            }
            else
            {  
                DialogEnd();
            }
        }

        private void NextSlideAnim(int sentenceIndex)
        {
            if (dialogAnimationStartTime[sentenceIndex] >= 0)
            {
                timelineManager.PlayAnimation(dialogAnimationStartTime[sentenceIndex]);
            }
        }

        private void DialogEnd()
        {
            // Fim do diálogo, você pode adicionar lógica adicional aqui
            //Debug.Log("Fim do diálogo");
            GameSteamManager.UnlockAchievement("ACH_FINISH_TUTORIAL");
            StopAllCoroutines();
            GameManager.instance.ChangeState(new GameStateMenu(), GameManager.GameScenes.MainScene.ToString());
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
            //Debug.Log("Botão de saída mantido pressionado por 3 segundos!");
            DialogEnd();
        }
    }
}

