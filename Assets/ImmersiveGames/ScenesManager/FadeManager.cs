using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGames
{
    public class FadeManager : MonoBehaviour
    {
        public static FadeManager instance { get; private set; }
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this);
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                Debug.LogError("FadeManager: CanvasGroup not found on the object.");
            }
        }

        public async Task FadeInAsync(float duration = 2.0f)
        {
            if (_canvasGroup != null)
            {
                await FadeAsync(true, duration).ConfigureAwait(false);
            }
            else
            {
                Debug.LogError("FadeManager: CanvasGroup not found on the object.");
            }
        }

        public async Task FadeOutAsync(float duration = 2.0f)
        {
            if (_canvasGroup != null)
            {
                await FadeAsync(false, duration).ConfigureAwait(false);
            }
            else
            {
                Debug.LogError("FadeManager: CanvasGroup not found on the object.");
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
                fadeCompletionSource.SetResult(true);
            }
        }
    }
}
