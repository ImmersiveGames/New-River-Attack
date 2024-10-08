using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Helpers
{
    public class CooldownSystem
    {
        private float _lastActionTime;

        public bool IsCooldownComplete(float cooldown)
        {
            return Time.time - _lastActionTime >= cooldown;
        }

        public void UpdateLastActionTime()
        {
            _lastActionTime = Time.time;
        }
    }
}