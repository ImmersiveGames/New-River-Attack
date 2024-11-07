using System;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.GameStateSystem.Interfaces;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.GameStateSystem
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        [SerializeField] private string nameLoadScene;

        public async Task LoadingSceneAsync(IGameStateScene obj, IGameStateScene previous)
        {
            // 1. Carrega a cena de loading na *main thread* e aguarda até que esteja totalmente carregada
            await LoadSceneAsync(nameLoadScene, LoadSceneMode.Additive);

            // 2. Descarrega a cena anterior (mantendo apenas a de loading ativa)
            if (previous != null)
            {
                var previousName = previous.SceneName ?? SceneManager.GetActiveScene().name;
                await UnloadSceneAsync(previousName);
            }
          
            // 3. Carrega a nova cena e aguarda o carregamento completo
            var currentName = obj?.SceneName;
            await LoadSceneAsync(currentName, LoadSceneMode.Additive);

            // 4. Descarrega a cena de loading, deixando apenas a nova cena ativa
            await UnloadSceneAsync(nameLoadScene);
        }

        private async Task LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Additive)
        {
            // Verifica se a cena já está carregada
            var existingScene = SceneManager.GetSceneByName(sceneName);
            if (existingScene.isLoaded)
            {
                Debug.LogWarning($"A cena {sceneName} já está carregada.");
                return;
            }

            var tcs = new TaskCompletionSource<bool>();

            // Inicia o carregamento assíncrono da cena
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, loadMode);
            if (loadOperation == null)
            {
                Debug.LogError($"A cena {sceneName} não pôde ser carregada.");
                tcs.SetResult(false);
                return;
            }

            // Impede a ativação automática da cena até o progresso estar em 90%
            loadOperation.allowSceneActivation = false;

            // Monitora o progresso e ativa a cena quando o carregamento estiver completo
            loadOperation.completed += _ => tcs.SetResult(true);
            while (loadOperation.progress < 0.9f)
            {
                await Task.Delay(100); // Aguarda um curto período para atualizar o progresso sem bloquear a *main thread*
            }

            loadOperation.allowSceneActivation = true;
            await tcs.Task;
        }

        private async Task UnloadSceneAsync(string sceneName)
        {
            // Verifica se a cena está carregada
            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded)
            {
                Debug.LogWarning($"A cena {sceneName} não está carregada ou já foi descarregada.");
                return;
            }

            var tcs = new TaskCompletionSource<bool>();

            // Inicia o descarregamento da cena
            var unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
            if (unloadOperation == null)
            {
                Debug.LogError($"Falha ao iniciar o descarregamento da cena {sceneName}.");
                tcs.SetResult(false);
                return;
            }

            unloadOperation.completed += _ => tcs.SetResult(true);
            await tcs.Task;
        }
    }
}
