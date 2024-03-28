using System;
using System.Threading;
using System.Threading.Tasks;

namespace ImmersiveGames.Utils
{
    public class TimeManager
    {
        private float _timer;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task _timerTask;
        private DateTime _lastIterationTime;

        public TimeManager(float duration)
        {
            _timer = duration;
            IsTimerExpired = false;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartTimer(Action onTimerExpired)
        {
            _lastIterationTime = DateTime.Now;
            // Iniciar a coroutine em uma thread separada
            _timerTask = TimerCoroutine(onTimerExpired, _cancellationTokenSource.Token);
        }

        private bool IsTimerExpired { get; set; }

        public void StopTimerAndExecuteCancellation(Action onCancelAction)
        {
            _cancellationTokenSource?.Cancel();
            onCancelAction?.Invoke(); // Executar a ação de cancelamento, se fornecida
            _timerTask?.Dispose(); // Liberar recursos do Task
            _cancellationTokenSource?.Dispose(); // Liberar recursos do CancellationTokenSource
        }

        public float GetNormalizedElapsedTime()
        {
            if (IsTimerExpired)
            {
                return 1.0f; // Timer expirado, tempo normalizado é 1 (100%)
            }
            if (_timer <= 0)
            {
                return 0.0f; // Tempo restante é zero, tempo normalizado é 0 (0%)
            }
            return 1.0f - (_timer / GetInitialDuration());
        }

        private float GetInitialDuration()
        {
            return _timer;
        }

        private async Task TimerCoroutine(Action onTimerExpired, CancellationToken cancellationToken)
        {
            try
            {
                while (_timer > 0)
                {
                    await Task.Delay(100, cancellationToken).ConfigureAwait(false);
                    var currentTime = DateTime.Now;
                    var elapsedTime = (float)(currentTime - _lastIterationTime).TotalSeconds;
                    _timer -= elapsedTime;
                    _lastIterationTime = currentTime;
                }

                IsTimerExpired = true;
                onTimerExpired();
            }
            finally
            {
                _cancellationTokenSource.Dispose(); // Liberar recursos do CancellationTokenSource no final da execução
            }
        }
    }
}
