using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers.Interfaces
{
    public interface IShootPattern
    {
        void Execute(Transform spawnPoint, ObjectShoot shooter);
        bool CanShoot();
    }
}