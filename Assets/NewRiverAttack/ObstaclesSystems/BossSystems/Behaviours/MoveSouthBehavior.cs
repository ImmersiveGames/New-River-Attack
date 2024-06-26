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
    public class MoveSouthBehavior : Behavior
    {
        private const float DistanceThreshold = 35f; // Exemplo de distância máxima permitida
        private readonly BossBehavior _bossBehavior;

        public MoveSouthBehavior(IBehavior[] subBehaviors, BossBehavior bossBehavior)
            : base(EnumNameBehavior.MoveSouthBehavior.ToString(), subBehaviors,
                new DefaultChangeBehaviorStrategy(),
                new DefaultUpdateStrategy())
        {
            _bossBehavior = bossBehavior;
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<MoveSouthBehavior>("Enter MoveSouthBehavior");
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

            DebugManager.Log<MoveSouthBehavior>("Updating MoveSouthBehavior");

            // Verificar a distância do jogador
            var distance = Vector3.Distance(_bossBehavior.transform.position, _bossBehavior.PlayerMaster.transform.position);
            DebugManager.Log<MoveSouthBehavior>($"Distance: {distance}");
            
            if (distance > DistanceThreshold)
            {
                DebugManager.Log<MoveSouthBehavior>("Player is too far. Finalizing...");
                Finalized = true;
                //await FinalizeAsync(token).ConfigureAwait(false);
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

            DebugManager.Log<MoveSouthBehavior>("Exit MoveSouthBehavior");

            // Chamar FinalizeAsync para garantir que todos os recursos sejam liberados
            //await FinalizeAsync(token).ConfigureAwait(false);

            // Marcar como finalizado para evitar múltiplas chamadas
            Finalized = true;
            
            DebugManager.Log<MoveSouthBehavior>("Sorteia e Chama aqui");
        }
    }
}
