using System.Collections.Generic;
using System.Linq;
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
        
        private bool _inTransition;
        private int _currentIndex;

        public IBehavior CurrentBehavior { get; private set; }

        public BehaviorManager SubBehaviorManager { get; private set; }

        public BehaviorManager(BossBehavior bossBehavior)
        {
            BossBehavior = bossBehavior;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void AddBehavior(IBehavior behavior)
        {
            _behaviors[behavior.Name] = behavior;
        }
        

        public string[] GetBehaviorsNames(string remove)
        {
            var keys = _behaviors.Keys.ToArray();
            return string.IsNullOrEmpty(remove) ? keys : keys.Where(valuePair => valuePair != remove).ToArray();
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
            if (_inTransition) return;

            DebugManager.Log<BehaviorManager>($"Chamou o Change: {behaviorName}");
            _inTransition = true;

            var token = _cancellationTokenSource.Token;

            if (!_behaviors.TryGetValue(behaviorName, out var nextBehavior))
            {
                DebugManager.LogError<BehaviorManager>($"Behavior not found: {behaviorName}");
                _inTransition = false;
                return;
            }

            if (CurrentBehavior != null)
            {
                if (SubBehaviorManager != null)
                {
                    SubBehaviorManager.CurrentBehavior.Finalized = true;
                    DebugManager.Log<BehaviorManager>($"Sub Finalizar: {CurrentBehavior.Name}");
                    await SubBehaviorManager.CurrentBehavior.ExitAsync(token).ConfigureAwait(false);
                    SubBehaviorManager.CurrentBehavior.Initialized = false;
                }
                CurrentBehavior.Finalized = true;
                DebugManager.Log<BehaviorManager>($"Finalizar: {CurrentBehavior.Name}");
                await CurrentBehavior.ExitAsync(token).ConfigureAwait(false);
                CurrentBehavior.Initialized = false;
            }

            CurrentBehavior = nextBehavior;
            CurrentBehavior.Finalized = false;
            CurrentBehavior.Initialized = true;

            await Task.Delay(100, token).ConfigureAwait(false);

            DebugManager.Log<BehaviorManager>($"Entrar: {CurrentBehavior.Name}");
            await CurrentBehavior.EnterAsync(token).ConfigureAwait(false);

            if (CurrentBehavior.SubBehaviors.Length > 0)
            {
                SubBehaviorManager = new BehaviorManager(BossBehavior);
                SubBehaviorManager.AddBehavior(CurrentBehavior.SubBehaviors);
                await Task.Delay(100, token).ConfigureAwait(false);
                await SubBehaviorManager.ChangeBehaviorAsync(CurrentBehavior.SubBehaviors[subIndex].Name).ConfigureAwait(true);
            }
            _inTransition = false;
        }
        public void UpdateAsync()
        {
            if (CurrentBehavior == null) return;
            if (_inTransition) return;
            DebugManager.Log<BehaviorManager>($"Updating {CurrentBehavior.Name}: Transition: {_inTransition}, Initialized: {CurrentBehavior?.Initialized}, Finalized: {CurrentBehavior?.Finalized}");
            CurrentBehavior?.UpdateAsync(_cancellationTokenSource.Token);

            if (SubBehaviorManager?.CurrentBehavior == null) return;
            if (SubBehaviorManager._inTransition && _inTransition) return;
            DebugManager.Log<BehaviorManager>($"Updating Sub {SubBehaviorManager.CurrentBehavior.Name}: Transition: {SubBehaviorManager._inTransition}, Initialized: {SubBehaviorManager.CurrentBehavior?.Initialized}, Finalized: {SubBehaviorManager.CurrentBehavior?.Finalized}");
            SubBehaviorManager.CurrentBehavior?.UpdateAsync(_cancellationTokenSource.Token);
        }
    }
}
