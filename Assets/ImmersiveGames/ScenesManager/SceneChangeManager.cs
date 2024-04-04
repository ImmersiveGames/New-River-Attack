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
            bool unloadPreviousAdditiveScene)
        {
            if (nextState == null || nextState.SceneName == previousSceneName || !nextState.RequiresSceneLoad)
            {
                return;
            }
            
            await MainThreadTaskExecutor.instance.RunOnMainThreadAsync(async () =>
            {
                if (SceneManager.GetActiveScene().name == nextState.SceneName) return;
                if (unloadPreviousAdditiveScene)
                {
                    // Descarrega a última cena aditiva
                    if (AdditiveScenes.Count > 0)
                    {
                        var previousAdditiveScene = AdditiveScenes.Pop();
                        await UnloadSceneAsync(previousAdditiveScene).ConfigureAwait(false);
                    }
                }

                await LoadSceneAsync(nextState.SceneName, loadSceneMode).ConfigureAwait(false);

                if (loadSceneMode == LoadSceneMode.Additive)
                {
                    // Empilha a nova cena aditiva
                    AdditiveScenes.Push(nextState.SceneName);
                }
                // Desative o painel de loading no final da transição
            }).ConfigureAwait(false);
        }

        private static async Task UnloadSceneAsync(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
                return;

            var unloadCompletionSource = new TaskCompletionSource<bool>();

            await MainThreadTaskExecutor.instance.RunOnMainThreadAsync(() =>
            {
                if (SceneManager.GetActiveScene().name == sceneName) return Task.CompletedTask;
                SceneManager.sceneUnloaded += SceneUnloaded;
                SceneManager.UnloadSceneAsync(sceneName);
                return Task.CompletedTask;
            }).ConfigureAwait(false);

            await unloadCompletionSource.Task.ConfigureAwait(false);

            void SceneUnloaded(Scene scene)
            {
                SceneManager.sceneUnloaded -= SceneUnloaded;
                unloadCompletionSource.SetResult(true);
                // Atualize a barra de progresso aqui (por exemplo, definindo-a como 100%)
                UpdateProgressBar(1.0f);
            }
        }

        private static async Task LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
        {
            if (string.IsNullOrEmpty(sceneName) || SceneManager.GetActiveScene().name == sceneName)
                return;

            var loadCompletionSource = new TaskCompletionSource<bool>();
            

            await MainThreadTaskExecutor.instance.RunOnMainThreadAsync(async () =>
            {
                SceneManager.sceneLoaded += SceneLoaded;
                instance.loadingProgressBar.gameObject.SetActive(true);
                var asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
                while (!asyncOperation.isDone)
                {
                    // Atualize a barra de progresso durante o carregamento
                    UpdateProgressBar(asyncOperation.progress);
                    await Task.Yield();
                }
                instance.loadingProgressBar.gameObject.SetActive(false);
            }).ConfigureAwait(false);
            await Task.Delay(500).ConfigureAwait(true);
            await loadCompletionSource.Task.ConfigureAwait(false);

            void SceneLoaded(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= SceneLoaded;
                loadCompletionSource.SetResult(true);
                // Atualize a barra de progresso aqui (por exemplo, definindo-a como 100%)
                UpdateProgressBar(1.0f);
            }
        }
        // Adicione o método para atualizar a barra de progresso
        private static void UpdateProgressBar(float progress)
        {
            instance.loadingProgressBar.value = progress;
        }
    }
}
