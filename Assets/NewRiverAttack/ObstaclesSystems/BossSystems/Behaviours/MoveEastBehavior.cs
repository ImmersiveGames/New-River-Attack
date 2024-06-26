using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems.Strategies;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveEastBehavior : Behavior
    {
        private const float DistanceThreshold = 31f; // Exemplo de distância máxima permitida
        private readonly BossBehavior _bossBehavior;

        public MoveEastBehavior(IBehavior[] subBehaviors, BossBehavior bossBehavior)
            : base(EnumNameBehavior.MoveEastBehavior.ToString(), subBehaviors,
                new DefaultChangeBehaviorStrategy(),
                new DefaultUpdateStrategy())
        {
            _bossBehavior = bossBehavior;
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<MoveEastBehavior>("Enter MoveEastBehavior");
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                // Verificar se o Boss está ao norte do Player
                if (_bossBehavior.transform.position.z <= _bossBehavior.PlayerMaster.transform.position.z)
                {
                    DebugManager.Log<MoveEastBehavior>("Boss is not east of the player. Exiting...");
                    Finalized = true;
                    return Task.CompletedTask;
                }
                Initialized = true;
                return Task.CompletedTask;
            }).ConfigureAwait(false);
        }

        public override async Task UpdateAsync(CancellationToken token)
        {
            if (IsPaused || token.IsCancellationRequested || Finalized) return;

            DebugManager.Log<MoveEastBehavior>("Updating MoveEastBehavior");

            // Verificar a distância do jogador
            var distance = Vector3.Distance(_bossBehavior.transform.position, _bossBehavior.PlayerMaster.transform.position);
            DebugManager.Log<MoveEastBehavior>($"Distance: {distance}");
            
            if (distance > DistanceThreshold)
            {
                DebugManager.Log<MoveEastBehavior>("Player is too far. Finalizing...");
                Finalized = true;
                await FinalizeAllSubBehavior(token).ConfigureAwait(false);
                return;
            }

            // Atualizar sub comportamentos, se houver
            if (SubBehaviors is { Length: > 0 })
            {
                var currentSubBehavior = SubBehaviors[CurrentSubBehaviorIndex];

                if (!currentSubBehavior.Finalized)
                {
                    await currentSubBehavior.UpdateAsync(token).ConfigureAwait(false);
                }
            }
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested || Finalized) return;

            DebugManager.Log<MoveNorthBehavior>("Exit MoveEastBehavior");

            // Chamar FinalizeAsync para garantir que todos os recursos sejam liberados
            //await FinalizeAsync(token).ConfigureAwait(false);

            // Marcar como finalizado para evitar múltiplas chamadas
            Finalized = true;
            
            DebugManager.Log<MoveNorthBehavior>("Sorteia e Chama aqui");
        }
    }
}
