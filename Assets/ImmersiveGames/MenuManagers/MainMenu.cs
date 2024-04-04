using System;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.MenuManagers.Abstracts;
using ImmersiveGames.ScenesManager;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers.Interfaces;
using ImmersiveGames.TimelineManagers;
using UnityEngine;
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
        
        private PlayersInputActions _inputActions;
        private ActionManager _actionManager;

        #region Unity Methods

        private void Start()
        {
            InputGameManager.RegisterAction("BackButton",InputBackButton );
            
            SetMenu(panelsMenuReferences);
            screenWash.gameObject.SetActive(activeScreenWash);
            ActivateMenu(0);
            if(FadeManager.Instance)
                FadeManager.Instance.EventFadeOutComplete += DeactivateScreenWash;
        }

        private void OnDisable()
        {
            if(FadeManager.Instance)
                FadeManager.Instance.EventFadeOutComplete -= DeactivateScreenWash;
        }
        
        private void OnDestroy()
        {
            InputGameManager.UnregisterAction("BackButton",InputBackButton );
        }
        #endregion

        #region AbstractMenuManager

        protected override void OnEnterMenu(PanelsMenuReference panelsMenuGameObject)
        {
            _timelineManager = new TimelineManager(playableDirector);
            _animationTimeStart = panelsMenuGameObject.startTimelineAnimation;
            SetInteractiveAllButtons(panelsMenuGameObject, true);
            StartPlayAnimations(_animationTimeStart);
        }

        protected override void OnExitMenu(PanelsMenuReference panelsMenuGameObject)
        {
            AudioManager.PlayMouseClick();
            SetInteractiveAllButtons(panelsMenuGameObject, false);
            _timelineManager = null;
        }

        #endregion
        
        private void InputBackButton(InputAction.CallbackContext context)
        {
            GoBack();
        }

        #region Buttons

        private void SetInteractiveAllButtons(PanelsMenuReference panelButtons, bool interactive)
        {
            var allButtons = panelButtons.menuGameObject.GetComponentsInChildren<Button>();
            foreach (var button in allButtons)
            {
                if (disableButtons != null && Array.Exists(disableButtons, obj => obj == button))
                    interactive = false;
                button.interactable = interactive;
            }   
        }
        public async void GotoBriefingRoom()
        {
            AudioManager.PlayMouseClick();
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateBriefingRoom.ToString()).ConfigureAwait(false);
        }
        public async void GotoClassicMode()
        {
            AudioManager.PlayMouseClick();
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateOpenGame.ToString()).ConfigureAwait(false);
        }
        public async void GotoMissionMode()
        {
            AudioManager.PlayMouseClick();
            await GameManager.StateManager.ChangeStateAsync(StatesNames.GameStateOpenGame.ToString()).ConfigureAwait(false);
        }
        public void ButtonPlayAnimation(float startTime)
        {
            AudioManager.PlayMouseClick();
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
        
    }
}