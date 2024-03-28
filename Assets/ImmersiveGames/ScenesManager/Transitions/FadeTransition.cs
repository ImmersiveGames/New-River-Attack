using System.Collections;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using UnityEngine;

namespace ImmersiveGames.ScenesManager.Transitions
{
    public class FadeTransition : ITransition
    {
        public async Task InTransitionAsync()
        {
            // Implement your fade-in or fade-out logic here
            DebugManager.Log<FadeTransition>("Performing fade IN transition");
            await FadeManager.Instance.FadeInAsync().ConfigureAwait(false);
        }
        public async Task OutTransitionAsync()
        {
            // Implement your fade-in or fade-out logic here
            DebugManager.Log<FadeTransition>("Performing fade Out transition");
            await FadeManager.Instance.FadeOutAsync().ConfigureAwait(false);
        }

        public static IEnumerator Fade(CanvasGroup canvasGroup, bool fadeIn, float fadeDuration)
        {
            var targetAlpha = fadeIn ? 1f : 0f;
            var startAlpha = canvasGroup.alpha;

            var elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                var alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                canvasGroup.alpha = alpha;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;

            if (!fadeIn)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.gameObject.SetActive(false);
            }
            else
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
        
    }
}