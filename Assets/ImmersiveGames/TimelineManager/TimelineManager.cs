using ImmersiveGames.DebugManagers;
using UnityEngine;
using UnityEngine.Playables;

namespace ImmersiveGames.TimelineManager
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

        /*
         * ChangeBindingReference(string track, UnityEngine.Object animator)
         * - Substituir a referência nula pelo Animator desejado em um Timeline
         */
        public void ChangeBindingReference(string track, Object animator)
        {
            if (animator == null)
            {
                DebugManager.LogError("Animator is null. Cannot change binding reference.");
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