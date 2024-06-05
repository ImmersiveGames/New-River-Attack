using ImmersiveGames.ObjectManagers.PowerUpManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ObjectsScriptables
{
    [CreateAssetMenu(fileName = "PowerUp", menuName = "ImmersiveGames/RiverAttack/PowerUp", order = 203)]
    public class PowerUpScriptable : CollectibleScriptable
    {
        [Header("PowerUp Settings")] 
        public PowerUpData powerUpData;
    }
}