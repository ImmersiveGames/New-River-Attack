using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class DeathBehavior : Behavior
    {
        private readonly GamePlayManager _gameOverManager;
        public DeathBehavior(GamePlayManager gamePlayManager, IBehavior[] subBehaviors) : base(subBehaviors)
        {
            _gameOverManager = gamePlayManager;
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            
            var tcs = new TaskCompletionSource<bool>();
            await Task.Delay(4000, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                _gameOverManager.FinisherGame();
            }).ConfigureAwait(false);
            await Task.Delay(4000, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                _gameOverManager.SendTo(GameManager.instance.gamePlayMode);
                return Task.CompletedTask;
            }).ConfigureAwait(false);
            
            // Aguardar a conclusão da animação ou o cancelamento
            await using (token.Register(() => tcs.TrySetCanceled()))
            {
                try
                {
                    await tcs.Task.ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    DebugManager.Log<DeathBehavior>("Animation was canceled.");
                    return;
                }
            }

            if (!token.IsCancellationRequested)
            {
                DebugManager.Log<DeathBehavior>("Animation completed.");
                Finalized = true;
            }
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            DebugManager.Log<DeathBehavior>("Exit EnterSceneBehavior.");
        }
    }
}