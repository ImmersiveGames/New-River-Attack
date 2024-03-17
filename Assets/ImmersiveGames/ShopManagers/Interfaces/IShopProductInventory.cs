using System.Collections.Generic;
using ImmersiveGames.PlayerManagers.ScriptableObjects;
using ImmersiveGames.ShopManagers.ShopProducts;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProductInventory : IShopProduct
    {
        void AddPlayerProductList( IStockShop stockShop, int quantity);
    }
}