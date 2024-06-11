using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.StateManagers.Interfaces;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems;

namespace ImmersiveGames.StateManagers
{
    public class BehaviorManager
    {
        private readonly Dictionary<string, IBehavior> _behaviors = new Dictionary<string, IBehavior>();
        private IBehavior _currentBehavior;
        private IBehavior _previousBehavior;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isPaused;
        private readonly BossBehavior _bossBehavior;

        public BehaviorManager(BossBehavior bossBehavior)
        {
            _bossBehavior = bossBehavior;
        }

        public void AddBehavior(IBehavior behavior)
        {
            _behaviors[behavior.Name] = behavior;
        }

        public IBehavior GetCurrentBehavior()
        {
            return _currentBehavior;
        }

        public IBehavior GetPreviousBehavior()
        {
            return _previousBehavior;
        }

        public async Task ChangeBehaviorAsync(string behaviorName)
        {
            if (!_behaviors.TryGetValue(behaviorName, out var nextBehavior))
            {
                await UnityMainThreadDispatcher.EnqueueAsync(() =>
                {
                    DebugManager.LogError<BehaviorManager>($"Behavior not found: {behaviorName}");
                }).ConfigureAwait(false);
                return;
            }

            if (nextBehavior == _currentBehavior)
            {
                await UnityMainThreadDispatcher.EnqueueAsync(() =>
                {
                    DebugManager.Log<BehaviorManager>($"Already in behavior: {behaviorName}");
                }).ConfigureAwait(false);
                return;
            }

            _previousBehavior = _currentBehavior;
            _currentBehavior = nextBehavior;

            if (_previousBehavior != null)
            {
                _cancellationTokenSource?.Cancel();
                if (_cancellationTokenSource != null)
                    await _previousBehavior.ExitAsync(nextBehavior, _cancellationTokenSource.Token)
                        .ConfigureAwait(false);
                _previousBehavior.Finalization = true;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            await _currentBehavior.EnterAsync(_previousBehavior, _bossBehavior, _cancellationTokenSource.Token).ConfigureAwait(false);
            _currentBehavior.Initialized = true;

            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                DebugManager.Log<BehaviorManager>($"Changed to Behavior: {behaviorName}");
            }).ConfigureAwait(false);
        }

        public async Task ChangeBehaviorAsync(string behaviorName, int behaviorIndex)
        {
            await ChangeBehaviorAsync(behaviorName).ConfigureAwait(false);

            if (_currentBehavior is Behavior behavior)
            {
                await behavior.ChangeBehaviorAsync(behaviorIndex, _bossBehavior, _cancellationTokenSource.Token).ConfigureAwait(false);
            }
        }

        public async Task ChangeBehaviorAsync(string behaviorName, string subBehaviorName)
        {
            await ChangeBehaviorAsync(behaviorName).ConfigureAwait(false);

            if (_currentBehavior is Behavior behavior)
            {
                var subBehaviorIndex = behavior.GetBehaviorIndexByName(subBehaviorName);
                if (subBehaviorIndex != -1)
                {
                    await behavior.ChangeBehaviorAsync(subBehaviorIndex, _bossBehavior, _cancellationTokenSource.Token).ConfigureAwait(false);
                }
            }
        }

        public async Task RandomBehaviorAsync(params string[] excludeBehaviors)
        {
            var availableBehaviors = _behaviors.Keys.Except(excludeBehaviors).ToList();
            if (availableBehaviors.Count == 0)
            {
                DebugManager.LogError<BehaviorManager>("No available behavior for random selection.");
                return;
            }

            var randomBehaviorIndex = UnityEngine.Random.Range(0, availableBehaviors.Count);
            var nextBehaviorName = availableBehaviors[randomBehaviorIndex];
            await ChangeBehaviorAsync(nextBehaviorName).ConfigureAwait(false);
        }

        public async Task UpdateAsync()
        {
            if (_isPaused || _currentBehavior == null) return;

            await _currentBehavior.UpdateAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
        }

        public void PauseCurrentBehavior()
        {
            _isPaused = true;
            _currentBehavior?.Pause();
        }

        public void ResumeCurrentBehavior()
        {
            _isPaused = false;
            _currentBehavior?.Resume();
        }

        public void StopCurrentBehavior()
        {
            _cancellationTokenSource?.Cancel();
            _currentBehavior?.Stop();
        }
    }
}
