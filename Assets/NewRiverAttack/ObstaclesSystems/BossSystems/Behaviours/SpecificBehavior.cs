using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Strategies;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    /*public class SpecificBehavior : Behavior
    {
        private BossBehavior _bossBehavior;

        public SpecificBehavior(IBehavior[] subBehaviors, BossBehavior bossBehavior)
            : base("SpecificBehavior", subBehaviors,
                new DefaultChangeBehaviorStrategy(),
                new DefaultUpdateStrategy()
            )
        {
            _bossBehavior = bossBehavior;
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            DebugManager.Log<SpecificBehavior>("Specific Behavior Entered");
        }

        public override async Task UpdateAsync(CancellationToken token)
        {
            await base.UpdateAsync(token).ConfigureAwait(false);
            DebugManager.Log<SpecificBehavior>("Specific Behavior Updating");
            
            //Finalization = true;
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            DebugManager.Log<SpecificBehavior>("Specific Behavior Exited");
        }
    }*/
}