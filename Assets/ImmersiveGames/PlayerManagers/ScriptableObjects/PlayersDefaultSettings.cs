using ImmersiveGames.ShopManagers.ShopProducts;
using UnityEngine;
using UnityEngine.Serialization;

namespace ImmersiveGames.PlayerManagers.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AllPlayersSettings", menuName = "ImmersiveGames/RiverAttack/AllPlayerSettings", order = 101)]
    public class PlayersDefaultSettings: ScriptableObject
    {
        [Header("Player Spawn")]
        public PlayerSettings[] playerSettings;
        public GameObject playerPrefab;
        public Vector3 spawnPosition = Vector3.zero;
        public Vector3 spawnRotation = Vector3.zero;
        [Header("Player Default Settings")]
        public ShopProductSkin skinDefault;
        public float playerSpeed;
        public float playerAgility;
        
        [Header("Player Start Settings")]
        public int maxBombs = 9;
        public int startBombs = 3;
        public int maxFuel = 100;
        public int startFuel = 100;
        public int maxLives = 9;
        public int startLives = 3;
        
        //Essa função é duplicada ela também existe em GameOptionSave, mas aqui ela define o padrão.
        public void SetSkinToPlayer(int indexPlayer, ShopProductSkin skin)
        {
            var player = playerSettings[indexPlayer];
            if(skin != null && skin == player.actualSkin) return;
            player.actualSkin = (skin == null) ? skinDefault : skin;
        }
        
    }
}