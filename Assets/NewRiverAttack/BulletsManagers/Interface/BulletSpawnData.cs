using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.BulletsManagers.Interface
{
    public class  BulletSpawnData : ISpawnData
    {
        public Vector3 Direction { get; set; }
        public ObjectMaster Owner { get; set; }
        public Vector3 Position { get; set; }

        public int Damage { get; set; }
        public float Speed { get; set; }
        public float Timer { get; set; }
        public bool PowerUp { get; set; }

        public BulletSpawnData(ObjectMaster owner, Vector3 direction, Vector3 position, int damage, float speed, float timer,
            bool powerUp)
        {
            Owner = owner;
            Direction = direction;
            Damage = damage;
            Speed = speed;
            Timer = timer;
            PowerUp = powerUp;
            Position = position;
        }
    }
}