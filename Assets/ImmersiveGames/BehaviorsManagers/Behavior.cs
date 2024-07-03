using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using UnityEngine;

namespace ImmersiveGames.BehaviorsManagers
{
    public abstract class Behavior : IBehavior
    {
        private const double MinimumDistance = 10;
        protected Behavior(string name, IBehavior[] subBehaviors)
        {
            Name = name;
            SubBehaviors = subBehaviors;
        }

        public string Name { get; }
        public IBehavior[] SubBehaviors { get; }

        public bool Initialized { get; set; }
        public bool Finalized { get; set; }

        public virtual Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Enter Behavior {Name}");
            Finalized = false;
            Initialized = false;
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Update Behavior {Name}");
            return Task.CompletedTask;
        }

        public virtual Task ExitAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Exit Behavior {Name}");
            Finalized = true;
            Initialized = false;
            return Task.CompletedTask;
        }

        protected string GetRandomNextBehavior(EnumDirectionBehavior exclude, Vector3 playerPosition)
        {
            var valid = false;
            var randomIndex = 0;
            var behaviorNames = Enum.GetNames(typeof(EnumDirectionBehavior))
                .Where(name => name != exclude.ToString())
                .ToList();
            randomIndex = UnityEngine.Random.Range(0, behaviorNames.Count);
            valid = IsMovePossible(behaviorNames[randomIndex], playerPosition);
            
            return behaviorNames[randomIndex];
        }
        private bool IsMovePossible(string behavior, Vector3 playerPosition)
        {
            var limitZ = GamePlayBossManager.instance.bossAreaZ;
            var limitX = GamePlayBossManager.instance.bossAreaX;

            if (behavior == "MoveNorthBehavior")
            {
                DebugManager.Log<Behavior>($"Vou pro Norte, posso? {playerPosition.z + MinimumDistance}");
                DebugManager.Log<Behavior>($"Máximo: {limitZ.y} {playerPosition.z + MinimumDistance < limitZ.y}");
            }
            if (behavior == "MoveSouthBehavior")
            {
                DebugManager.Log<Behavior>($"Vou pro Sul, posso? {playerPosition.z - MinimumDistance }");
                DebugManager.Log<Behavior>($"Máximo: {limitZ.x} {playerPosition.z - MinimumDistance >= limitZ.x}");
            }
            if (behavior == "MoveEastBehavior")
            {
                DebugManager.Log<Behavior>($"Vou pro Leste, posso? {playerPosition.x + MinimumDistance }");
                DebugManager.Log<Behavior>($"Máximo: {limitX.y} {playerPosition.x + MinimumDistance <= limitX.y}");
            }
            if (behavior == "MoveWestBehavior")
            {
                DebugManager.Log<Behavior>($"Vou pro Oeste, posso? {playerPosition.x - MinimumDistance}");
                DebugManager.Log<Behavior>($"Máximo: {limitX.x} {playerPosition.x - MinimumDistance >= limitX.x}");
            }

            return behavior switch
            {
                "MoveNorthBehavior" => playerPosition.z + MinimumDistance <= limitZ.y,
                "MoveSouthBehavior" => playerPosition.z - MinimumDistance >= limitZ.x,
                "MoveEastBehavior" => playerPosition.x + MinimumDistance <= limitX.x,
                "MoveWestBehavior" => playerPosition.x - MinimumDistance >= limitX.y,
                _ => false
            };
        }

        
    }
}