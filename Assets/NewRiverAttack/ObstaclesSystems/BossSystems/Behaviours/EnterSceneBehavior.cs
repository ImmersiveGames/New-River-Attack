using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems.Strategies;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class EnterSceneBehavior : Behavior
    {
        private const float InitialDelay = 2f;
        private const float MoveDuration = 5f;
        private const float DistanceOffset = 30f;
        private readonly BossBehavior _bossBehavior;

        public EnterSceneBehavior(IBehavior[] subBehaviors, BossBehavior bossBehavior)
            : base(EnumNameBehavior.EnterSceneBehavior.ToString(), subBehaviors,
                new DefaultChangeBehaviorStrategy(),
                new DefaultUpdateStrategy()
            )
        {
            _bossBehavior = bossBehavior;
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<EnterSceneBehavior>("Enter EnterSceneBehavior");
            var tcs = new TaskCompletionSource<bool>();

            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                // Ajustando a Posição inicial.
                var vector3 = _bossBehavior.transform.position;
                vector3.z = _bossBehavior.PlayerMaster.transform.position.z;
                vector3.x = _bossBehavior.PlayerMaster.transform.position.x;
                _bossBehavior.transform.position = vector3;
                var distance = _bossBehavior.PlayerMaster.transform.position.z + DistanceOffset;

                var mySequence = DOTween.Sequence();
                mySequence.AppendInterval(InitialDelay);
                mySequence.Append(_bossBehavior.transform.DOMoveZ(distance, MoveDuration).SetEase(Ease.Linear));

                // Aguardar a conclusão da animação ou o cancelamento

                mySequence.OnComplete(() => tcs.TrySetResult(true));
                mySequence.OnKill(() => tcs.TrySetResult(false));
                mySequence.Play();
                
                return Task.CompletedTask;
            }).ConfigureAwait(false);

            // Aguardar a conclusão da animação ou o cancelamento
            using (token.Register(() => tcs.TrySetCanceled()))
            {
                try
                {
                    await tcs.Task.ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    DebugManager.Log<EnterSceneBehavior>("Animation was canceled.");
                    return;
                }
            }

            if (!token.IsCancellationRequested)
            {
                DebugManager.Log<EnterSceneBehavior>("Animation completed.");
            }
            Initialized = true;
            Finalized = true;
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;
            Initialized = false;
            NextBehavior = _bossBehavior.GetBehaviorManager.GetBehavior("MoveNorthBehavior");
            DebugManager.Log<EnterSceneBehavior>("Exit EnterSceneBehavior");
            await base.ExitAsync(token).ConfigureAwait(false);
            await _bossBehavior.GetBehaviorManager.ChangeBehaviorAsync(NextBehavior.Name).ConfigureAwait(false);
        }
    }
}