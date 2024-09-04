using ImmersiveGames.TimelineManagers.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace ImmersiveGames.TimelineManagers
{
    public class OpenAnimationMaster : AnimationsMaster
    {

        private void Awake()
        {
            SetInitialReferences();
            GamePlayManagerRef.EventPlayerInitialize += SetupTimeline;
            GamePlayManagerRef.EventPostStateGameInitialize += PlayTimeline;
            GamePlayManagerRef.EventGameReload += ResetTimeLine;
        }

        private void OnDestroy()
        {
            GamePlayManagerRef.EventPlayerInitialize -= SetupTimeline;
            GamePlayManagerRef.EventPostStateGameInitialize -= PlayTimeline;
            GamePlayManagerRef.EventGameReload -= ResetTimeLine;
        }

        private void SetupTimeline(PlayerMaster playerMaster)
        {
            gameObject.SetActive(true);
            var playerAnimator = playerMaster.GetComponent<Animator>();
            TimelineManagerRef.ChangeBindingReference("Animation Track", playerAnimator);
        }
        
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            VirtualCamera.Priority = 10;
        }
        private void ResetTimeLine()
        {
            VirtualCamera.Priority = 10;
            PlayTimeline();
        }

        public void DisableAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}