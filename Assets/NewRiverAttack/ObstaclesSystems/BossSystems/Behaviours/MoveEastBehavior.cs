using System;
using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveEastBehavior : BossDirections
    {
        public MoveEastBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors)
            : base(behaviorManager, subBehaviors)
        {
            MoveDistance = 20f;
            BossMovement = new BossMovement(BossMovement.Direction.MoveEastBehavior, PlayerMaster.transform);
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            var random = new Random();
            Cycles = random.Next(4,6);
            await base.EnterAsync(token).ConfigureAwait(false);
        }
    }
}
