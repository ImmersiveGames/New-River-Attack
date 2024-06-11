using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveNorthBehavior : Behavior
    {
        public MoveNorthBehavior() : base(nameof(MoveNorthBehavior), null)
        {
        }
        public override Task EnterAsync(IBehavior previousBehavior, MonoBehaviour monoBehaviour, CancellationToken token)
        {
            return Task.CompletedTask;
            // Implemente a lógica de entrada do SubBehavior1
        }

        public override Task UpdateAsync(CancellationToken token)
        {
            return Task.CompletedTask;
            // Implemente a lógica de atualização do SubBehavior1
        }

        public override Task ExitAsync(IBehavior nextBehavior, CancellationToken token)
        {
            return Task.CompletedTask;
            // Implemente a lógica de saída do SubBehavior1
        }
    }
}