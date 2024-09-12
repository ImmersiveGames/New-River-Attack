using System.Collections;
using System.Collections.Generic;
using System.Text;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.SteamServicesManagers;
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
        private bool _isTyping;
        private bool _sentenceComplete;
        private bool _isPanelTransitioning;
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
            InputGameManager.RegisterAction("Next", InputNextDialog);
            dialogText.text = "";
            nameText.text = "";
            StartCoroutine(StartDialogAfterDelay());
        }

        private void OnDisable()
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);
            InputGameManager.ActionManager.EventOnActionComplete -= StopDialog;
        }

        #endregion

        #region Métodos de Diálogo

        private IEnumerator StartDialogAfterDelay()
        {
            yield return new WaitForSeconds(2f); // Atraso inicial antes de começar o diálogo
            StartDialog(0, 0);
        }

        private void StartDialog(int dialogIndex, int sentenceIndex)
        {
            if (dialogIndex >= dialogDatas.Count)
            {
                DebugManager.LogError<DialogManager>("Índice de diálogo inválido: " + dialogIndex);
                return;
            }

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

        private void NextDialog()
        {
            if (!_dialogOn || _isPanelTransitioning) return;

            // Se a sentença ainda está sendo digitada, exibe o texto completo imediatamente
            if (_isTyping)
            {
                StopCoroutine(_typingCoroutine);
                dialogText.text = dialogDatas[_currentDialogueIndex].sentences[_currentSentenceIndex].GetLocalizedString();
                IsTyping(false);
                _sentenceComplete = true;
            }
            // Se a sentença já foi completamente exibida, avança para a próxima
            else if (_sentenceComplete)
            {
                StartCoroutine(WaitBeforeNextSentence());
            }
        }

        private IEnumerator WaitBeforeNextSentence()
        {
            yield return new WaitForSeconds(0.5f); // Pequeno atraso antes de avançar

            // Avança para a próxima sentença
            if (_currentSentenceIndex < dialogDatas[_currentDialogueIndex].sentences.Count - 1)
            {
                _currentSentenceIndex++;
                ShowSentence();
            }
            // Se for a última sentença do conjunto, transita para o próximo conjunto de diálogo
            else
            {
                _currentDialogueIndex++;
                _currentSentenceIndex = 0;

                if (_currentDialogueIndex < dialogDatas.Count)
                {
                    StartCoroutine(CloseAndOpenPanel());
                }
                else
                {
                    EndDialogue(); // Fim do diálogo
                }
            }
        }

        private void ShowSentence()
        {
            if (_currentSentenceIndex >= dialogDatas[_currentDialogueIndex].sentences.Count)
            {
                DebugManager.LogError<DialogManager>("Índice de frase inválido: " + _currentSentenceIndex);
                return;
            }

            _sentenceComplete = false; // Reset a flag para a nova sentença
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
            SteamAchievementService.Instance.UnlockAchievement("ACH_FINISH_TUTORIAL");
            StopDialog();
        }

        private void NextDialogAnimation(int sentenceIndex)
        {
            if (sentenceIndex >= dialogDatas.Count || dialogDatas[sentenceIndex].startTimeSentences < 0)
                return;

            _timelineManager.PlayAnimation(dialogDatas[sentenceIndex].startTimeSentences);
        }

        #endregion

        #region InputManager

        private void InputNextDialog(InputAction.CallbackContext context)
        {
            NextDialog();
        }

        #endregion

        #region Métodos Auxiliares

        private IEnumerator TypeSentence(LocalizedString sentence)
        {
            dialogText.text = "";
            IsTyping(true);
            var localizedSentence = sentence.GetLocalizedString();
            var typedText = new StringBuilder();

            foreach (var character in localizedSentence)
            {
                typedText.Append(character);
                dialogText.text = typedText.ToString();
                yield return new WaitForSeconds(letterSpeed);

                if (!_isTyping)
                {
                    break;
                }
            }

            dialogText.text = localizedSentence;
            IsTyping(false);
            _sentenceComplete = true;
        }

        private IEnumerator CloseAndOpenPanel()
        {
            _isPanelTransitioning = true;

            // Fecha o painel
            yield return PanelsHelper.TogglePanel(dialogPanel, openTimeDuration, closeTimeDuration, false, false);
            yield return new WaitForSeconds(closeTimeDuration + 0.2f); // Pequeno atraso após fechar o painel

            dialogText.text = "";
            nameText.text = dialogDatas[_currentDialogueIndex].nameSpeaker.GetLocalizedString();

            // Reproduz a animação do próximo conjunto de diálogos
            NextDialogAnimation(_currentDialogueIndex);

            yield return new WaitForSeconds(openTimeDuration); // Espera antes de abrir o painel

            // Abre o painel com o próximo conjunto de diálogo
            yield return PanelsHelper.TogglePanel(dialogPanel, openTimeDuration, closeTimeDuration, true, false);

            _isPanelTransitioning = false;

            if (_currentSentenceIndex < dialogDatas[_currentDialogueIndex].sentences.Count)
            {
                ShowSentence();
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
