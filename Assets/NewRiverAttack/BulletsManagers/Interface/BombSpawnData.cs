using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers.Interface
{
    public class BombSpawnData : ISpawnData
    {
        public Vector3 Direction { get; set; }
        public ObjectMaster Owner { get; set; }
        public Vector3 Position { get; set; }
        public int Damage { get; set; }
        public float Timer { get; set; }
        public readonly float BombRadius;
        public readonly float BombRadiusSpeed;
        public readonly float BombShakeForce;
        public readonly float BombShakeTime;
        public const float BulletOffSet = 2f;

        public BombSpawnData(ObjectMaster owner, int damage, float timer, float bombRadius, float bombRadiusSpeed, float bombShakeForce, float bombShakeTime)
        {
            Owner = owner;
            Damage = damage;
            Timer = timer;
            BombRadius = bombRadius;
            BombRadiusSpeed = bombRadiusSpeed;
            BombShakeForce = bombShakeForce;
            BombShakeTime = bombShakeTime;
        }
        
    }
}