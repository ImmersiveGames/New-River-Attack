using System.Threading.Tasks;
using ImmersiveGames.GameStateSystem.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.GameStateSystem.GameStates
{
    public class GameStatesRunning : IGameState, IGameStateScene, IGameStateTransition
    {
        public GameStatesRunning(string stateName)
        {
            StateName = stateName;
        }

        public string StateName { get; }
        public void EnterState(IGameState previousState)
        {
            Debug.Log($"Enter State{StateName}");
        }

        public void ExitState(IGameState nextState)
        {
            Debug.Log($"Exit State{StateName}");
        }

        public async Task EnterScene()
        {
           // await GameSceneManager.instance.LoadSceneAsync(SceneName,LoadSceneMode.Additive).ConfigureAwait(false);
            Debug.Log($"Enter Cena: {StateName} new TESTE");
        }

        public async Task ExitScene()
        {
            //await GameSceneManager.instance.UnloadSceneAsync(SceneName).ConfigureAwait(false);
            Debug.Log($"Exit Cena: {StateName} new TESTE");
        }
        public string SceneName => "New TESTE";
    }
}