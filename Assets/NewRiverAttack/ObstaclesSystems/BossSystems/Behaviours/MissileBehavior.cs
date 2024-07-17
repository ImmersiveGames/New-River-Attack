using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MissileBehavior : Behavior
    {
        private BossBehavior BossBehavior { get; }
        private readonly BossShoot _bossShoot;
        private readonly object[] _dataShoot;
        private BossMissileShoot _bossMissileShoot;
        private static string identifier;
        private PlayerMaster PlayerMaster { get; }
        
        // Parametro 0 = Numero de misseis
        // Parametro 1 = angulo de misseis
        // Parametro 1 = cycles
        public MissileBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base(subBehaviors, string.Join("_", data))
        {
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
            _bossShoot = BossBehavior.GetComponent<BossMissileShoot>();
            _dataShoot = data;
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                _bossMissileShoot= _bossShoot as BossMissileShoot;
                var numMissiles = (int)(_dataShoot[0] ?? 5);
                var angleCones = (float)(_dataShoot[1] ?? 90f);
                var numCycles = (int)(_dataShoot[2] ?? 3);
                if (_bossMissileShoot != null) 
                    _bossMissileShoot.SetMissiles(numMissiles, angleCones,numCycles);
                _bossShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossShoot.UpdateCadenceShoot();
                _bossShoot.StartShoot();
            }).ConfigureAwait(false);
            
            Initialized = true;

        }
        
        public override async Task UpdateAsync(CancellationToken token)
        {
            await base.UpdateAsync(token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                if (!_bossShoot || !_bossShoot.ShouldBeShoot) return;
                _bossShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
                if (!_bossMissileShoot.EndCycle) return;
                Finalized = true;
            }).ConfigureAwait(false);
            
        }
        public override Task ExitAsync(CancellationToken token)
        {
            _bossShoot.StopShoot();
            return base.ExitAsync(token);
        }
        
    }
}