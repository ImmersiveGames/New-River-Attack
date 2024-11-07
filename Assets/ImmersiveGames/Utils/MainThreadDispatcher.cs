using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static MainThreadDispatcher _instance;
        private static readonly ConcurrentQueue<Action> ActionsQueue = new ConcurrentQueue<Action>();

        private void Awake()
        {
            // Garantir que não haja duplicatas de MainThreadDispatcher
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            // Processar todas as ações enfileiradas na thread principal
            while (ActionsQueue.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }

        public static MainThreadDispatcher Instance
        {
            get
            {
                Initialize(); // Garante que o dispatcher é inicializado
                return _instance;
            }
        }

        // Método de inicialização seguro
        public static void Initialize()
        {
            if (_instance != null) return;

            // Cria o objeto no caso de ainda não existir
            var go = new GameObject("MainThreadDispatcher");
            _instance = go.AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(go);
        }

        public static void Enqueue(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            ActionsQueue.Enqueue(action);
        }

        public static Task EnqueueAsync(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            var tcs = new TaskCompletionSource<bool>();
            Enqueue(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        public static Task EnqueueAsync(Func<Task> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            var tcs = new TaskCompletionSource<bool>();

            Enqueue(async () =>
            {
                try
                {
                    await action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }
    }
}
