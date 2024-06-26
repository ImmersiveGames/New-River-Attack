using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class EnterSceneBehavior : Behavior
    {
        private const float InitialDelay = 2f;
        private const float MoveDuration = 5f;
        private const float DistanceOffset = 30f;
        private readonly BehaviorManager _behaviorManager;
        private BossBehavior BossBehavior { get; }
        private PlayerMaster PlayerMaster { get; }

        public EnterSceneBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(nameof(EnterSceneBehavior), subBehaviors)
        {
            _behaviorManager = behaviorManager;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;

        }

        public override async Task EnterAsync(CancellationToken token)
        {
            //await base.EnterAsync(token).ConfigureAwait(false);
            
            var tcs = new TaskCompletionSource<bool>();
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                var vector3 = BossBehavior.transform.position;
                vector3.z = PlayerMaster.transform.position.z;
                vector3.x = PlayerMaster.transform.position.x;
                BossBehavior.transform.position = vector3;
                
                var distance = PlayerMaster.transform.position.z + DistanceOffset;
                
                var mySequence = DOTween.Sequence();
                mySequence.AppendInterval(InitialDelay);
                mySequence.Append(BossBehavior.transform.DOMoveZ(distance, MoveDuration).SetEase(Ease.Linear));

                // Aguardar a conclusão da animação ou o cancelamento

                mySequence.OnComplete(() => tcs.TrySetResult(true));
                mySequence.OnKill(() => tcs.TrySetResult(false));
                mySequence.Play();
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
                    DebugManager.Log<EnterSceneBehavior>("Animation was canceled.");
                    return;
                }
            }

            if (!token.IsCancellationRequested)
            {
                DebugManager.Log<EnterSceneBehavior>("Animation completed.");
                Finalized = true;
            }
        }

        public override Task ExitAsync(CancellationToken token)
        {
            _ = _behaviorManager.ChangeBehaviorAsync(EnumNameBehavior.MoveNorthBehavior.ToString());
            return Task.CompletedTask;
        }
    }
}