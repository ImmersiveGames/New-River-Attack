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
        }

        private void OnDestroy()
        {
            GamePlayManagerRef.EventPlayerInitialize -= SetupTimeline;
            GamePlayManagerRef.EventPostStateGameInitialize -= PlayTimeline;
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

        public void DisableAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}