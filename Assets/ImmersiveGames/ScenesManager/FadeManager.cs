using System;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using UnityEngine;

namespace ImmersiveGames.ScenesManager
{
    public sealed class FadeManager : MonoBehaviour
    {
        public delegate void FadeManagerEventHandler();

        private event FadeManagerEventHandler EventFadeInStart;
        public event FadeManagerEventHandler EventFadeOutComplete;
        
        public static FadeManager Instance { get; private set; }
        
        public float durationFadeIn = 2.0f;
        public float durationFadeOut = 2.0f;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                DebugManager.LogError<FadeManager>("CanvasGroup not found on the object.");
            }

            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }

        public async Task FadeInAsync()
        {
            if (_canvasGroup != null)
            {
                MainThreadTaskExecutor.RunOnMainThread(OnEventFadeInStart);
                await FadeAsync(true, durationFadeIn).ConfigureAwait(false);
            }
            else
            {
                DebugManager.LogError<FadeManager>("CanvasGroup not found on the object.");
            }
        }

        public async Task FadeOutAsync()
        {
            if (_canvasGroup != null)
            {
                await FadeAsync(false, durationFadeOut).ConfigureAwait(false);
                MainThreadTaskExecutor.RunOnMainThread(OnEventOutComplete);
            }
            else
            {
                DebugManager.LogError<FadeManager>(" CanvasGroup not found on the object.");
            }
        }

        private async Task FadeAsync(bool fadeIn, float duration)
        {
            var startAlpha = fadeIn ? 0f : 1f;
            var targetAlpha = fadeIn ? 1f : 0f;
            var elapsedTime = 0f;

            // Create TaskCompletionSource to await the completion of the fade
            var fadeCompletionSource = new TaskCompletionSource<bool>();

            // Run the fade on the main thread
            MainThreadTaskExecutor.RunOnMainThread(FadeUpdate);

            // Wait for the fade to complete before returning
            await fadeCompletionSource.Task.ConfigureAwait(false);
            return;

            async void FadeUpdate()
            {
                while (elapsedTime < duration)
                {
                    await Task.Yield();
                    elapsedTime += Time.deltaTime;
                    _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
                }

                _canvasGroup.alpha = targetAlpha;
                _canvasGroup.blocksRaycasts = targetAlpha != 0;
                fadeCompletionSource.SetResult(true);
            }
        }

        private void OnEventOutComplete()
        {
            EventFadeOutComplete?.Invoke();
            DebugManager.Log<FadeManager>("Complete Fade out");
        }

        private void OnEventFadeInStart()
        {
            EventFadeInStart?.Invoke();
            DebugManager.Log<FadeManager>("Start Fade in");
        }
    }
}
