using System.Collections.Generic;
using System.Threading.Tasks;
using ImmersiveGames.StateManagers.Interfaces;
using ImmersiveGames.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ImmersiveGames.ScenesManager
{
    public class SceneChangeManager : Singleton<SceneChangeManager>
    {
        private static readonly Stack<string> AdditiveScenes = new Stack<string>();

        public Slider loadingProgressBar;

        public static async Task StartSceneTransitionAsync(IState nextState, string previousSceneName, LoadSceneMode loadSceneMode, 
            bool unloadPreviousAdditiveScene, bool reloadIfSameScene = false) // Novo parâmetro opcional
        {
            if (nextState == null || (nextState.SceneName == previousSceneName && !reloadIfSameScene) || !nextState.RequiresSceneLoad)
                return;

            await MainThreadDispatcher.EnqueueAsync(async () =>
            {
                // Remove a verificação do nome da cena ativa para permitir o recarregamento
                if (unloadPreviousAdditiveScene && AdditiveScenes.Count > 0)
                {
                    var previousAdditiveScene = AdditiveScenes.Pop();
                    await UnloadSceneAsync(previousAdditiveScene);
                }

                await LoadSceneAsync(nextState.SceneName, loadSceneMode);

                if (loadSceneMode == LoadSceneMode.Additive)
                {
                    AdditiveScenes.Push(nextState.SceneName);
                }
            });
        }


        private static async Task UnloadSceneAsync(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;

            var unloadCompletionSource = new TaskCompletionSource<bool>();

            try
            {
                await MainThreadDispatcher.EnqueueAsync(() =>
                {
                    if (SceneManager.GetActiveScene().name == sceneName) return Task.CompletedTask;
                    SceneManager.sceneUnloaded += SceneUnloaded;

                    var asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
                    if (asyncUnload != null)
                        asyncUnload.completed += _ => unloadCompletionSource.SetResult(true);

                    return Task.CompletedTask;
                });

                await unloadCompletionSource.Task.ConfigureAwait(false);
            }
            finally
            {
                SceneManager.sceneUnloaded -= SceneUnloaded;
            }

            UpdateProgressBar(1.0f);
        }

        private static async Task LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            if (string.IsNullOrEmpty(sceneName) || SceneManager.GetActiveScene().name == sceneName)
                return;

            var loadCompletionSource = new TaskCompletionSource<bool>();

            await MainThreadDispatcher.EnqueueAsync(async () =>
            {
                SceneManager.sceneLoaded += SceneLoaded;

                instance.loadingProgressBar.gameObject.SetActive(true);
                var asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

                while (asyncOperation is { isDone: false })
                {
                    UpdateProgressBar(asyncOperation.progress);
                    await Task.Yield();
                }

                instance.loadingProgressBar.gameObject.SetActive(false);
            });

            await loadCompletionSource.Task.ConfigureAwait(false);
        }

        private static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            UpdateProgressBar(1.0f);
        }

        private static void SceneUnloaded(Scene scene)
        {
            // Essa função é chamada quando a cena é completamente descarregado.
            UpdateProgressBar(1.0f); // Opcional: pode ser ajustada conforme necessário.
        }

        private static void UpdateProgressBar(float progress)
        {
            if (instance?.loadingProgressBar != null)
                instance.loadingProgressBar.value = progress;
        }
    }
}
