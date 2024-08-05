using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossCleanShoot : BossShoot
    {
        private void Awake()
        {
            poolName = $"Pool ({nameof(BossCleanShoot)})";
        }
        
        public void SetShoots(float cadence, int repeat)
        {
            timesRepeat = repeat <= 0 ? 3 : repeat;
            CadenceShoot = cadence <= 0 ? CadenceShoot : cadence;
        }
        
    }
}