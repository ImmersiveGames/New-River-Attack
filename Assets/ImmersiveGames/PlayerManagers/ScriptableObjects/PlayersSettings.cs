using System.Linq;
using UnityEngine;

namespace ImmersiveGames.PlayerManagers.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AllPlayersSettings", menuName = "ImmersiveGames/RiverAttack/AllPlayerSettings", order = 101)]
    public class PlayersSettings: ScriptableObject
    {
        public PlayerSettings[] playerSettings;
        
    }
}