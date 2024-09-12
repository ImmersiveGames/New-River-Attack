using ImmersiveGames.DebugManagers;
using UnityEngine;
using UnityEngine.Playables;

namespace ImmersiveGames.TimelineManagers
{
    public class TimelineManager
    {
        private readonly PlayableDirector _playableDirector;

        public TimelineManager(PlayableDirector playableDirector)
        {
            _playableDirector = playableDirector;
        }

        public void PlayAnimation(float startTimer)
        {
            _playableDirector.Stop(); // Stop automatically pauses if it's playing
            if(startTimer >= 0)
                _playableDirector.time = startTimer;
            _playableDirector.Play();
        }
        public bool TryPlayAnimation(float startTime)
        {
            if (_playableDirector == null)
            {
                DebugManager.LogError<TimelineManager>("TimelineManager: PlayableDirector is null.");
                return false;
            }

            _playableDirector.Stop(); // Stop automatically pauses if it's playing
            if (startTime >= 0)
            {
                _playableDirector.time = startTime;
            }

            _playableDirector.Play();
            return true;
        }

        /*
         * ChangeBindingReference(string track, UnityEngine.Object animator)
         * - Substituir a referência nula pelo Animator desejado em um Timeline
         */
        public void ChangeBindingReference(string track, Object animator)
        {
            if (animator == null)
            {
                DebugManager.LogError<TimelineManager>("Animator is null. Cannot change binding reference.");
                return;
            }

            foreach (var playableBinding in _playableDirector.playableAsset.outputs)
            {
                if (playableBinding.streamName != track) continue;
                var bindingReference = _playableDirector.GetGenericBinding(playableBinding.sourceObject);

                if (bindingReference == null)
                {
                    // Substituir a referência nula pelo Animator desejado
                    _playableDirector.SetGenericBinding(playableBinding.sourceObject, animator);
                }
            }
        }
    }
}