using System;
using Cinemachine;
using ImmersiveGames.MenuManagers.Abstracts;
using ImmersiveGames.ScenesManager;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.TimelineManagers;
using UnityEngine;
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

        #region Unity Methods

        private void Start()
        {
            SetMenu(panelsMenuReferences);
            screenWash.gameObject.SetActive(activeScreenWash);
            ActivateMenu(0);
            if(FadeManager.instance)
                FadeManager.instance.EventFadeOutComplete += DeactivateScreenWash;
        }

        private void OnDisable()
        {
            if(FadeManager.instance)
                FadeManager.instance.EventFadeOutComplete -= DeactivateScreenWash;
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
            SetInteractiveAllButtons(panelsMenuGameObject, false);
            _timelineManager = null;
        }

        #endregion

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
        public async void ButtonBriefingRoom()
        {
            await InitializationManager.StateManager.ChangeStateAsync("GameStateBriefingRoom").ConfigureAwait(false);
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