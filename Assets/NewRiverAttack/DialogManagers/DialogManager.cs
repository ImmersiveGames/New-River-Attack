using System.Collections;
using System.Collections.Generic;
using System.Text;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.TimelineManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GameManagers;
using NewRiverAttack.StateManagers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Playables;

namespace NewRiverAttack.DialogManagers
{
    public class DialogManager : MonoBehaviour
    {
        #region Configurações

        [Header("Painel")]
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private GameObject nextCursor;
        [SerializeField] private float openTimeDuration = 0.2f;
        [SerializeField] private float closeTimeDuration = 0.2f;

        [Header("Textos")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text dialogText;
        [SerializeField] private float letterSpeed = 0.05f;

        [Header("Animações")]
        [SerializeField] private Animator speakerAnimatorController;
        [SerializeField] private PlayableDirector playableDirector;

        [Header("Diálogos")]
        [SerializeField] private List<DialogData> dialogDatas;

        #endregion

        #region Variáveis Privadas

        private bool _dialogOn;
        private const float WaitTime = 0.2f;
        private bool _isTyping;
        private int _currentDialogueIndex;
        private int _currentSentenceIndex;
        private Coroutine _typingCoroutine;
        private TimelineManager _timelineManager;
        private static readonly int Speak = Animator.StringToHash("Speak");

        #endregion

        #region Métodos de Unity

        private void Awake()
        {
            nextCursor.SetActive(false);
        }

        private void OnEnable()
        {
            InputGameManager.ActionManager.EventOnActionComplete += StopDialog;
        }

        private void Start()
        {
            _timelineManager = new TimelineManager(playableDirector);
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.BriefingRoom);
            InputGameManager.RegisterAction("Next", InputNextDialog );
            dialogText.text = "";
            nameText.text = "";
            StartCoroutine(WaitForInitialization());
        }

        private void OnDisable()
        {
            StopCoroutine(_typingCoroutine); 
            InputGameManager.ActionManager.EventOnActionComplete -= StopDialog;
        }

        #endregion

        #region Métodos de Diálogo

        private void StartDialog(int dialogIndex, int sentenceIndex)
        {
            // Verificação de índice do diálogo
            if (dialogIndex >= dialogDatas.Count)
            {
                DebugManager.LogError<DialogManager>("Índice de diálogo inválido: " + dialogIndex);
                return;
            }

            // Verificação de índice da frase
            if (sentenceIndex >= dialogDatas[dialogIndex].sentences.Count)
            {
                DebugManager.LogError<DialogManager>("Índice de frase inválido: " + sentenceIndex);
                return;
            }

            _currentDialogueIndex = dialogIndex;
            _currentSentenceIndex = sentenceIndex;
            _dialogOn = true;
            nameText.text = dialogDatas[_currentDialogueIndex].nameSpeaker.GetLocalizedString();
            StartCoroutine(CloseAndOpenPanel());
        }
        private void NextDialogAnimation(int sentenceIndex)
        {
            // Verificação de índice da animação
            if (sentenceIndex >= dialogDatas.Count || dialogDatas[sentenceIndex].startTimeSentences < 0)
                return;
            _timelineManager.PlayAnimation(dialogDatas[sentenceIndex].startTimeSentences);
            
        }
        
        private void NextDialog()
        {
            if (!_dialogOn) return;
            // Se o diálogo ainda estiver sendo digitado, interrompe a digitação e mostra o texto completo
            if (_isTyping)
            {
                StopCoroutine(_typingCoroutine); // Para a coroutine de digitação
                dialogText.text = dialogDatas[_currentDialogueIndex].sentences[_currentSentenceIndex].GetLocalizedString(); // Mostra o texto completo
                IsTyping(false); // Define _isTyping como false para indicar que a digitação foi interrompida
                return; // Sai do método sem avançar para a próxima sentença
            }

            // Avança para a próxima sentença se todas as frases do diálogo atual não foram completadas
            if (_currentSentenceIndex < dialogDatas[_currentDialogueIndex].sentences.Count - 1)
            {
                _currentSentenceIndex++;
                ShowSentence(); // Mostra a próxima sentença
            }
            else
            {
                // Avança para o próximo diálogo se todas as frases do diálogo atual foram completadas
                _currentDialogueIndex++;
                _currentSentenceIndex = 0;

                // Verifica se o próximo diálogo existe
                if (_currentDialogueIndex < dialogDatas.Count)
                {
                    NextDialogAnimation(_currentDialogueIndex);
                    StartCoroutine(CloseAndOpenPanel());
                }
                else
                {
                    EndDialogue(); // Fim do diálogo se não houver mais diálogos para exibir
                }
            }
        }

