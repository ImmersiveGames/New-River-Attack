using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class TargetingSystem
    {
        public bool AimAtTarget(Transform spawnPoint, Transform target)
        {
            if (target == null) return false;
            var directionToTarget = (target.position - spawnPoint.position).normalized;
            spawnPoint.rotation = Quaternion.LookRotation(directionToTarget);
            return true;
        }
    }
}