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
            GameOptionsSave.Instance.AddInventory(stockShop.ShopProduct, quantity);
        }

        public bool HaveBuyAllProductInList(IEnumerable<ShopProductStock> shopProductList)
        {
            return shopProductList.Select(product => GameOptionsSave.Instance.HaveProduct(product.ShopProduct)).All(check => check);
        }
    }
}