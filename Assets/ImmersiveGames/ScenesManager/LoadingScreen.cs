using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGames.ScenesManager
{
    public static class LoadingScreen
    {
        private static GameObject loadingScreen;
        private static Slider loadingProgressBar;

        public static void Initialize(Slider progressBar)
        {
            if (loadingProgressBar == null)
            {
                loadingProgressBar = progressBar;
            }
        }

        public static void Show()
        {
            if (loadingScreen == null)
            {
                loadingScreen = GameObject.Find("LoadingScreen"); // Supondo que você tenha um objeto de UI com esse nome
            }
            loadingScreen.SetActive(true);
            ShowProgressBar();
        }

        public static void Hide()
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(false);
            }
            HideProgressBar();
        }

        public static void UpdateProgress(float progress)
        {
            if (loadingProgressBar != null)
            {
                loadingProgressBar.value = progress;
            }
        }

        private static void ShowProgressBar()
        {
            if (loadingProgressBar != null)
            {
                loadingProgressBar.gameObject.SetActive(true);
            }
        }

        private static void HideProgressBar()
        {
            if (loadingProgressBar != null)
            {
                loadingProgressBar.gameObject.SetActive(false);
            }
        }
    }
}