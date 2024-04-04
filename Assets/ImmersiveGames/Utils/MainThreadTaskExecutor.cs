using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public class MainThreadTaskExecutor : MonoBehaviour
    {
        private static MainThreadTaskExecutor _instance;
        private static readonly Queue<Action> MainThreadActions = new Queue<Action>();
        private static readonly object Lock = new object();

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            lock (Lock)
            {
                while (MainThreadActions.Count > 0)
                {
                    var action = MainThreadActions.Dequeue();
                    action?.Invoke();
                }
            }
        }

        public static MainThreadTaskExecutor instance
        {
            get
            {
                if (_instance != null) return _instance;
                var go = new GameObject("MainThreadTaskExecutor");
                _instance = go.AddComponent<MainThreadTaskExecutor>();

                return _instance;
            }
        }
        public async Task RunOnMainThreadAsync(Func<Task> action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            lock (Lock)
            {
                async void Item()
                {
                    await action.Invoke().ConfigureAwait(false);
                    taskCompletionSource.SetResult(true);
                }

                MainThreadActions.Enqueue(Item);
            }

            // Espera a tarefa ser concluída na thread principal
            await taskCompletionSource.Task.ConfigureAwait(false);
        }
        public static void RunOnMainThread(Action action)
        {
            lock (Lock)
            {
                MainThreadActions.Enqueue(action);
            }
        }
    }
}