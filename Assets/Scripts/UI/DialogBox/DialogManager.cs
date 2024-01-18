using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RiverAttack 
{
    public class DialogManager : MonoBehaviour
    {
        [Header("Dialog Settings")]
        [SerializeField] GameSettings gameSettings;
        [SerializeField] Animator speakerAnimatorController;
        [SerializeField] protected TimeLineManager timelineManager;
        [SerializeField] TMP_Text dialogText;
        [SerializeField] DialogObject dialog;
        [SerializeField] float letterSpeed = 0.05f;
        [SerializeField] GameObject nextCursor;

        [Header("Exit Button Settings")]
        [SerializeField] Animator fadePanelAnimator;
        [SerializeField] Image fillExitButtonImage;
        [SerializeField] private float holdTimer = 0f;
        [SerializeField] float holdDuration = 3f;
        [SerializeField] int sceneToLoad;

        PlayersInputActions m_InputSystem;

        string[] m_Sentences;
        int m_SentenceIndex = 0;
        bool m_IsTyping = false;        
        bool m_IsExitButtonPressed = false;

        Coroutine m_TypingCoroutine;
        Coroutine m_ExitButtonCoroutine;

        const string PT_BR_LOCALIZATION = "Portuguese (Brazil) (pt-BR)";
        const string EN_LOCALIZATION = "English (en)";

        void OnEnable()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.BriefingRoom.Enable();

            m_InputSystem.BriefingRoom.Next.performed += NextButton;
            m_InputSystem.BriefingRoom.Exit.started += ctx => StartHoldTime();
            m_InputSystem.BriefingRoom.Exit.canceled += ctx => StopHoldTime();
        }

        void OnDisable()
        {
            m_InputSystem.BriefingRoom.Disable();
        }


        void Start()
        {
            m_Sentences = GetLocalization();
            m_TypingCoroutine = StartCoroutine(TypeSentence(m_Sentences[m_SentenceIndex]));
        }
        

        string[] GetLocalization()
        {
            var language = gameSettings.startLocale;

            return language.LocaleName switch
            {
                PT_BR_LOCALIZATION => dialog.dialogSentences_PT_BR,
                EN_LOCALIZATION => dialog.dialogSentences_EN,
                _ => dialog.dialogSentences_EN
            };

        }

        void NextButton(InputAction.CallbackContext context)
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

        void IsTyping(bool isSpeaking)
        {
            m_IsTyping = isSpeaking;
            nextCursor.SetActive(!isSpeaking);
            if (speakerAnimatorController != null)
                speakerAnimatorController.SetBool("Speak", isSpeaking);
        }

        IEnumerator TypeSentence(string sentence)
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

        void NextSentence()
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

        void NextSlideAnim(int sentenceIndex)
        {
            if (dialog.dialogAnimationStartTime[sentenceIndex] >= 0)
            {
                timelineManager.PlayAnimation(dialog.dialogAnimationStartTime[sentenceIndex]);
            }
        } 

        void DialogEnd()
        {
            // Fim do diálogo, você pode adicionar lógica adicional aqui
            //Debug.Log("Fim do diálogo");
            GameSteamManager.UnlockAchievement("ACH_FINISH_TUTORIAL");
            fadePanelAnimator.SetTrigger("FadeOut");
            StopAllCoroutines();
            SceneManager.LoadSceneAsync(sceneToLoad);
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
            //Debug.Log("Botão de saída mantido pressionado por 3 segundos!");
            DialogEnd();
        }
    }
}

