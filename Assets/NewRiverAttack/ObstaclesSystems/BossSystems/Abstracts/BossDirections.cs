using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts
{
    public abstract class BossDirections : Behavior
    {
        protected float MoveDistance = 25f;
        
        private readonly BehaviorManager _behaviorManager;

        protected BossMovement BossMovement;
        private BossBehavior BossBehavior { get; set; }
        protected PlayerMaster PlayerMaster { get; set; }

        protected BossDirections(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(subBehaviors)
        {
            _behaviorManager = behaviorManager;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            ChangeBehavior = false;
            ChangeSubBehavior = false;
            await base.EnterAsync(token).ConfigureAwait(false);
            await ChangePosition(BossMovement, BossBehavior, MoveDistance, token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, true).ConfigureAwait(false);
        }
        public override async void UpdateAsync(CancellationToken token)
        {
            base.UpdateAsync(token);
            await ChangeRandomSubBehavior().ConfigureAwait(false);
            await Task.Delay(100, token).ConfigureAwait(false);
            await ChangeRandomBehavior().ConfigureAwait(false);
        }

        public override async Task ExitAsync(CancellationToken token)
        {
            ChangeBehavior = false;
            ChangeSubBehavior = false;
            await base.ExitAsync(token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, false).ConfigureAwait(false);
            await DropGas(BossBehavior).ConfigureAwait(false);
        }

        private async Task ChangeRandomSubBehavior()
        {
            if(_behaviorManager.SubBehaviorManager == null) return;
            await Task.Delay(100).ConfigureAwait(false);
            DebugManager.Log<BossDirections>($"Name: {_behaviorManager.SubBehaviorManager.CurrentBehavior.Name}, Finalize: {_behaviorManager.SubBehaviorManager.CurrentBehavior.Finalized}");
            if (_behaviorManager.SubBehaviorManager.CurrentBehavior.Finalized && !ChangeSubBehavior)
            {
                ChangeSubBehavior = true;
                var otherSubBehaviorNames = _behaviorManager.SubBehaviorManager.GetBehaviorsNames(_behaviorManager.SubBehaviorManager.CurrentBehavior.Name);
                var subBehaviorName = otherSubBehaviorNames.Length switch
                {
                    > 1 => GetRandomBehavior(otherSubBehaviorNames),
                    1 => otherSubBehaviorNames[0],
                    _ => _behaviorManager.SubBehaviorManager.CurrentBehavior.Name
                };
                DebugManager.Log<BossDirections>($"Mudando Para: {subBehaviorName}");
                await _behaviorManager.SubBehaviorManager.ChangeBehaviorAsync(subBehaviorName).ConfigureAwait(false);
                ChangeSubBehavior = false;
                Cycles--;
            }
        }

        private async Task ChangeRandomBehavior()
        {
            if(_behaviorManager.SubBehaviorManager.CurrentBehavior.Finalized && !ChangeBehavior)
            {
                //DebugManager.Log<BossDirections>($"Sub Finalizado Novo ciclo: {Cycles}");
                if (Cycles <= 0)
                {
                    ChangeBehavior = true;
                    // Ja ultrapassou o numero de ciclos então precisa mudar o comportamento principal.
                    var otherBehaviorNames = _behaviorManager.GetBehaviorsNames(_behaviorManager.CurrentBehavior.Name);
                    var behaviorName = otherBehaviorNames.Length switch
                    {
                        > 1 => GetRandomBehavior(otherBehaviorNames),
                        1 => otherBehaviorNames[0],
                        _ => _behaviorManager.CurrentBehavior.Name
                    };
                    if (otherBehaviorNames.Length > 1)
                    {
                        behaviorName = GetRandomBehavior(otherBehaviorNames); 
                    }
                    
                    DebugManager.Log<BossDirections>($"MUDAR PARA: {behaviorName}");
                    await _behaviorManager.ChangeBehaviorAsync(behaviorName).ConfigureAwait(false);
                }
            }
        }
    }
}