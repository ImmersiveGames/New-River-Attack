using ImmersiveGames.MenuManagers.Abstracts;
using ImmersiveGames.MenuManagers.Interfaces;
using ImmersiveGames.ScenesManager;
using ImmersiveGames.ScenesManager.Transitions;
using UnityEngine;
using UnityEngine.Playables;

namespace ImmersiveGames.MenuManagers
{
    public class MainMenu: AbstractMenuManager, IMenuTimelineAnimation
    {
        public bool activeScreenWash;
        [SerializeField] private Transform screenWash;
        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] private MenuTimelineReference[] timelineReferences;

        public MenuTimelineReference[] TimelineReferences
        {
            get => timelineReferences;
            set { }
        }

        private TimelineManager.TimelineManager _timelineManager;
        private const float ScreenWashTimer = .5f;
        private float _animationTimeStart;

        #region Unity Methods

        private void Start()
        {
            screenWash.gameObject.SetActive(activeScreenWash);
            ActivateMenu(0);
            FadeManager.instance.EventFadeOutComplete += DeactivateScreenWash;
        }

        private void OnDisable()
        {
            FadeManager.instance.EventFadeOutComplete -= DeactivateScreenWash;
        }
        #endregion

        #region AbstractMenuManager

        protected override void OnEnterMenu(GameObject menuGameObject)
        {
            _timelineManager = new TimelineManager.TimelineManager(playableDirector);
            _animationTimeStart = GetTimeAnimationByGameObject(menuGameObject);
        }

        protected override void OnExitMenu(GameObject menuGameObject)
        {
            _timelineManager = null;
        }

        #endregion

        #region Interface IMenuTimelineAnimation
        
        public void TimelinePlayAnimation(float animationTimeStart)
        {
            _timelineManager?.PlayAnimation(animationTimeStart);
        }
        
        public float GetTimeAnimationByGameObject(GameObject menuGameObject)
        {
            var menuRef = System.Array.Find(timelineReferences, timeline => timeline.menuGameObject == menuGameObject);
            return menuRef?.timeAnimation ?? 0f; // or any default value you prefer
        }

        public async void ButtonBriefingRoom()
        {
            await InitializationManager.StateManager.ChangeStateAsync("GameStateBriefingRoom").ConfigureAwait(false);
        }

        #endregion
        
        private void DeactivateScreenWash()
        {
            if (activeScreenWash)
            {
                screenWash.gameObject.SetActive(false);
                var canvasGroup = screenWash.gameObject.GetComponent<CanvasGroup>();
                StartCoroutine(FadeTransition.Fade(canvasGroup, true, ScreenWashTimer));
            }
            if(playableDirector)
                TimelinePlayAnimation(_animationTimeStart);
        }
        
    }
}