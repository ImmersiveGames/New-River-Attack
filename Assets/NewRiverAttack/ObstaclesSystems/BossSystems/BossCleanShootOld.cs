using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossCleanShootOld : BossShootOld
    {
        private void Awake()
        {
            poolName = $"Pool ({nameof(BossCleanShootOld)})";
        }
        
        public void SetShoots(float cadence, int repeat)
        {
            timesRepeat = repeat <= 0 ? 3 : repeat;
            CadenceShoot = cadence <= 0 ? CadenceShoot : cadence;
        }
        
    }
}