using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.ShopProducts
{
    [CreateAssetMenu(fileName = "ShopProductConsumable", menuName = "ImmersiveGames/Shopping/ShopProductConsumable", order = 202)]
    public class ShopProductUsable: ShopProduct, IShopProductUsable
    {
        [Range(1,10)]
        public int itemPackMultiply;

        public GameObject prefabItem;
        public void Use(int indexPlayer, IStockShop stockShop, int quantity)
        {
            throw new System.NotImplementedException();
        }
    }
}