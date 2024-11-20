using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ShootSystems
{
    public abstract class ShootPatternBase : ScriptableObject
    {
        public abstract void Execute(Transform spawnPoint, ObjectShoot shooter);
    }
}