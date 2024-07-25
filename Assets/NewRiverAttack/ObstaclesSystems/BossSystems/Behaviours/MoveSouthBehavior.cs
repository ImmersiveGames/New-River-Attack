using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
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
            await ChangePosition(_bossMovement, BossBehavior, MoveDistance, token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, true).ConfigureAwait(false);
        }
        public override void UpdateAsync(CancellationToken token)
        {
            base.UpdateAsync(token);
            
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, false).ConfigureAwait(false);
            await DropGas(BossBehavior).ConfigureAwait(false);
        }
    }
}
