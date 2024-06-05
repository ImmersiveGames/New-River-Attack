using ImmersiveGames.ShopManagers.ShopProducts;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerSetting", menuName = "ImmersiveGames/RiverAttack/PlayerSettings", order = 102)]
    public class PlayerSettings: ScriptableObject
    {
        [Header("Player Custom Settings")] 
        public string playerName;
        
        [Header("Skin Atual")] 
        public ShopProductSkin actualSkin;
    }
}