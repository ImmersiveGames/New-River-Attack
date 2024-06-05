using ImmersiveGames.ShopManagers.ShopProducts;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AllPlayersSettings", menuName = "ImmersiveGames/RiverAttack/AllPlayerSettings", order = 101)]
    public class PlayersDefaultSettings: ScriptableObject
    {
        [Header("Player Spawn")]
        public GameObject playerPrefab;
        public Vector3 spawnPosition = Vector3.zero;
        public Vector3 spawnRotation = Vector3.zero;
        [Header("Player Default Settings")]
        public ShopProductSkin skinDefault;
        
        [Header("Player Start Settings")]
        public int maxBombs = 9;
        public int startBombs = 3;
        public int maxFuel = 100;
        public int maxLives = 9;
        public int startLives = 3;
        
    }
}