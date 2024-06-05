using System;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;

namespace ImmersiveGames.ObjectManagers.PowerUpManagers
{
    [Serializable]
    public struct PowerUpData
    {
        public PowerUpTypes powerUpType;
        public float duration;
        public bool accumulateDuration;
        public bool uniqueEffect;
    }
}