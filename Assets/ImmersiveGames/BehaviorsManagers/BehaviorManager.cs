using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems;
using UnityEngine;

namespace ImmersiveGames.BehaviorsManagers
{
    public class BehaviorManager
    {
        private readonly Dictionary<string, IBehavior> _behaviors = new Dictionary<string, IBehavior>();
        private IBehavior _previousBehavior;
        private BehaviorManager _subManagerBehavior;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private bool _isPaused;
        private readonly BossBehavior _bossBehavior;

        public BehaviorManager(BossBehavior bossBehavior)
        {
            _bossBehavior = bossBehavior;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public IBehavior CurrentBehavior { get; private set; }

        public void AddBehavior(IBehavior behavior)
        {
            _behaviors[behavior.Name] = behavior;
        }

        public void AddBehavior(IBehavior[] behaviors)
        {
            foreach (var behavior in behaviors)
            {
                AddBehavior(behavior);
            }
        }

        public IBehavior GetBehavior(string nameBehavior)
        {
            return _behaviors[nameBehavior];
        }

        public async Task ChangeBehaviorAsync(string behaviorName)
        {
            if (!_behaviors.TryGetValue(behaviorName, out var nextBehavior))
            {
                DebugManager.LogError<BehaviorManager>($"Behavior not found: {behaviorName}");
                return;
            }

            if (nextBehavior == CurrentBehavior)
            {
                DebugManager.Log<BehaviorManager>($"Already in behavior: {behaviorName}");
                return;
            }
            CurrentBehavior = nextBehavior;
            await CurrentBehavior.ChangeBehaviorStrategy.ChangeBehaviorAsync(CurrentBehavior, _bossBehavior, _cancellationTokenSource).ConfigureAwait(false);
        }

        public async Task UpdateAsync()
        {
            if (_isPaused || CurrentBehavior == null) return;
            await CurrentBehavior.UpdateStrategy.UpdateAsync(CurrentBehavior, this, _cancellationTokenSource.Token).ConfigureAwait(false);
        }
        
        public async Task FinalizeSubBehaviorAsync(IBehavior subBehavior)
        {
            if (subBehavior.Initialized && !subBehavior.Finalized)
            {
                await subBehavior.ExitAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
            }
        }

        public void Pause()
        {
            _isPaused = true;
            CurrentBehavior?.Pause();
        }

        public void Resume()
        {
            _isPaused = false;
            CurrentBehavior?.Resume();
        }

        public void StopCurrentBehavior()
        {
            _cancellationTokenSource?.Cancel();
            CurrentBehavior?.Stop();
            CurrentBehavior = null;
        }
    }
}
