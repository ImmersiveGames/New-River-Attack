using System;
using System.Threading.Tasks;
using ImmersiveGames.GameStateSystem.Interfaces;
using UnityEngine;

namespace ImmersiveGames.GameStateSystem
{
    public class SceneEventManager : MonoBehaviour
    {
        public static event Action EventSceneLoadStart;
        public static event Action EventSceneLoadComplete;
        public static event Action<IGameStateScene> EventStateChange;

        public static void OnSceneLoadStart()
        {
            EventSceneLoadStart?.Invoke();
        }

        public static void OnSceneLoadComplete()
        {
            EventSceneLoadComplete?.Invoke();
        }

        public static Task OnEventStateChange(IGameStateScene gameState)
        {
            EventStateChange?.Invoke(gameState);
            return Task.CompletedTask;
        }
    }
}