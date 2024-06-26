﻿using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems.Strategies;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveNorthBehavior : Behavior
    {
        private const float DistanceThreshold = 31f; // Exemplo de distância máxima permitida
        private readonly BossBehavior _bossBehavior;

        public bool StopUpdate = false;

        public MoveNorthBehavior(IBehavior[] subBehaviors, BossBehavior bossBehavior)
            : base(EnumNameBehavior.MoveNorthBehavior.ToString(), subBehaviors,
                new DefaultChangeBehaviorStrategy(),
                new DefaultUpdateStrategy())
        {
            _bossBehavior = bossBehavior;
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<MoveNorthBehavior>("Enter MoveNorthBehavior");
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                // Verificar se o Boss está ao norte do Player
                if (_bossBehavior.transform.position.z <= _bossBehavior.PlayerMaster.transform.position.z)
                {
                    DebugManager.Log<MoveNorthBehavior>("Boss is not north of the player. Exiting...");
                    Finalized = true;
                    return Task.CompletedTask;
                }
                Initialized = true;
                return Task.CompletedTask;
            }).ConfigureAwait(false);
        }

        public override async Task UpdateAsync(CancellationToken token)
        {
            if (IsPaused || token.IsCancellationRequested || Finalized || StopUpdate) return;

            DebugManager.Log<MoveNorthBehavior>("Updating MoveNorthBehavior");

            // Verificar a distância do jogador
            var distance = Vector3.Distance(_bossBehavior.transform.position, _bossBehavior.PlayerMaster.transform.position);
            //DebugManager.Log<MoveNorthBehavior>($"Distance: {distance}");
            
            if (distance > DistanceThreshold)
            {
                StopUpdate = true;
                DebugManager.Log<MoveNorthBehavior>("Player is too far. Finalizing...");
                await ExitAsync(token).ConfigureAwait(false);
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
            DebugManager.Log<MoveNorthBehavior>("Exit MoveNorthBehavior");

            // Chamar FinalizeAsync para garantir que todos os recursos sejam liberados
            await FinalizeAllSubBehavior(token).ConfigureAwait(false);
            
            DebugManager.Log<MoveNorthBehavior>("Sorteia e Chama aqui");
            await _bossBehavior.GetBehaviorManager.ChangeBehaviorAsync("MoveSouthBehavior").ConfigureAwait(false);
        }
    }
}
