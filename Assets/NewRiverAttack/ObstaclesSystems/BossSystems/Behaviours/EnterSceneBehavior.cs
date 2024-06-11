using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class EnterSceneBehavior : Behavior
    {
        private BossBehavior _bossBehavior;
        public EnterSceneBehavior() : base(nameof(EnterSceneBehavior), null)
        {
        }
        public override Task EnterAsync(IBehavior previousBehavior, MonoBehaviour monoBehaviour, CancellationToken token)
        {
            DebugManager.Log<Behavior>("Entering EnterSceneBehavior");

            if (monoBehaviour is not BossBehavior bossBehavior)
            {
                DebugManager.LogError<Behavior>("MonoBehavior não autorizado");
                return Task.CompletedTask;
            }
            //Ajustando a Posição inicial.
            var vector3 = bossBehavior.transform.position;
            vector3.z = bossBehavior.PlayerMaster.transform.position.z;
            vector3.x = bossBehavior.PlayerMaster.transform.position.x;
            bossBehavior.transform.position = vector3;
            
            var distance = bossBehavior.PlayerMaster.transform.position.z + 45;
            var mySequence = DOTween.Sequence();
            mySequence.Append(bossBehavior.transform.DOMoveZ(distance, 7f).SetEase(Ease.Linear));
            mySequence.Play();
            var boss = bossBehavior.BossMaster;
            boss.OnEventBossShowUp();
            return Task.CompletedTask;
            // Implemente a lógica de entrada do BehaviorWithoutBehaviors
        }

        public override Task UpdateAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>("Updating EnterSceneBehavior");
            return Task.CompletedTask;
            // Implemente a lógica de atualização do BehaviorWithoutBehaviors
        }

        public override Task ExitAsync(IBehavior nextBehavior, CancellationToken token)
        {
            DebugManager.Log<Behavior>("Exiting EnterSceneBehavior");
            return Task.CompletedTask;
            // Implemente a lógica de saída do BehaviorWithoutBehaviors
        }
    }
}