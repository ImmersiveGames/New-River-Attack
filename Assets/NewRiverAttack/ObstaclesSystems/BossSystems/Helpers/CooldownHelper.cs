using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class CooldownHelper
    {
        private readonly float _cooldownTime;
        private float _lastShootTime;

        public CooldownHelper(float cooldown)
        {
            _cooldownTime = cooldown;
            _lastShootTime = Time.realtimeSinceStartup;
        }

        public bool IsReady()
        {
            return Time.realtimeSinceStartup >= _lastShootTime + _cooldownTime;
        }

        public void Reset()
        {
            _lastShootTime = Time.realtimeSinceStartup;
        }
    }
}