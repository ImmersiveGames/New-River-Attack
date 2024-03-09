using ImmersiveGames.PlayerManagers.ScriptableObjects;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers
{
    [CreateAssetMenu(fileName = "ShopProductConsumable", menuName = "ImmersiveGames/Shopping/ShopProductConsumable", order = 202)]
    public class ShopProductConsumable: ShopProduct, IShopProductConsumable
    {
        [Range(1,10)]
        public int itemPackMultiply;
        public void Expend(PlayerSettings player, int quantity)
        {
            throw new System.NotImplementedException();
        }
    }
}