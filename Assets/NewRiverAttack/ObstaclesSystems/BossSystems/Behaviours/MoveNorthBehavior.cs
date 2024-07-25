using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.AreaEffectSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveNorthBehavior : Behavior
    {
        private const float MoveDistance = 25f;

        private readonly BossMovement _bossMovement;
        private readonly BehaviorManager _behaviorManager;
        private BehaviorManager _subBehaviorManager;
        private BossBehavior BossBehavior { get; }
        private PlayerMaster PlayerMaster { get; }
        private readonly IBehavior[] _subBehaviors;
        private int _subIndex;

        public MoveNorthBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(subBehaviors)
        {
            _behaviorManager = behaviorManager;
            _subBehaviors = subBehaviors;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
            _bossMovement = new BossMovement(BossMovement.Direction.MoveNorthBehavior, PlayerMaster.transform);
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            await ChangePosition(_bossMovement, BossBehavior, MoveDistance, token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, true).ConfigureAwait(false);
        }

        public override async void UpdateAsync(CancellationToken token)
        {
            base.UpdateAsync(token);
            await UnityMainThreadDispatcher.EnqueueAsync(async () =>
            {
            if (Input.GetKey(KeyCode.L))
            {
                await _behaviorManager.ChangeBehaviorAsync("MoveEastBehavior").ConfigureAwait(false);
            }
            }).ConfigureAwait(false);
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, false).ConfigureAwait(false);
            await DropGas(BossBehavior).ConfigureAwait(false);
        }
        
    }
}