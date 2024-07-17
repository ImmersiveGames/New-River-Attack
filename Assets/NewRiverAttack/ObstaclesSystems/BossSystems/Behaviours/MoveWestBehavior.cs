using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveWestBehavior : Behavior
    {
        private const float MoveDistance = 20f;
        
        private readonly BossMovement _bossMovement;
        private readonly BehaviorManager _behaviorManager;
        private BossBehavior BossBehavior { get; }
        private PlayerMaster PlayerMaster { get; }

        public MoveWestBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors)
            : base(subBehaviors)
        {
            _behaviorManager = behaviorManager;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
            _bossMovement = new BossMovement(BossMovement.Direction.MoveWestBehavior, PlayerMaster.transform);
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            _behaviorManager.CurrentIndex = 0;
            await base.EnterAsync(token).ConfigureAwait(false);
            
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                var myDirection = _bossMovement.GetRelativeDirection(BossBehavior.transform.position);
                DebugManager.Log<Behavior>($"Estou na direção {myDirection} em relação ao player");
                if (myDirection != _bossMovement.MyDirection)
                {
                    DebugManager.LogWarning<Behavior>($"Estou numa posição diferente preciso me mover");
                    var newPosition = _bossMovement.GetNewPosition(_bossMovement.MyDirection, MoveDistance);
                    BossBehavior.transform.position = newPosition;
                    BossBehavior.BossMaster.OnEventBossEmerge();
                }

                return Task.FromResult<Task>(null);
            }).ConfigureAwait(false);

            Initialized = true;
            BossBehavior.BossMaster.OnEventBossEmerge();

        }

        public override async Task ExitAsync(CancellationToken token)
        {
            var animationTime = 0f;
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                animationTime = BossBehavior.GetComponent<BossAnimation>().GetSubmergeTime();
                BossBehavior.BossMaster.OnEventBossSubmerge();
            }).ConfigureAwait(false);
            await Task.Delay((int)animationTime * 1000, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                var newBehavior = _bossMovement.GetRandomDirectionSelfExclude();
                DebugManager.Log<Behavior>($"Sort New Behavior {Name}");
                _ = _behaviorManager.ChangeBehaviorAsync(newBehavior.ToString());
            }).ConfigureAwait(false);
            
            await base.ExitAsync(token).ConfigureAwait(false);
            
        }
    }
}
