using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

        private void Update()
        {
            while (_actions.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }

        public static void Enqueue(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            _actions.Enqueue(action);
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

        public static Task<T> EnqueueAsync<T>(Func<T> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            var tcs = new TaskCompletionSource<T>();

            Enqueue(() =>
            {
                try
                {
                    var result = func();
                    tcs.SetResult(result);
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