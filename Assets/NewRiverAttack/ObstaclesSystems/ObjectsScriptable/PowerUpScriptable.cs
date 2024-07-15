using ImmersiveGames.ObjectManagers.PowerUpManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ObjectsScriptable
{
    [CreateAssetMenu(fileName = "PowerUp", menuName = "ImmersiveGames/RiverAttack/PowerUp", order = 203)]
    public class PowerUpScriptable : CollectibleScriptable
    {
        [Header("PowerUp Settings")] 
        public PowerUpData powerUpData;
    }
}