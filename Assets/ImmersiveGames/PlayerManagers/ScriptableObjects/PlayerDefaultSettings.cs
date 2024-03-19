using ImmersiveGames.ShopManagers.ShopProducts;
using UnityEngine;

namespace ImmersiveGames.PlayerManagers.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerSetting", menuName = "ImmersiveGames/RiverAttack/PlayerSettings", order = 102)]
    public class PlayerDefaultSettings: ScriptableObject
    {
        [Header("Skin Default")] public ShopProductSkin skinDefault;
    }
}