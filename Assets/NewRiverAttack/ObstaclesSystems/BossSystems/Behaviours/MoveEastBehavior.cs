﻿using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MoveEastBehavior : Behavior
    {
        private const float MoveDistance = 20f;
        
        private readonly BossMovement _bossMovement;
        private readonly BehaviorManager _behaviorManager;
        private readonly BehaviorManager _subBehaviorManager;
        private BossBehavior BossBehavior { get; }
        private PlayerMaster PlayerMaster { get; }
        
        private readonly IBehavior[] _subBehaviors;
        private int _subIndex;

        public MoveEastBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors)
            : base(subBehaviors)
        {
            _behaviorManager = behaviorManager;
            _subBehaviors = subBehaviors;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
            _bossMovement = new BossMovement(BossMovement.Direction.MoveEastBehavior, PlayerMaster.transform);
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            await ChangePosition(_bossMovement, BossBehavior, MoveDistance, token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, true).ConfigureAwait(false);
        }
        public override void UpdateAsync(CancellationToken token)
        {
            base.UpdateAsync(token);
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, false).ConfigureAwait(false);
            await DropGas(BossBehavior).ConfigureAwait(false);
        }
    }
}
