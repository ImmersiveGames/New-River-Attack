using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using NewRiverAttack.SaveManagers;
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

        public bool HaveBuyAllProductInList(IEnumerable<ShopProductStock> shopProductList)
        {
            return shopProductList.Select(product => GameOptionsSave.instance.HaveProduct(product.shopProduct)).All(check => check);
        }
    }
}