using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MineBehavior : Behavior
    {
        private BossBehavior BossBehavior { get; }
        private readonly BossMineShoot _bossMineShoot;
        private readonly object[] _dataShoot;
        private PlayerMaster PlayerMaster { get; }
        
        // Parametro 0 = Numero de Minas
        // Parametro 1 = cadencia
        public MineBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base( subBehaviors)
        {
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
            _bossMineShoot = BossBehavior.GetComponent<BossMineShoot>();
            _dataShoot = data;
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            Finalized = false;
            await Task.Delay(100, token).ConfigureAwait(false);
            
            await Emerge(BossBehavior.BossMaster,token, false).ConfigureAwait(false);
            DebugManager.Log<MineBehavior>($"Enter {GetType().Name}. Finalize: {Finalized}");
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            { 
                var numMines = (int)(_dataShoot[0] ?? 10);
                var cadence = (float)(_dataShoot[1] ?? 0.8f);
                
                if (_bossMineShoot == null) return;
                _bossMineShoot.SetShoots(numMines, cadence);
                _bossMineShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossMineShoot.UpdateCadenceShoot();
                _bossMineShoot.StartShoot();
            }).ConfigureAwait(false);

        }
        public override void UpdateAsync(CancellationToken token)
        {
            if (!_bossMineShoot || !_bossMineShoot.ShouldBeShoot || Finalized) return;
            _bossMineShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
            base.UpdateAsync(token);
            if(_bossMineShoot.timesRepeat <= 0)
                Finalized = true;
            
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            await Task.Delay(100, token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, true).ConfigureAwait(false);
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                _bossMineShoot.StopShoot();
            }).ConfigureAwait(false);
        }
        
    }
}