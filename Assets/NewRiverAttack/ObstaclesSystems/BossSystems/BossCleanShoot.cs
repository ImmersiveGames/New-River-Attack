using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossCleanShoot : BossShoot
    {
        public int maxShoot;
        private int _actualNumShoot;
        private void Awake()
        {
            poolName = $"Pool ({nameof(BossCleanShoot)})";
            _actualNumShoot = 0;
        }
        
        public void SetShoots(int numMissile)
        {
            maxShoot = numMissile;
        }

        protected override void Fire()
        {
            base.Fire();
            _actualNumShoot++;
        }
        
        public bool EndCycle => _actualNumShoot > maxShoot;
    }
}