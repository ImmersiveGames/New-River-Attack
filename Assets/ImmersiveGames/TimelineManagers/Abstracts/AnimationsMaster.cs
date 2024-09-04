using Cinemachine;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;
using UnityEngine.Playables;

namespace ImmersiveGames.TimelineManagers.Abstracts
{
    public abstract class AnimationsMaster : MonoBehaviour
    {
        private PlayableDirector _playableDirector;
        protected GamePlayManager GamePlayManagerRef;
        protected TimelineManager TimelineManagerRef;
        protected CinemachineVirtualCamera VirtualCamera;
        
        protected virtual void SetInitialReferences()
        {
            GamePlayManagerRef = GamePlayManager.Instance;
            _playableDirector = GetComponent<PlayableDirector>();
            _playableDirector.gameObject.SetActive(true);
            TimelineManagerRef = new TimelineManager(_playableDirector);
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        protected virtual void PlayTimeline()
        {
            TimelineManagerRef.PlayAnimation(0);
        }
        public void DownPriority()
        {
            VirtualCamera.Priority = 9;
        }
    }
}