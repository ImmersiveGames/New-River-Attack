using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.Utils;
using NewRiverAttack.PlayerManagers.PlayerSystems;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MissileBehavior : Behavior
    {
        private BossBehavior BossBehavior { get; }
        private readonly object[] _dataShoot;
        private readonly BossMissileShootOld _bossMissileShootOld;
        private PlayerMaster PlayerMaster { get; }
        
        // Parametro 0 = Numero de misseis
        // Parametro 1 = angulo de misseis
        // Parametro 2 = cadência dos misseis
        // Parametro 3 = cycles
        public MissileBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base( subBehaviors, string.Join("_", data))
        {
            BossBehavior = behaviorManager.BossBehavior;
            //PlayerMaster = BossBehavior.PlayerMaster;
            _bossMissileShootOld = BossBehavior.GetComponent<BossMissileShootOld>();
            _dataShoot = data;
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            Finalized = false;
            await Task.Delay(100, token).ConfigureAwait(false);
            await MainThreadDispatcher.EnqueueAsync(() =>
            {
                var numMissiles = (int)(_dataShoot[0] ?? 5);
                var angleCones = (float)(_dataShoot[1] ?? 90f);
                var cadence = (float)(_dataShoot[2] ?? 1f);
                var numCycles = (int)(_dataShoot[3] ?? 3);

                if (_bossMissileShootOld == null) return;
                _bossMissileShootOld.SetShoots(numMissiles, angleCones, cadence, numCycles);
                //_bossMissileShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossMissileShootOld.UpdateCadenceShoot();
                _bossMissileShootOld.StartShoot();
            }).ConfigureAwait(false);
        }
        
        public override void UpdateAsync(CancellationToken token)
        {
            if (!_bossMissileShootOld || !_bossMissileShootOld.ShouldBeShoot || Finalized) return;
            //_bossMissileShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
            base.UpdateAsync(token);
            if(_bossMissileShootOld.timesRepeat <= 0)
                Finalized = true;

        }
        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            await Task.Delay(100, token).ConfigureAwait(false);
            await MainThreadDispatcher.EnqueueAsync(() =>
            {
                _bossMissileShootOld.StopShoot();
            }).ConfigureAwait(false);
        }
    }
}