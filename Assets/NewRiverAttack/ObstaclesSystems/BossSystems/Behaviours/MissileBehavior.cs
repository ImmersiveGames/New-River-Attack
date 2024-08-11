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
        private readonly BossMissileShoot _bossMissileShoot;
        private readonly BehaviorManager _behaviorManager;
        private PlayerMaster PlayerMaster { get; }
        
        // Parametro 0 = Numero de misseis
        // Parametro 1 = angulo de misseis
        // Parametro 2 = cadência dos misseis
        // Parametro 3 = cycles
        public MissileBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base( subBehaviors, string.Join("_", data))
        {
            _behaviorManager = behaviorManager;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
            _bossMissileShoot = BossBehavior.GetComponent<BossMissileShoot>();
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

                if (_bossMissileShoot == null) return;
                _bossMissileShoot.SetShoots(numMissiles, angleCones, cadence, numCycles);
                _bossMissileShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossMissileShoot.UpdateCadenceShoot();
                _bossMissileShoot.StartShoot();
            }).ConfigureAwait(false);
        }
        
        public override void UpdateAsync(CancellationToken token)
        {
            if (!_bossMissileShoot || !_bossMissileShoot.ShouldBeShoot || Finalized) return;
            _bossMissileShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
            base.UpdateAsync(token);
            if(_bossMissileShoot.timesRepeat <= 0)
                Finalized = true;

        }
        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            await Task.Delay(100, token).ConfigureAwait(false);
            await MainThreadDispatcher.EnqueueAsync(() =>
            {
                _bossMissileShoot.StopShoot();
            }).ConfigureAwait(false);
        }
    }
}