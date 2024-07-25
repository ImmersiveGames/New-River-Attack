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
        private readonly BehaviorManager _behaviorManager;
        private PlayerMaster PlayerMaster { get; }
        
        // Parametro 0 = Numero de misseis
        // Parametro 1 = angulo de misseis
        // Parametro 1 = cycles
        public MissileBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base(subBehaviors, string.Join("_", data))
        {
            _behaviorManager = behaviorManager;
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
                if (_bossMissileShoot == null) return;
                _bossMissileShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossMissileShoot.UpdateCadenceShoot();
                _bossMissileShoot.StartShoot();
            }).ConfigureAwait(false);
        }
        
        public override async void UpdateAsync(CancellationToken token)
        {
            base.UpdateAsync(token);
            if (!_bossMissileShoot || !_bossMissileShoot.ShouldBeShoot) return;
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(async () =>
            {
                if (_bossMissileShoot && _bossMissileShoot.ShouldBeShoot)
                {
                    _bossMissileShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
                }
                await Task.Delay(100, token).ConfigureAwait(false);
            }).ConfigureAwait(false);
            
            /*Debug.Log($"EndCycle: {_bossMissileShoot.EndCycle},{_dataShoot[0]}, {_dataShoot[1]}, {_dataShoot[2]}");
            
            if (!_bossMissileShoot.EndCycle) return;
            await Task.Delay(100, token).ConfigureAwait(false);
            Finalized = true;
            await NextBehavior(_behaviorManager.SubBehaviorManager).ConfigureAwait(false);*/
            
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            await base.ExitAsync(token).ConfigureAwait(false);
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                _bossShoot.StopShoot();
            }).ConfigureAwait(false);
        }

        
        
    }
}