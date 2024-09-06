using System;
using ImmersiveGames.InputManager;
using ImmersiveGames.MenuManagers.Abstracts;
using ImmersiveGames.ScenesManager;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.TimelineManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.StateManagers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers
{
    public class MainMenu: AbstractMenuManager
    {
        public PanelsMenuReference[] panelsMenuReferences;
        public Button[] disableButtons;
        public bool activeScreenWash;
        [SerializeField] private Transform screenWash;
        [SerializeField] private PlayableDirector playableDirector;

        private TimelineManager _timelineManager;
        private const float ScreenWashTimer = .5f;
        private float _animationTimeStart;

        private bool _canGoBack = true; // Flag para controle de retorno
        private bool _onAction = false;

        #region Unity Methods

        private void Start()
        {
            _onAction = false;
            InputGameManager.RegisterAction("BackButton", InputBackButton);

            SetMenu(panelsMenuReferences);
            screenWash.gameObject.SetActive(activeScreenWash);
            ActivateMenu(0);
            if (FadeManager.Instance)
                FadeManager.Instance.EventFadeOutComplete += DeactivateScreenWash;
        }

        private void OnDisable()
        {
            if (FadeManager.Instance)
                FadeManager.Instance.EventFadeOutComplete -= DeactivateScreenWash;
        }

        private void OnDestroy()
        {
            InputGameManager.UnregisterAction("BackButton", InputBackButton);
        }
        #endregion

        #region AbstractMenuManager

        protected override void OnEnterMenu(PanelsMenuReference panelsMenuGameObject)
        {
            _timelineManager = new TimelineManager(playableDirector);
            _animationTimeStart = panelsMenuGameObject.startTimelineAnimation;
            SetInitialInteractiveButtons(panelsMenuGameObject, true);
            StartPlayAnimations(_animationTimeStart);
        }

        protected override void OnExitMenu(PanelsMenuReference panelsMenuGameObject)
        {
            AudioManager.instance.PlayMouseClick();
            SetInitialInteractiveButtons(panelsMenuGameObject, false);
            _timelineManager = null;
        }

        #endregion
        
        private void InputBackButton(InputAction.CallbackContext context)
        {
            GoBack(); // Agora apenas chama o GoBack()
        }
        private void SetInitialInteractiveButtons(PanelsMenuReference panelButtons, bool interactive)
        {
            var allButtons = panelButtons.menuGameObject.GetComponentsInChildren<Button>();
            foreach (var button in allButtons)
            {
                //Debug.Log($"Button: {button.name}");
                if (disableButtons != null && Array.Exists(disableButtons, obj => obj == button))
                    interactive = false;
                button.interactable = interactive;
            }
        }

        #region Buttons

        public async void GotoBriefingRoom()
        {
            if(_onAction) return;
            _onAction = true;
            DisableOnPress(GetCurrentMenu);
            _canGoBack = false; // Desabilita o retorno
            AudioManager.instance.PlayMouseClick();
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateBriefingRoom.ToString()).ConfigureAwait(false);
        }

        public async void GotoClassicMode()
        {
            if(_onAction) return;
            _onAction = true;
            _canGoBack = false; // Desabilita o retorno
            AudioManager.instance.PlayMouseClick();
            GameManager.instance.gamePlayMode = GamePlayModes.ClassicMode;
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStatePlay.ToString()).ConfigureAwait(false);
        }

        public async void GotoMissionMode()
        {
            if(_onAction) return;
            _onAction = true;
            _canGoBack = false; // Desabilita o retorno
            AudioManager.instance.PlayMouseClick();
            GameManager.instance.gamePlayMode = GamePlayModes.MissionMode;
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateHub.ToString()).ConfigureAwait(false);
        }

        public void ButtonPlayAnimation(float startTime)
        {
            DisableOnPress(GetCurrentMenu);
            AudioManager.instance.PlayMouseClick();
            _timelineManager.PlayAnimation(startTime);
        }

        #endregion
        
        private void DeactivateScreenWash()
        {
            if (!activeScreenWash) return;
            screenWash.gameObject.SetActive(false);
            var canvasGroup = screenWash.gameObject.GetComponent<CanvasGroup>();
            StartCoroutine(FadeTransition.Fade(canvasGroup, true, ScreenWashTimer));
        }

        private void StartPlayAnimations(float timeAnimationStart)
        {
            if(timeAnimationStart >= 0)
                _timelineManager.PlayAnimation(timeAnimationStart);
        }

        public override void GoBack()
        {
            if (_canGoBack) // Verifica se o retorno está permitido
            {
                // Lógica de retorno original aqui
                base.GoBack(); // Se for necessário
            }
        }
    }
}