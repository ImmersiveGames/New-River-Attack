using System;
using System.Threading.Tasks;

namespace ImmersiveGames.Utils
{
    public class TimeManager
    {
        private float _timer;

        public void StartTimer(float duration, Action onTimerExpired)
        {
            _timer = duration;
            IsTimerExpired = false;

            // Iniciar a coroutine em uma thread separada
            Task.Run(() => TimerCoroutine(onTimerExpired));
        }

        public bool IsTimerExpired { get; private set; }

        private async Task TimerCoroutine(Action onTimerExpired)
        {
            while (_timer > 0)
            {
                await Task.Delay(100).ConfigureAwait(false);
                _timer -= 0.1f;
            }

            IsTimerExpired = true;
            onTimerExpired();
        }
    }
}