        private void ShowSentence()
        {
            // Verificação de índice da frase
            if (_currentSentenceIndex >= dialogDatas[_currentDialogueIndex].sentences.Count)
            {
                DebugManager.LogError<DialogManager>("Índice de frase inválido: " + _currentSentenceIndex);
                return;
            }
            _typingCoroutine = StartCoroutine(TypeSentence(dialogDatas[_currentDialogueIndex].sentences[_currentSentenceIndex]));
        }
        private async void StopDialog()
        {
            _dialogOn = false;
            StopAllCoroutines();
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateMenuInitial.ToString()).ConfigureAwait(false);
        }

        private void EndDialogue()
        {
            DebugManager.Log<DialogManager>("Fim do diálogo!");
            StopDialog();
        }

        #endregion

        #region InputManager

        private void InputNextDialog(InputAction.CallbackContext context)
        {
            NextDialog();
        }

        #endregion

        #region Métodos Auxiliares

        private IEnumerator WaitForInitialization()
        {
            while (!GameManager.StateManager.GetCurrentState().StateFinalization)
            {
                yield return null;
            }

            // Inicia o primeiro diálogo quando o painel estiver totalmente aberto
            StartDialog(0, 0);
        }
        private IEnumerator TypeSentence(LocalizedString sentence)
        {
            dialogText.text = "";
            IsTyping(true);
            var localizedSentence = sentence.GetLocalizedString(); // Cache do resultado
            var typedText = new StringBuilder();

            // Controle de avanço por caractere
            foreach (var character in localizedSentence)
            {
                typedText.Append(character);
                dialogText.text = typedText.ToString();
                yield return new WaitForSeconds(letterSpeed);

                // Verificação de interrupção
                if (!_isTyping)
                {
                    break;
                }
            }
            // Mostrar a frase completa
            dialogText.text = localizedSentence;

            IsTyping(false);
        }

        private IEnumerator CloseAndOpenPanel()
        {
            // Aguarda até que a escrita da última sentença seja concluída
            while (_isTyping)
            {
                yield return null;
            }

            // Fecha o painel
            yield return PanelsHelper.TogglePanel(dialogPanel, openTimeDuration, closeTimeDuration, false, false);
            yield return new WaitForSeconds(closeTimeDuration + WaitTime); // Aguarda um pouco após fechar o painel

            // Limpa o texto e inicia a próxima sentença
            dialogText.text = "";
            nameText.text = dialogDatas[_currentDialogueIndex].nameSpeaker.GetLocalizedString();

            // Adiciona um pequeno atraso antes de começar a próxima sentença
            yield return new WaitForSeconds(openTimeDuration);

            // Abre o painel novamente
            yield return PanelsHelper.TogglePanel(dialogPanel, openTimeDuration, closeTimeDuration, true, false);

            // Inicia o próximo diálogo ou mostra a próxima frase
            if (_currentSentenceIndex < dialogDatas[_currentDialogueIndex].sentences.Count)
            {
                ShowSentence();
            }
            else
            {
                NextDialog();
            }
        }

        private void IsTyping(bool isSpeaking)
        {
            _isTyping = isSpeaking;
            nextCursor.SetActive(!isSpeaking);
            if (speakerAnimatorController != null)
            {
                speakerAnimatorController.SetBool(Speak, isSpeaking);
            }
        }
        
        #endregion
    }
}
