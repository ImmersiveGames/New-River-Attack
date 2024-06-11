using System;
using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.StateManagers.Interfaces;
using ImmersiveGames.Utils;
using UnityEngine;

namespace ImmersiveGames.StateManagers
{
    public abstract class Behavior : IBehavior
    {
        public string Name { get; protected set; }
        public bool Initialized { get; set; }
        public bool Finalization { get; set; }
        private readonly IBehavior[] _behaviors;
        private int _currentBehaviorIndex;
        private bool _isPaused;

        protected Behavior(string name, IBehavior[] behaviors)
        {
            Name = name;
            _behaviors = behaviors ?? Array.Empty<IBehavior>();
        }

        public virtual async Task EnterAsync(IBehavior previousBehavior, MonoBehaviour monoBehaviour, CancellationToken token)
        {
            await UnityMainThreadDispatcher.EnqueueAsync(async () =>
            {
                DebugManager.Log<Behavior>($"Entering Behavior: {Name}");
                _currentBehaviorIndex = 0;
                while (_currentBehaviorIndex < _behaviors.Length && _behaviors[_currentBehaviorIndex] != null)
                {
                    await _behaviors[_currentBehaviorIndex].EnterAsync(previousBehavior, monoBehaviour, token).ConfigureAwait(false);
                    _currentBehaviorIndex++;
                }
            }).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(CancellationToken token)
        {
            if (_isPaused || token.IsCancellationRequested) return;

            if (_behaviors.Length > 0 && _behaviors[_currentBehaviorIndex] != null)
            {
                await _behaviors[_currentBehaviorIndex].UpdateAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task ExitAsync(IBehavior nextBehavior, CancellationToken token)
        {
            if (_behaviors.Length > 0 && _behaviors[_currentBehaviorIndex] != null)
            {
                await _behaviors[_currentBehaviorIndex].ExitAsync(nextBehavior, token).ConfigureAwait(false);
            }
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                DebugManager.Log<Behavior>($"Exiting Behavior: {Name}");
            }).ConfigureAwait(false);
        }

        public virtual async Task TransitionToAsync(IBehavior nextBehavior, MonoBehaviour monoBehaviour, CancellationToken token)
        {
            await ExitAsync(nextBehavior, token).ConfigureAwait(false);
            await nextBehavior.EnterAsync(this, monoBehaviour,token).ConfigureAwait(false);
        }

        public async Task ChangeBehaviorAsync(int newIndex, MonoBehaviour monoBehaviour, CancellationToken token)
        {
            if (newIndex < 0 || newIndex >= _behaviors.Length || _behaviors[newIndex] == null) return;
            if (_behaviors[_currentBehaviorIndex] != null)
            {
                await _behaviors[_currentBehaviorIndex].ExitAsync(_behaviors[newIndex], token).ConfigureAwait(false);
            }
            _currentBehaviorIndex = newIndex;
            await _behaviors[_currentBehaviorIndex].EnterAsync(_behaviors[newIndex], monoBehaviour, token).ConfigureAwait(false);
        }

        public int GetBehaviorIndexByName(string subBehaviorName)
        {
            for (var i = 0; i < _behaviors.Length; i++)
            {
                if (_behaviors[i]?.Name == subBehaviorName)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        public void Stop()
        {
            _isPaused = false;
        }
    }
}
