using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


namespace RiverAttack 
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] GameSettings gameSettings;
        [SerializeField] Animator speakerAnimatorController;
        [SerializeField] TMP_Text dialogText;
        [SerializeField] DialogObject dialog;
        [SerializeField] float letterSpeed = 0.05f;
        [SerializeField] GameObject nextCursor;

        PlayersInputActions m_InputSystem;

        private string[] sentences;
        private int sentenceIndex = 0;
        private bool isTyping = false;        

        const string PT_BR_LOCALIZATION = "Portuguese (Brazil) (pt-BR)";
        const string EN_LOCALIZATION = "English (en)";

        void OnEnable()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.BriefingRoom.Enable();

            m_InputSystem.BriefingRoom.Next.performed += NextButton;
            m_InputSystem.BriefingRoom.Exit.performed += ExitButton;
        }

        void OnDisable()
        {
            m_InputSystem.BriefingRoom.Disable();
        }


        void Start()
        {
            sentences = GetLocalization();
            StartCoroutine(TypeSentence(sentences[sentenceIndex]));
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

        void Update()
        {
            //NextButton();
        }

        private void NextButton(InputAction.CallbackContext context)
        {
            if (isTyping)
            {
                // Se ainda estiver digitando, exiba o texto inteiro imediatamente
                StopAllCoroutines();
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
                StartCoroutine(TypeSentence(sentences[sentenceIndex]));
            }
            else
            {
                // Fim do diálogo, você pode adicionar lógica adicional aqui
                DialogEnd();
            }
        }

        void ExitButton(InputAction.CallbackContext context)
        {
            DialogEnd();
        }

        void DialogEnd()
        {
            Debug.Log("Fim do diálogo");
        }
    }
}

