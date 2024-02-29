using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace ImmersiveGames
{
    public class SceneChangeManager : Singleton<SceneChangeManager>
    {
        private static readonly Stack<string> AdditiveScenes = new Stack<string>();

        public static async Task StartSceneTransitionAsync(IState nextState, string previousSceneName, LoadSceneMode loadSceneMode, 
            bool unloadPreviousAdditiveScene)
        {
            if (nextState == null || nextState.sceneName == previousSceneName || !nextState.requiresSceneLoad)
            {
                return;
            }

            await MainThreadTaskExecutor.instance.RunOnMainThreadAsync(async () =>
            {
                if (unloadPreviousAdditiveScene)
                {
                    // Descarrega a última cena aditiva
                    if (AdditiveScenes.Count > 0)
                    {
                        var previousAdditiveScene = AdditiveScenes.Pop();
                        await UnloadSceneAsync(previousAdditiveScene).ConfigureAwait(false);
                    }
                }

                await LoadSceneAsync(nextState.sceneName, loadSceneMode).ConfigureAwait(false);

                if (loadSceneMode == LoadSceneMode.Additive)
                {
                    // Empilha a nova cena aditiva
                    AdditiveScenes.Push(nextState.sceneName);
                }
            }).ConfigureAwait(false);
        }

        private static async Task UnloadSceneAsync(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
                return;

            var unloadCompletionSource = new TaskCompletionSource<bool>();

            await MainThreadTaskExecutor.instance.RunOnMainThreadAsync(() =>
            {
                SceneManager.sceneUnloaded += SceneUnloaded;
                SceneManager.UnloadSceneAsync(sceneName);
                return Task.CompletedTask;
            }).ConfigureAwait(false);

            await unloadCompletionSource.Task.ConfigureAwait(false);

            void SceneUnloaded(Scene scene)
            {
                SceneManager.sceneUnloaded -= SceneUnloaded;
                unloadCompletionSource.SetResult(true);
            }
        }

        private static async Task LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            if (string.IsNullOrEmpty(sceneName))
                return;

            var loadCompletionSource = new TaskCompletionSource<bool>();

            await MainThreadTaskExecutor.instance.RunOnMainThreadAsync(() =>
            {
                SceneManager.sceneLoaded += SceneLoaded;
                SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
                return Task.CompletedTask;
            }).ConfigureAwait(false);

            await loadCompletionSource.Task.ConfigureAwait(false);

            void SceneLoaded(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= SceneLoaded;
                loadCompletionSource.SetResult(true);
            }
        }
    }
}
