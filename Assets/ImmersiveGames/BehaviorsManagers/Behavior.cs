using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.AreaEffectSystems;
using NewRiverAttack.ObstaclesSystems.BossSystems;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using UnityEngine;
using Random = System.Random;

namespace ImmersiveGames.BehaviorsManagers
{
    public abstract class Behavior : IBehavior
    {
        
        private const int DebugDelay = 100;

        protected int Cycles;

        protected bool ChangeBehavior;
        protected bool ChangeSubBehavior;

        protected Behavior(IBehavior[] subBehaviors, string identifier = "")
        {
            Name = GetType().Name.Trim();
            if (identifier != "")
            {
                Name += "_" + identifier;
            }
            SubBehaviors = subBehaviors;
        }

        public string Name { get; }
        public IBehavior[] SubBehaviors { get; }

        public bool Initialized { get; set; }
        public bool Finalized { get; set; }

        #region State Machine

        public virtual async Task EnterAsync(CancellationToken token)
        {
            await Task.Delay(DebugDelay, token).ConfigureAwait(false);
            DebugManager.Log<Behavior>($"Enter {GetType().Name}. Finalize: {Finalized}");
        }

        public virtual async void UpdateAsync(CancellationToken token)
        {
            await Task.Delay(DebugDelay, token).ConfigureAwait(false);
            DebugManager.Log<Behavior>($"Update {GetType().Name}. Finalize: {Finalized}");
        }

        public virtual async Task ExitAsync(CancellationToken token)
        {
            await Task.Delay(DebugDelay, token).ConfigureAwait(false);
            DebugManager.Log<Behavior>($"Exit {GetType().Name}. Finalize: {Finalized}");
        }

        #endregion
       

        #region Funções Auxiliatres

        protected string GetRandomBehavior(string[] nameBehaviors)
        {
            if (nameBehaviors.Length <= 0) return nameBehaviors[0];
            var random = new Random();
            var indexRandom = random.Next(nameBehaviors.Length);
            return nameBehaviors[indexRandom];

        }
        protected static async Task Emerge( BossMaster bossMaster, CancellationToken token, bool emerge)
        {
            var animationTime = 0;
            await Task.Delay(100, token).ConfigureAwait(false);
            await MainThreadDispatcher.EnqueueAsync( () =>
            {
                if (bossMaster.IsEmerge == emerge) return;
                bossMaster.IsEmerge = emerge;
                animationTime = (int)bossMaster.GetComponent<BossAnimation>().GetSubmergeTime();
                if(emerge)
                {
                    bossMaster.OnEventBossEmerge();
                }
                else
                {
                    bossMaster.OnEventBossSubmerge();
                }

            }).ConfigureAwait(false);
            await Task.Delay(animationTime * 1000, token).ConfigureAwait(false);
        }

        protected async Task ChangePosition(BossMovement bossMovement, Component obstacleTransform, float distance, CancellationToken token)
        {
            await Task.Delay(100, token).ConfigureAwait(false);
            await MainThreadDispatcher.EnqueueAsync( () =>
            {
                var myDirection = bossMovement.GetRelativeDirection(obstacleTransform.transform.position);
                DebugManager.Log<Behavior>($"Direção: {myDirection}");

                if (myDirection == bossMovement.MyDirection) return;
                DebugManager.Log<Behavior>($"Estou numa posição diferente preciso me mover");
                var newPosition = bossMovement.GetNewPosition(bossMovement.MyDirection, distance);
                obstacleTransform.transform.position = newPosition;
            }).ConfigureAwait(false);
        }
        protected static async Task DropGas(BossBehavior bossBehavior)
        {
            await MainThreadDispatcher.EnqueueAsync( () =>
            {
                /*if (bossBehavior.BossMaster.IsDead) return;
                var gasPosition = bossBehavior.transform.position;
                var newObj = Object.Instantiate(bossBehavior.gasStation);
                newObj.SetActive(true);
                var effectMaster = newObj.GetComponent<AreaEffectMaster>();
                effectMaster.IsDisable = false;
                newObj.transform.position = new Vector3(gasPosition.x, 0, gasPosition.z);*/
            }).ConfigureAwait(false);
        }
        

        protected bool AreaDistance(BossBehavior bossBehavior, float minDistance)
        {
            /*if (bossBehavior.BossMaster.IsDead) return false;
            var targetPosition = bossBehavior.PlayerMaster.transform.position;
            var bossPosition = bossBehavior.transform.position;
            var distance = Vector3.Distance(bossPosition, targetPosition);
            return !(minDistance >= distance);*/
            return true;
        }

        #endregion
        
        
    }
}