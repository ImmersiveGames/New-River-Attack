using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossCleanShoot : BossShoot
    {
        private void Awake()
        {
            poolName = $"Pool ({nameof(BossCleanShoot)})";
        }
    }
}