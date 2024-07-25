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
    public class MineBehavior : Behavior
    {
        private BossBehavior BossBehavior { get; }
        private readonly BossMineShoot _bossShoot;
        private PlayerMaster PlayerMaster { get; }
        private readonly BehaviorManager _behaviorManager;
        public MineBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors) : base(subBehaviors)
        {
            _behaviorManager = behaviorManager;
            BossBehavior = behaviorManager.BossBehavior;
            PlayerMaster = BossBehavior.PlayerMaster;
            _bossShoot = BossBehavior.GetComponent<BossMineShoot>();
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            await Task.Delay(100, token).ConfigureAwait(false);
            await Emerge(BossBehavior.BossMaster,token, false).ConfigureAwait(false);
            
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            { 
                _bossShoot.SetDataBullet(BossBehavior.BossMaster);
                _bossShoot.UpdateCadenceShoot();
                _bossShoot.StartShoot();
            }).ConfigureAwait(false);

        }
        public override async void UpdateAsync(CancellationToken token)
        {
            if(Finalized)
                await NextBehavior(_behaviorManager.SubBehaviorManager).ConfigureAwait(false);
            if (_bossShoot && _bossShoot.ShouldBeShoot)
            {
                Debug.Log($"Atirando: {_bossShoot.EndShoot}");
                _bossShoot.AttemptShoot(BossBehavior.BossMaster, PlayerMaster.transform);
            }

            if (_bossShoot.EndShoot)
            {
                await Task.Delay(100, token).ConfigureAwait(false);
                Finalized = true;
            }
            /*await Task.Delay(2000, token).ConfigureAwait(false);
            Debug.Log($"Update Mine Finalize: {Finalized}");
            Finalized = true;
            await NextBehavior(_behaviorManager.SubBehaviorManager).ConfigureAwait(false);*/
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            await Emerge(BossBehavior.BossMaster,token, true).ConfigureAwait(false);
            await UnityMainThreadDispatcher.EnqueueAsync(() =>
            {
                _bossShoot.StopShoot();
            }).ConfigureAwait(false);
        }
        
    }
}