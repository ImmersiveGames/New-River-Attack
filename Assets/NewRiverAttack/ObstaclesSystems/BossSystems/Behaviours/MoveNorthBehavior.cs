using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveNorthBehavior : Behavior
    {
        //private const float InitialDelay = 2f;
        private const float MoveDuration = 5f;
        private const float DistanceOffset = 30f;
        private const float MinimumDistance = 10f;
        
        
        private readonly BehaviorManager _behaviorManager;
        private BossBehavior BossBehavior { get; }
        private PlayerMaster PlayerMaster { get; }

        public MoveNorthBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(
            nameof(MoveNorthBehavior), subBehaviors)
        {
            _behaviorManager = behaviorManager;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            _behaviorManager.CurrentIndex = 0;
            await base.EnterAsync(token).ConfigureAwait(false);
            
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(async () =>
            {
               
            });

            Initialized = true;

        }

        public override async Task ExitAsync(CancellationToken token)
        {
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                var newBehavior = GetRandomNextBehavior(EnumDirectionBehavior.MoveNorthBehavior, PlayerMaster.transform.position);
                DebugManager.Log<Behavior>($"Sort New Behavior {Name}");
                _ = _behaviorManager.ChangeBehaviorAsync(newBehavior);
            }).ConfigureAwait(false);
            await Task.Delay(100, token).ConfigureAwait(false);
            await base.ExitAsync(token).ConfigureAwait(false);
        }

        
    }
}
