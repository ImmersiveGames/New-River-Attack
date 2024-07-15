using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems;

namespace ImmersiveGames.BehaviorsManagers
{
    public class BehaviorManager
    {
        private readonly Dictionary<string, IBehavior> _behaviors = new Dictionary<string, IBehavior>();
        private readonly CancellationTokenSource _cancellationTokenSource;
        internal readonly BossBehavior BossBehavior;
        private bool _isExitingCurrentBehavior;
        private bool _isFinalizingSubBehavior;
        internal int CurrentIndex;

        private IBehavior CurrentBehavior { get; set; }
        private BehaviorManager SubBehaviorManager { get; set; }

        public BehaviorManager(BossBehavior bossBehavior)
        {
            BossBehavior = bossBehavior;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void AddBehavior(IBehavior behavior)
        {
            _behaviors[behavior.Name] = behavior;
        }

        private void AddBehavior(IBehavior[] behaviors)
        {
            foreach (var behavior in behaviors)
            {
                AddBehavior(behavior);
            }
        }

        public async Task ChangeBehaviorAsync(string behaviorName, int subIndex = 0)
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
            DebugManager.Log<BehaviorManager>($"Current behavior: {behaviorName}");
            
            await Task.Delay(100).ConfigureAwait(false);
            CurrentBehavior.Initialized = true;
            _isExitingCurrentBehavior = false;

            await CurrentBehavior.EnterAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
            SubBehaviorManager = null;
            if (CurrentBehavior.SubBehaviors.Length > 0 && CurrentBehavior.SubBehaviors[subIndex] != null)
            {
                SubBehaviorManager = new BehaviorManager(BossBehavior);
                SubBehaviorManager.AddBehavior(CurrentBehavior.SubBehaviors);
                await SubBehaviorManager.ChangeBehaviorAsync(CurrentBehavior.SubBehaviors[subIndex].Name).ConfigureAwait(true);
            }
        }

        public async Task UpdateAsync()
        {
            if (CurrentBehavior is not { Initialized: true }) return;

            if (!CurrentBehavior.Finalized)
            {
                DebugManager.Log<BehaviorManager>($"Updating via Manager");
                await Task.Delay(100).ConfigureAwait(false);
                await CurrentBehavior.UpdateAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
            }
            
            if (SubBehaviorManager?.CurrentBehavior is { Initialized: true, Finalized: false })
            {
                DebugManager.Log<BehaviorManager>($"Updating Subs via Manager");
                await SubBehaviorManager.CurrentBehavior.UpdateAsync(_cancellationTokenSource.Token).ConfigureAwait(true);
            }

            if (SubBehaviorManager?.CurrentBehavior is { Initialized: true, Finalized: true } && !_isFinalizingSubBehavior)
            {
                _isFinalizingSubBehavior = true;
                DebugManager.Log<BehaviorManager>($"Finalizing Sub via Manager");
                await FinalizeBehavior(SubBehaviorManager.CurrentBehavior).ConfigureAwait(false);
                CurrentIndex++;
                if (CurrentIndex < CurrentBehavior.SubBehaviors.Length)
                {
                    DebugManager.Log<BehaviorManager>($"Tem um novo sub para carregar {CurrentIndex}");
                    await SubBehaviorManager.ChangeBehaviorAsync(CurrentBehavior.SubBehaviors[CurrentIndex].Name, CurrentIndex).ConfigureAwait(false);
                }
                else
                {
                    CurrentBehavior.Finalized = true;
                }
                _isFinalizingSubBehavior = false;
            }

            if (CurrentBehavior.Finalized && !_isExitingCurrentBehavior)
            {
                _isExitingCurrentBehavior = true;
                await FinalizeBehavior(CurrentBehavior).ConfigureAwait(false);

                if (SubBehaviorManager?.CurrentBehavior.Initialized == true)
                {
                    await FinalizeBehavior(SubBehaviorManager.CurrentBehavior).ConfigureAwait(false);
                }
            }
        }

        private async Task FinalizeBehavior(IBehavior behavior)
        {
            if (!behavior.Initialized) return;

            behavior.Initialized = false;
            await Task.Delay(100).ConfigureAwait(false);
            await behavior.ExitAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
        }

        public void StopCurrentBehavior()
        {
            CurrentIndex = 0;
            CurrentBehavior.Finalized = true;
        }

        public void StopCurrentSubBehavior()
        {
            SubBehaviorManager.CurrentBehavior.Finalized = true;
        }
    }
}
