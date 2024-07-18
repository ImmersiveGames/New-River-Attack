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
    public class MoveSouthBehavior : Behavior
    {
        private const float MoveDistance = 10f;
        
        private readonly BossMovement _bossMovement;
        private readonly BehaviorManager _behaviorManager;
        private BossBehavior BossBehavior { get; }
        private PlayerMaster PlayerMaster { get; }

        public MoveSouthBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors)
            : base(subBehaviors)
        {
            _behaviorManager = behaviorManager;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
            _bossMovement = new BossMovement(BossMovement.Direction.MoveSouthBehavior, PlayerMaster.transform);
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            var animationTime = 0;
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync( () =>
            {
                var myDirection = _bossMovement.GetRelativeDirection(BossBehavior.transform.position);
                DebugManager.Log<MoveSouthBehavior>($"Direção: {myDirection}");

                if (myDirection != _bossMovement.MyDirection)
                {
                    DebugManager.Log<MoveSouthBehavior>($"Estou numa posição diferente preciso me mover");
                    var newPosition = _bossMovement.GetNewPosition(_bossMovement.MyDirection, MoveDistance);
                    BossBehavior.transform.position = newPosition;
                }
                if (BossBehavior.BossMaster.IsEmerge) return;
                BossBehavior.BossMaster.IsEmerge = true;
                animationTime = (int)BossBehavior.GetComponent<BossAnimation>().GetSubmergeTime();
                BossBehavior.BossMaster.OnEventBossEmerge();
            }).ConfigureAwait(false);
            await Task.Delay(animationTime * 1000, token).ConfigureAwait(false);
        }
        public override void UpdateAsync(CancellationToken token)
        {
            base.UpdateAsync(token);
            
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            var animationTime = 0;
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync( () =>
            {
                if (!BossBehavior.BossMaster.IsEmerge) return;
                BossBehavior.BossMaster.IsEmerge = false;
                animationTime = (int)BossBehavior.GetComponent<BossAnimation>().GetSubmergeTime();
                BossBehavior.BossMaster.OnEventBossSubmerge();
            }).ConfigureAwait(false);
            await Task.Delay(animationTime * 1000, token).ConfigureAwait(false);
        }
    }
}
