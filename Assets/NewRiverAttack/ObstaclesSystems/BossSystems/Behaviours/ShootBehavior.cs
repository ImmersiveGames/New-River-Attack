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
    public abstract class ShootBehavior : Behavior
    {
        private BossBehavior BossBehavior { get; }
        private readonly BossShoot _bossShoot;
        private PlayerMaster PlayerMaster { get; }

        protected ShootBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(subBehaviors, "1")
        {
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = behaviorManager.BossBehavior.PlayerMaster;
            _bossShoot = BossBehavior.GetComponent<BossCleanShoot>();
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            await base.EnterAsync(token).ConfigureAwait(false);
            
            await Task.Delay(100, token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                _bossShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossShoot.UpdateCadenceShoot();
                Debug.Log(_bossShoot);
                _bossShoot.StartShoot();
            }).ConfigureAwait(false);
            
            Initialized = true;

        }
        
        public override async Task UpdateAsync(CancellationToken token)
        {
            await base.UpdateAsync(token).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                if (_bossShoot && _bossShoot.ShouldBeShoot)
                {
                    _bossShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
                }
            }).ConfigureAwait(false);
            await Task.Delay(100, token).ConfigureAwait(false);
            
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            _bossShoot.StopShoot();
            await base.ExitAsync(token).ConfigureAwait(false);

        }
    }
}