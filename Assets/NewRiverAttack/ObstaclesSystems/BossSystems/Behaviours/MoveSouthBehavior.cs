using System;
using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveSouthBehavior : BossDirections
    {

        public MoveSouthBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors)
            : base(behaviorManager,subBehaviors)
        {
            MoveDistance = 10f;
            BossMovement = new BossMovement(BossMovement.Direction.MoveSouthBehavior, PlayerMaster.transform);
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            var random = new Random();
            Cycles = random.Next(2,4);
            await base.EnterAsync(token).ConfigureAwait(false);
        }
    }
}
