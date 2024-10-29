using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class TargetingSystem
    {
        public static void AimAtTarget(Transform spawnPoint, Transform target)
        {
            if (target == null) return;
            var directionToTarget = (target.position - spawnPoint.position).normalized;
            spawnPoint.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }
}