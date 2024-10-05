using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace ImmersiveGames.PoolSystems.Interfaces
{
    public interface ISpawnData
    {
        public Vector3 Direction { get; set; }
        public ObjectMaster Owner { get; set; }
    }
}