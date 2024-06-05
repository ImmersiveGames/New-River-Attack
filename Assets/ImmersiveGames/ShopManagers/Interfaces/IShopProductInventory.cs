using System.Collections.Generic;
using ImmersiveGames.ShopManagers.ShopProducts;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface IShopProductInventory : IShopProduct
    {
        void AddPlayerProductList( int indexPlayer, IStockShop stockShop, int quantity);
        bool HaveBuyAllProductInList(IEnumerable<ShopProductStock> shopProductList);
    }
}