using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.ShopProducts
{
    [CreateAssetMenu(fileName = "ShopProductBuy", menuName = "ImmersiveGames/Shopping/ShopProductBuy", order = 201)]
    public class ShopProductInventory: ShopProduct, IShopProductInventory
    {
        public void AddPlayerProductList(int indexPlayer, IStockShop stockShop, int quantity)
        {
            GameOptionsSave.instance.AddInventory(stockShop.shopProduct, quantity);
        }
    }
}