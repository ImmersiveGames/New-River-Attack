using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace ImmersiveGames.BulletsManagers
{
    public struct BombData : IBulletsData
    {
        public float BombRadius;
        public float BombRadiusSpeed;
        public float BombShakeForce;
        public float BombShakeTime;
        public long BombMillisecondsVibrate;
        public int BulletDamage { get; set; }
        public float BulletOffSet { get; set; }
        public Vector3 BulletDirection { get; set; }
        public float BulletSpeed { get; set; }
        public bool BulletPowerUp { get; set; }
        public float BulletTimer{ get; set; }
        public ObjectMaster BulletOwner { get; set; }
    }
}