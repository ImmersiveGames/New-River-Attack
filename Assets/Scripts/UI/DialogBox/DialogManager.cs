using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
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

        private string[] sentences;
        private int sentenceIndex = 0;
        private bool isTyping = false;        
        private bool isExitButtonPressed = false;

        private Coroutine typingCoroutine;
        private Coroutine exitButtonCoroutine;

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
            sentences = GetLocalization();
            typingCoroutine = StartCoroutine(TypeSentence(sentences[sentenceIndex]));
        }

        void Update()
        {
            //NextButton();
        }

        private string[] GetLocalization()
        {
            var language = gameSettings.startLocale;

            if (language.LocaleName == PT_BR_LOCALIZATION)
            {
                return dialog.dialogSentences_PT_BR;
            }                

            if (language.LocaleName == EN_LOCALIZATION)
            {
                return dialog.dialogSentences_EN;
            }                

            else return dialog.dialogSentences_EN;
        }

        private void NextButton(InputAction.CallbackContext context)
        {
            if (isTyping)
            {
                // Se ainda estiver digitando, exiba o texto inteiro imediatamente
                StopCoroutine(typingCoroutine);
                dialogText.text = sentences[sentenceIndex];
                IsTyping(false);
            }
            else
            {
                // Se a frase terminou, avance para a próxima ou encerre o diálogo
                NextSentence();
            }
        }

        private void IsTyping(bool isSpeaking)
        {
            isTyping = isSpeaking;
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
            if (sentenceIndex < sentences.Length - 1)
            {
                sentenceIndex++;
                NextSlideAnim(sentenceIndex);
                typingCoroutine = StartCoroutine(TypeSentence(sentences[sentenceIndex]));
            }
            else
            {  
                DialogEnd();
            }
        }

        private void NextSlideAnim(int sentenceIndex)
        {
            if (dialog.dialogAnimationStartTime[sentenceIndex] >= 0)
            {
                timelineManager.PlayAnimation(dialog.dialogAnimationStartTime[sentenceIndex]);
            }
        } 

        void DialogEnd()
        {
            // Fim do diálogo, você pode adicionar lógica adicional aqui
            Debug.Log("Fim do diálogo");
            fadePanelAnimator.SetTrigger("FadeOut");
            StopAllCoroutines();
            SceneManager.LoadSceneAsync(sceneToLoad);
        }

        private void StartHoldTime()
        {
            isExitButtonPressed = true;

            exitButtonCoroutine = StartCoroutine(FillImageOverTime());
        }

        private void StopHoldTime()
        {
            isExitButtonPressed = false;
            holdTimer = 0f;
            StopCoroutine(exitButtonCoroutine); // Parar qualquer preenchimento ainda em andamento
            fillExitButtonImage.fillAmount = 0f; // Reiniciar o preenchimento quando o botão é liberado        
        }

        IEnumerator FillImageOverTime()
        {
            while (isExitButtonPressed && holdTimer < holdDuration)
            {
                holdTimer += Time.deltaTime;

                float fillAmount = Mathf.Clamp01(holdTimer / holdDuration);
                fillExitButtonImage.fillAmount = fillAmount;

                yield return null;
            }

            // Ação a ser executada após manter o botão pressionado por 3 segundos
            Debug.Log("Botão de saída mantido pressionado por 3 segundos!");
            DialogEnd();
        }
    }
}

