using Cinemachine;
using ImmersiveGames.GamePlayManagers;
using ImmersiveGames.PlayerManagers.PlayerSystems;
using UnityEngine;
using UnityEngine.Playables;

namespace ImmersiveGames.TimelineManagers
{
    public class OpenAnimationMaster : MonoBehaviour
    {
        private PlayableDirector _playableDirector;
        private GamePlayManager _gamePlayManager;
        private TimelineManager _timelineManager;
        private CinemachineVirtualCamera _virtualCamera;

        private void Awake()
        {
            SetInitialReferences();
            _gamePlayManager.EventPlayerInitialize += SetupTimeline;
            _gamePlayManager.EventGameInitialize += PlayTimeline;
        }

        private void OnDestroy()
        {
            _gamePlayManager.EventPlayerInitialize -= SetupTimeline;
            _gamePlayManager.EventGameInitialize -= PlayTimeline;
        }

        private void SetupTimeline(PlayerMaster playerMaster)
        {
            var playerAnimator = playerMaster.GetPlayerAnimator();
            _timelineManager.ChangeBindingReference("Animation Track", playerAnimator);
        }

        private void PlayTimeline()
        {
            _timelineManager.PlayAnimation(0);
        }

        public void DownPriority()
        {
            _virtualCamera.Priority = 9;
        }
        

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _playableDirector = GetComponent<PlayableDirector>();
            _playableDirector.gameObject.SetActive(true);
            _timelineManager = new TimelineManager(_playableDirector);
            _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            _virtualCamera.Priority = 10;

        }
    }
}