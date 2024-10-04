using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace ImmersiveGames.PoolSystems.Interfaces
{
    public interface ISpawnData
    {
        public int BulletDamage { get; set; }
        public float BulletOffSet { get; set; }
        public Vector3 BulletDirection { get; set; }
        public float BulletSpeed { get; set; }
        public float BulletTimer{ get; set; }
        public bool BulletPowerUp { get; set; }
        public ObjectMaster BulletOwner { get; set; }
    }
}