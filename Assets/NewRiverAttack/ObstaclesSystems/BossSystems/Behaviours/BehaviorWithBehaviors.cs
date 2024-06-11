using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class SubBehavior1 : Behavior
    {
        public SubBehavior1() : base("SubBehavior1", null) { }

        public override Task EnterAsync(IBehavior previousBehavior, MonoBehaviour monoBehaviour, CancellationToken token)
        {
            // Implemente a lógica de entrada do SubBehavior1
            DebugManager.Log<Behavior>("Entering SubBehavior1");
            return Task.CompletedTask;
        }

        public override Task UpdateAsync(CancellationToken token)
        {
            // Implemente a lógica de atualização do SubBehavior1
            DebugManager.Log<Behavior>("Updating SubBehavior1");
            return Task.CompletedTask;
        }

        public override Task ExitAsync(IBehavior nextBehavior, CancellationToken token)
        {
            // Implemente a lógica de saída do SubBehavior1
            DebugManager.Log<Behavior>("Exiting SubBehavior1");
            return Task.CompletedTask;
        }
    }

    public class SubBehavior2 : Behavior
    {
        public SubBehavior2() : base("SubBehavior2", null) { }

        public override Task EnterAsync(IBehavior previousBehavior, MonoBehaviour monoBehaviour ,CancellationToken token)
        {
            // Implemente a lógica de entrada do SubBehavior2
            DebugManager.Log<Behavior>("Entering SubBehavior2");
            return Task.CompletedTask;
        }

        public override Task UpdateAsync(CancellationToken token)
        {
            // Implemente a lógica de atualização do SubBehavior2
            DebugManager.Log<Behavior>("Updating SubBehavior2");
            return Task.CompletedTask;
        }

        public override Task ExitAsync(IBehavior nextBehavior, CancellationToken token)
        {
            // Implemente a lógica de saída do SubBehavior2
            DebugManager.Log<Behavior>("Exiting SubBehavior2");
            return Task.CompletedTask;
        }
    }

    public class BehaviorWithBehaviors : Behavior
    {
        public BehaviorWithBehaviors() : base("BehaviorWithBehaviors", new IBehavior[] { new SubBehavior1(), new SubBehavior2() }) { }

        public override Task EnterAsync(IBehavior previousBehavior, MonoBehaviour monoBehaviour, CancellationToken token)
        {
            DebugManager.Log<Behavior>("Entering BehaviorWithBehaviors");
            return Task.CompletedTask;
            // Implemente a lógica de entrada do BehaviorWithBehaviors
        }

        public override Task UpdateAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>("Updating BehaviorWithBehaviors");
            return Task.CompletedTask;
            // Implemente a lógica de atualização do BehaviorWithBehaviors
        }

        public override Task ExitAsync(IBehavior nextBehavior, CancellationToken token)
        {
            DebugManager.Log<Behavior>("Exiting BehaviorWithBehaviors");
            return Task.CompletedTask;
            // Implemente a lógica de saída do BehaviorWithBehaviors
        }
    }
}
