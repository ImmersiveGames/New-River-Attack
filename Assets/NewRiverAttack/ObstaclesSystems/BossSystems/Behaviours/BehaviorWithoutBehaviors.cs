using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BehaviorWithoutBehaviors : Behavior
    {
        public BehaviorWithoutBehaviors() : base("BehaviorWithoutBehaviors", null) { }

        public override Task EnterAsync(IBehavior previousBehavior, MonoBehaviour monoBehaviour, CancellationToken token)
        {
            DebugManager.Log<BehaviorWithoutBehaviors>("Entering BehaviorWithoutBehaviors");
            return Task.CompletedTask;
            // Implemente a lógica de entrada do BehaviorWithoutBehaviors
        }

        public override Task UpdateAsync(CancellationToken token)
        {
            DebugManager.Log<BehaviorWithoutBehaviors>("Updating BehaviorWithoutBehaviors");
            return Task.CompletedTask;
            // Implemente a lógica de atualização do BehaviorWithoutBehaviors
        }

        public override Task ExitAsync(IBehavior nextBehavior, CancellationToken token)
        {
            DebugManager.Log<BehaviorWithoutBehaviors>("Exiting BehaviorWithoutBehaviors");
            return Task.CompletedTask;
            // Implemente a lógica de saída do BehaviorWithoutBehaviors
        }
    }
}