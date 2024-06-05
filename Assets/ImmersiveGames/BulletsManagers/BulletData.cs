using NewRiverAttack.ObstaclesSystems.Abstracts;

namespace ImmersiveGames.BulletsManagers
{
    public struct BulletData : IBulletsData
    {
        public int BulletDamage { get; set; }
        public float BulletOffSet { get; set; }
        public float BulletSpeed { get; set; }
        public float BulletTimer { get; set; }
        public bool BulletPowerUp { get; set; }
        public ObjectMaster BulletOwner { get; set; }
    }
}